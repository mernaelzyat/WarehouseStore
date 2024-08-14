using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.BillDtos;
using WarehouseStores.Core.Dto.CustomerDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.StatusDtos;
using WarehouseStores.Core.Dto.SupplierDtos;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Repository.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly StorageDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BillRepository> _logger;
        public BillRepository(StorageDbContext context, IMapper mapper, ILogger<BillRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BillDto> AddPurchaseBillAsync(AddSimpleBillRequestDto billRequestDto)
        {
            if (billRequestDto == null)
            {
                throw new ArgumentNullException(nameof(billRequestDto));
            }

            if (billRequestDto.OrderId.HasValue && await _context.Bill.AnyAsync(b => b.OrderId == billRequestDto.OrderId.Value))
            {
                throw new ArgumentException($"معرف الطلبية '{billRequestDto.OrderId}' موجود بالفعل. من فضلك استخدم معرف مختلف.");
            }

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == billRequestDto.DepartmentName);

            if (department == null || department.Name != "المشتريات")
            {
                throw new ArgumentException("الفاتورة يجب أن تكون لقسم المشتريات فقط.");
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Name == billRequestDto.EmployeeName);
            if (employee == null)
            {
                throw new ArgumentException($"الموظف '{billRequestDto.EmployeeName}' غير متواجد. من فضلك اضف موظف جديد.");
            }
            if (billRequestDto.Email != employee.Email)
            {
                throw new ArgumentException($"البريد الإلكتروني للموظف '{billRequestDto.EmployeeName}' غير متطابق مع البريد المسجل. من فضلك استخدم البريد المسجل '{employee.Email}'.");
            }

            var bill = new Bill
            {
                Phone = billRequestDto.Phone,
                Email = billRequestDto.Email,
                Notes = billRequestDto.Notes,
                Date = billRequestDto.Date,
                TotalPrice = 0m, // Initialize TotalPrice
                OrderId = billRequestDto.OrderId ?? 0, // Ensure OrderId is assigned must be unique
                DepartmentId = department.Id,
                EmployeeId = employee.Id
            };

            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == billRequestDto.SupplierName);
            var status = await _context.Status.FirstOrDefaultAsync(s => s.Name == billRequestDto.StatusName);
            var priority = await _context.Priority.FirstOrDefaultAsync(p => p.Name == billRequestDto.PriorityName);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == billRequestDto.CustomerName);

            if (supplier == null)
            {
                throw new ArgumentException($"المورد '{billRequestDto.SupplierName}' غير متواجد من فضلك اضف مورد جديد.");
            }
            if (billRequestDto.Phone != supplier.Phone)
            {
                throw new ArgumentException($"رقم الهاتف للمورد '{billRequestDto.SupplierName}' غير متطابق مع الرقم المسجل. من فضلك استخدم الرقم المسجل '{supplier.Phone}'.");
            }
            if (status == null)
            {
                throw new ArgumentException($"الحالة '{billRequestDto.StatusName}' غير متواجدة من فضلك اضف الحالة جديد.");
            }
            if (priority == null)
            {
                throw new ArgumentException($"الأولوية '{billRequestDto.PriorityName}' غير متواجدة من فضلك اضف أولوية جديد.");
            }
            if (customer == null)
            {
                throw new ArgumentException($"العميل '{billRequestDto.CustomerName}' غير متواجد من فضلك اضف عميل جديد.");
            }

            bill.SupplierId = supplier.Id;
            bill.StatusId = status.Id;
            bill.PriorityId = priority.Id;
            bill.CustomerId = customer.Id;

            await _context.Bill.AddAsync(bill);
            await _context.SaveChangesAsync();

            var billProducts = new List<BillProducts>();
            var productDtos = new List<ProductDto>();
            decimal totalPrice = 0m;

            foreach (var productDto in billRequestDto.Products)
            {
                var existingProduct = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == productDto.Id);
                if (existingProduct != null)
                {
                    // تحقق من صحة السعر والوحدة والضريبة
                    if (productDto.Price != existingProduct.Price)
                    {
                        throw new ArgumentException($"السعر المدخل للمنتج '{existingProduct.Name}' غير صحيح. السعر الصحيح هو {existingProduct.Price}.");
                    }
                    if (productDto.Unit != existingProduct.Unit)
                    {
                        throw new ArgumentException($"الوحدة المدخلة للمنتج '{existingProduct.Name}' غير صحيحة. الوحدة الصحيحة هي {existingProduct.Unit}.");
                    }
                    if (productDto.SalesTax != existingProduct.SalesTax)
                    {
                        throw new ArgumentException($"الضريبة المدخلة للمنتج '{existingProduct.Name}' غير صحيحة. الضريبة الصحيحة هي {existingProduct.SalesTax}.");
                    }

                    decimal taxAmount = productDto.Price ?? 0 * (productDto.SalesTax ?? 0);
                    decimal priceAfterTax = productDto.Price ?? 0 + taxAmount;

                    var billProduct = new BillProducts
                    {
                        BillId = bill.Id,
                        ProductId = existingProduct.Id,
                        Quantity = productDto.Quantity,
                        Price = priceAfterTax
                    };

                    var productDate = productDto.ExpiryDate;
                    if (productDate.HasValue)
                    {
                        var existingProductDate = await _context.ProductDates.FirstOrDefaultAsync(pd => pd.ProductId == existingProduct.Id);
                        if (existingProductDate == null)
                        {
                            existingProductDate = new ProductDates
                            {
                                ExpiryDate = productDate.Value,
                                ProductId = existingProduct.Id
                            };
                            await _context.ProductDates.AddAsync(existingProductDate);
                        }
                        else
                        {
                            existingProductDate.ExpiryDate = productDate.Value;
                            _context.ProductDates.Update(existingProductDate);
                        }
                    }

                    billProducts.Add(billProduct);

                    decimal productTotalPrice = priceAfterTax * (productDto.Quantity ?? 0);
                    totalPrice += productTotalPrice;

                    existingProduct.Quantity += productDto.Quantity ?? 0;
                    existingProduct.TotalStock += productDto.Quantity ?? 0;

                    _context.Products.Update(existingProduct);

                    productDtos.Add(new ProductDto
                    {
                        Id = existingProduct.Id,
                        Name = existingProduct.Name,
                        Unit = existingProduct.Unit,
                        Weight = existingProduct.Weight,
                        Quantity = productDto.Quantity,
                        Price = priceAfterTax,
                        SalesTax = productDto.SalesTax,
                        Description = productDto.Description,
                        ProductDatesDto = new List<ProductDatesDto>
                {
                    new ProductDatesDto
                    {
                        ExpiryDate = productDate
                    }
                }
                    });
                }
                else
                {
                    throw new ArgumentException($"المنتج '{productDto.Id}' غير متواجد.");
                }
            }

            await _context.BillProducts.AddRangeAsync(billProducts);
            await _context.SaveChangesAsync();

            bill.TotalPrice = totalPrice;
            _context.Bill.Update(bill);
            await _context.SaveChangesAsync();

            var billDtoResponse = new BillDto
            {
                Id = bill.Id,
                OrderId = bill.OrderId,
                Phone = bill.Phone,
                Email = bill.Email,
                Notes = bill.Notes,
                Date = bill.Date,
                TotalPrice = bill.TotalPrice,
                Department = new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name
                },
                Supplier = new SupplierDto
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Email = supplier.Email,
                    Phone = supplier.Phone,
                    Address = supplier.Address
                },
                Status = new StatusDto
                {
                    Id = status.Id,
                    Name = status.Name
                },
                Priority = new PriorityDto
                {
                    Id = priority.Id,
                    Name = priority.Name
                },
                Customer = new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name
                },
                Employee = new EmployeeDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Job = employee.Job
                },
                Products = productDtos
            };

            return billDtoResponse;
        }


        public async Task<BillDto> AddSalesBillAsync(AddSimpleBillRequestDto billRequestDto)
        {
            if (billRequestDto == null)
            {
                throw new ArgumentNullException(nameof(billRequestDto));
            }

            if (billRequestDto.OrderId.HasValue && await _context.Bill.AnyAsync(b => b.OrderId == billRequestDto.OrderId.Value))
            {
                throw new ArgumentException($"معرف الطلبية '{billRequestDto.OrderId}' موجود بالفعل. من فضلك استخدم معرف مختلف.");
            }

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == billRequestDto.DepartmentName);

            if (department == null || department.Name == "المشتريات")
            {
                throw new ArgumentException("الفاتورة يجب أن تكون لقسم المبيعات أو المنتجات فقط.");
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Name == billRequestDto.EmployeeName);
            if (employee == null)
            {
                throw new ArgumentException($"الموظف '{billRequestDto.EmployeeName}' غير متواجد. من فضلك اضف موظف جديد.");
            }
            if (billRequestDto.Email != employee.Email)
            {
                throw new ArgumentException($"البريد الإلكتروني للموظف '{billRequestDto.EmployeeName}' غير متطابق مع البريد المسجل. من فضلك استخدم البريد المسجل '{employee.Email}'.");
            }

            var bill = new Bill
            {
                Phone = billRequestDto.Phone,
                Email = billRequestDto.Email,
                Notes = billRequestDto.Notes,
                Date = billRequestDto.Date,
                TotalPrice = 0m, // Initialize TotalPrice
                OrderId = billRequestDto.OrderId ?? 0, // Ensure OrderId is assigned must be unique
                DepartmentId = department.Id,
                EmployeeId = employee.Id
            };

            var status = await _context.Status.FirstOrDefaultAsync(s => s.Name == billRequestDto.StatusName);
            var priority = await _context.Priority.FirstOrDefaultAsync(p => p.Name == billRequestDto.PriorityName);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == billRequestDto.CustomerName);

            if (status == null)
            {
                throw new ArgumentException($"الحالة '{billRequestDto.StatusName}' غير متواجدة من فضلك اضف الحالة جديد.");
            }
            if (priority == null)
            {
                throw new ArgumentException($"الأولوية '{billRequestDto.PriorityName}' غير متواجدة من فضلك اضف أولوية جديد.");
            }
            if (customer == null)
            {
                throw new ArgumentException($"العميل '{billRequestDto.CustomerName}' غير متواجد من فضلك اضف عميل جديد.");
            }

            bill.StatusId = status.Id;
            bill.PriorityId = priority.Id;
            bill.CustomerId = customer.Id;

            await _context.Bill.AddAsync(bill);
            await _context.SaveChangesAsync();

            var billProducts = new List<BillProducts>();
            var productDtos = new List<ProductDto>();
            decimal totalPrice = 0m;

            foreach (var productDto in billRequestDto.Products)
            {
                var existingProduct = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == productDto.Id);
                if (existingProduct != null)
                {
                    // تحقق من صحة السعر والوحدة والضريبة
                    if (productDto.Price != existingProduct.Price)
                    {
                        throw new ArgumentException($"السعر المدخل للمنتج '{existingProduct.Name}' غير صحيح. السعر الصحيح هو {existingProduct.Price}.");
                    }
                    if (productDto.Unit != existingProduct.Unit)
                    {
                        throw new ArgumentException($"الوحدة المدخلة للمنتج '{existingProduct.Name}' غير صحيحة. الوحدة الصحيحة هي {existingProduct.Unit}.");
                    }
                    if (productDto.SalesTax != existingProduct.SalesTax)
                    {
                        throw new ArgumentException($"الضريبة المدخلة للمنتج '{existingProduct.Name}' غير صحيحة. الضريبة الصحيحة هي {existingProduct.SalesTax}.");
                    }

                    decimal taxAmount = productDto.Price ?? 0 * (productDto.SalesTax ?? 0);
                    decimal priceAfterTax = productDto.Price ?? 0 + taxAmount;

                    var billProduct = new BillProducts
                    {
                        BillId = bill.Id,
                        ProductId = existingProduct.Id,
                        Quantity = productDto.Quantity,
                        Price = priceAfterTax
                    };

                    var productDate = productDto.ExpiryDate;
                    if (productDate.HasValue)
                    {
                        var existingProductDate = await _context.ProductDates.FirstOrDefaultAsync(pd => pd.ProductId == existingProduct.Id);
                        if (existingProductDate == null)
                        {
                            existingProductDate = new ProductDates
                            {
                                ExpiryDate = productDate.Value,
                                ProductId = existingProduct.Id
                            };
                            await _context.ProductDates.AddAsync(existingProductDate);
                        }
                        else
                        {
                            existingProductDate.ExpiryDate = productDate.Value;
                            _context.ProductDates.Update(existingProductDate);
                        }
                    }

                    billProducts.Add(billProduct);

                    decimal productTotalPrice = priceAfterTax * (productDto.Quantity ?? 0);
                    totalPrice += productTotalPrice;

                    existingProduct.Quantity -= productDto.Quantity ?? 0;

                    _context.Products.Update(existingProduct);

                    productDtos.Add(new ProductDto
                    {
                        Id = existingProduct.Id,
                        Name = existingProduct.Name,
                        Unit = existingProduct.Unit,
                        Weight = existingProduct.Weight,
                        Quantity = productDto.Quantity,
                        Price = priceAfterTax,
                        SalesTax = productDto.SalesTax,
                        Description = productDto.Description,
                        ProductDatesDto = new List<ProductDatesDto>
                {
                    new ProductDatesDto
                    {
                        ExpiryDate = productDate
                    }
                }
                    });
                }
                else
                {
                    throw new ArgumentException($"المنتج '{productDto.Id}' غير متواجد.");
                }
            }

            await _context.BillProducts.AddRangeAsync(billProducts);
            await _context.SaveChangesAsync();

            bill.TotalPrice = totalPrice;
            _context.Bill.Update(bill);
            await _context.SaveChangesAsync();

            var billDtoResponse = new BillDto
            {
                Id = bill.Id,
                OrderId = bill.OrderId,
                Phone = bill.Phone,
                Email = bill.Email,
                Notes = bill.Notes,
                Date = bill.Date,
                TotalPrice = bill.TotalPrice,
                Department = new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name
                },
                Status = new StatusDto
                {
                    Id = status.Id,
                    Name = status.Name
                },
                Priority = new PriorityDto
                {
                    Id = priority.Id,
                    Name = priority.Name
                },
                Customer = new CustomerDto
                {
                    Id = customer.Id,
                    Name = customer.Name
                },
                Employee = new EmployeeDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Job = employee.Job
                },
                Products = productDtos
            };

            return billDtoResponse;
        }


        public async Task<BillDto> GetBillByIdAsync(int id)
        {
            var bill = await _context.Bill
                .Include(b => b.Department)
                .Include(b => b.Supplier)
                .Include(b => b.Status)
                .Include(b => b.Priority)
                .Include(b => b.BillProducts)
                    .ThenInclude(bp => bp.Product)
                    .ThenInclude(p => p.ProductDates)
               // .Include(b => b.ReceivedOrders)
                    //.ThenInclude(ro => ro.PurchaseOrder)
                .Include(b => b.Customers)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
            {
                _logger.LogWarning($"Bill with Id {id} not found.");
                return null;
            }

            var billDto = new BillDto
            {
                Id = id,
                OrderId = bill.OrderId,
                Phone = bill.Phone,
                Email = bill.Email,
                Notes = bill.Notes,
                Date = bill.Date,
                Price = bill.TotalPrice,
                Department = bill.Department != null ? new DepartmentDto
                {
                    Id = bill.Department.Id,
                    Name = bill.Department.Name
                } : null,
                Supplier = bill.Supplier != null ? new SupplierDto
                {
                    Id = bill.Supplier.Id,
                    Name = bill.Supplier.Name,
                    Phone = bill.Supplier.Phone,
                    Email = bill.Supplier.Email,
                    Address = bill.Supplier.Address
                } : null,
                Status = bill.Status != null ? new StatusDto
                {
                    Id = bill.Status.Id,
                    Name = bill.Status.Name
                } : null,
                Priority = bill.Priority != null ? new PriorityDto
                {
                    Id = bill.Priority.Id,
                    Name = bill.Priority.Name
                } : null,
                Customer = bill.CustomerId.HasValue ? new CustomerDto
                {
                    Id = bill.CustomerId.Value,
                    Name = bill.Customers?.Name,
                    Address = bill.Customers?.Address,
                    Email = bill.Customers?.Email,
                    CommercialRegister = bill.Customers?.CommercialRegister,
                    Phone = bill.Customers?.Phone
                } : null,
                Products = bill.BillProducts?.Select(bp => new ProductDto
                {
                    Id = bp.Product.Id,
                    Name = bp.Product.Name,
                    Unit = bp.Product.Unit,
                    Weight = bp.Product.Weight,
                    Quantity = bp.Quantity,
                    Price = bp.Product.Price,
                    SalesTax = bp.Product.SalesTax,
                    Description = bp.Product.Description,
                    ProductDatesDto = bp.Product.ProductDates?.Select(pd => new ProductDatesDto
                    {
                        AddDate = pd.AddDate,
                        ExpiryDate = pd.ExpiryDate
                    }).ToList()
                }).ToList(),
            };

            return billDto;
        }

        public async Task UpdatePurchaseBillAsync(AddSimpleBillRequestDto billDto)
        {
            var bill = await _context.Bill
                .Include(b => b.BillProducts)
                .ThenInclude(bp => bp.Product)
                .FirstOrDefaultAsync(b => b.Id == billDto.Id);

            if (bill == null)
            {
                throw new ArgumentException($"الفاتورة {billDto.Id} غير متواجدة.");
            }

            // Map fields from BillAddDto to existing bill entity manually
            bill.Phone = billDto.Phone;
            bill.Email = billDto.Email;
            bill.Notes = billDto.Notes;
            bill.Date = billDto.Date;

            // Fetch related entities from the database based on names provided in billDto
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.Name == billDto.SupplierName);
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == billDto.DepartmentName);
            var status = await _context.Status.FirstOrDefaultAsync(s => s.Name == billDto.StatusName);
            var priority = await _context.Priority.FirstOrDefaultAsync(p => p.Name == billDto.PriorityName);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == billDto.CustomerName);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Name == billDto.EmployeeName);
            var existingOrder = await _context.Bill.FirstOrDefaultAsync(o => o.OrderId == billDto.OrderId);

            if (existingOrder != null && existingOrder.Id != billDto.Id)
            {
                throw new ArgumentException($"معرف المنتج '{billDto.OrderId}' موجود بالفعل. من فضلك استخدم معرف مختلف.");
            }

            // Validate fetched entities
            if (supplier == null)
            {
                throw new ArgumentException($"المورد '{billDto.SupplierName}' غير متواجد من فضلك اضف مورد جديد.");
            }
            if (billDto.Email != supplier.Email)
            {
                throw new ArgumentException($"البريد الإلكتروني للمورد '{billDto.SupplierName}' غير متطابق مع البريد المسجل. من فضلك استخدم البريد المسجل '{supplier.Email}'.");
            }
            if (billDto.Phone != supplier.Phone)
            {
                throw new ArgumentException($"رقم الهاتف للمورد '{billDto.SupplierName}' غير متطابق مع الرقم المسجل. من فضلك استخدم الرقم المسجل '{supplier.Phone}'.");
            }
            if (department == null)
            {
                throw new ArgumentException($"القسم '{billDto.DepartmentName}' غير متواجد من فضلك اضف قسم جديد.");
            }
            if (status == null)
            {
                throw new ArgumentException($"الحالة '{billDto.StatusName}' غير متواجدة من فضلك اضف الحالة جديدة.");
            }
            if (priority == null)
            {
                throw new ArgumentException($"الأولوية '{billDto.PriorityName}' غير متواجدة من فضلك اضف أولوية جديدة.");
            }
            if (customer == null)
            {
                throw new ArgumentException($"العميل '{billDto.CustomerName}' غير متواجد من فضلك اضف عميل جديد.");
            }
            if (employee == null)
            {
                throw new ArgumentException($"الموظف '{billDto.EmployeeName}' غير متواجد من فضلك اضف موظف جديد.");
            }

            // Update IDs for related entities in the Bill entity
            bill.OrderId = billDto.OrderId.Value;
            bill.SupplierId = supplier.Id;
            bill.DepartmentId = department.Id;
            bill.StatusId = status.Id;
            bill.PriorityId = priority.Id;
            bill.CustomerId = customer.Id;
            bill.EmployeeId = employee.Id;

            // Remove existing BillProducts entries
            _context.BillProducts.RemoveRange(bill.BillProducts);
            await _context.SaveChangesAsync();

            // Add new BillProducts entries
            var billProducts = new List<BillProducts>();
            decimal totalPrice = 0;

            foreach (var productDto in billDto.Products)
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productDto.Id);
                if (existingProduct != null)
                {
                    // تحقق من صحة السعر والوحدة والضريبة
                    if (productDto.Price != existingProduct.Price)
                    {
                        throw new ArgumentException($"السعر المدخل للمنتج '{existingProduct.Name}' غير صحيح. السعر الصحيح هو {existingProduct.Price}.");
                    }
                    if (productDto.Unit != existingProduct.Unit)
                    {
                        throw new ArgumentException($"الوحدة المدخلة للمنتج '{existingProduct.Name}' غير صحيحة. الوحدة الصحيحة هي {existingProduct.Unit}.");
                    }
                    if (productDto.SalesTax != existingProduct.SalesTax)
                    {
                        throw new ArgumentException($"الضريبة المدخلة للمنتج '{existingProduct.Name}' غير صحيحة. الضريبة الصحيحة هي {existingProduct.SalesTax}.");
                    }

                    decimal taxAmount = existingProduct.Price * (existingProduct.SalesTax ?? 0);
                    decimal priceAfterTax = existingProduct.Price + taxAmount;

                    var billProduct = new BillProducts
                    {
                        BillId = bill.Id,
                        ProductId = existingProduct.Id,
                        Quantity = productDto.Quantity,
                        Price = existingProduct.Price
                    };

                    var existingProductDate = await _context.ProductDates.FirstOrDefaultAsync(pd => pd.ProductId == existingProduct.Id);
                    if (existingProductDate == null)
                    {
                        existingProductDate = new ProductDates
                        {
                            ExpiryDate = productDto.ExpiryDate,
                            ProductId = existingProduct.Id
                        };
                        await _context.ProductDates.AddAsync(existingProductDate);
                    }
                    else
                    {
                        existingProductDate.ExpiryDate = productDto.ExpiryDate;
                        _context.ProductDates.Update(existingProductDate);
                    }

                    billProducts.Add(billProduct);

                    decimal productTotalPrice = priceAfterTax * (productDto.Quantity ?? 0);
                    totalPrice += productTotalPrice;

                    // Update the product quantity based on the department
                    if (department.Name == "المبيعات")
                    {
                        existingProduct.Quantity -= productDto.Quantity ?? 0;
                    }
                    else if (department.Name == "المشتريات")
                    {
                        existingProduct.Quantity += productDto.Quantity ?? 0;
                        existingProduct.TotalStock += productDto.Quantity ?? 0;
                    }

                    // Save the updated product quantity
                    _context.Products.Update(existingProduct);
                }
                else
                {
                    throw new ArgumentException($"المنتج '{productDto.Id}' غير متواجد.");
                }
            }

            await _context.BillProducts.AddRangeAsync(billProducts);
            await _context.SaveChangesAsync();

            // Update the total price of the bill
            bill.TotalPrice = totalPrice;

            // Update the bill entity
            _context.Bill.Update(bill);
            await _context.SaveChangesAsync();
        }



        public async Task UpdateSalesBillAsync(AddSimpleBillRequestDto billDto)
        {
            var bill = await _context.Bill
                .Include(b => b.BillProducts)
                .ThenInclude(bp => bp.Product)
                .FirstOrDefaultAsync(b => b.Id == billDto.Id);

            if (bill == null)
            {
                throw new ArgumentException($"الفاتورة {billDto.Id} غير متواجدة.");
            }

            // Map fields from BillAddDto to existing bill entity manually
            bill.Phone = billDto.Phone;
            bill.Email = billDto.Email;
            bill.Notes = billDto.Notes;
            bill.Date = billDto.Date;

            // Fetch related entities from the database based on names provided in billDto
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name == billDto.DepartmentName);
            var status = await _context.Status.FirstOrDefaultAsync(s => s.Name == billDto.StatusName);
            var priority = await _context.Priority.FirstOrDefaultAsync(p => p.Name == billDto.PriorityName);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == billDto.CustomerName);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Name == billDto.EmployeeName);
            var existingOrder = await _context.Bill.FirstOrDefaultAsync(o => o.OrderId == billDto.OrderId);

            if (existingOrder != null && existingOrder.Id != billDto.Id)
            {
                throw new ArgumentException($"معرف المنتج '{billDto.OrderId}' موجود بالفعل. من فضلك استخدم معرف مختلف.");
            }

            // Validate fetched entities
            if (billDto.Email != customer.Email)
            {
                throw new ArgumentException($"البريد الإلكتروني للعميل '{billDto.CustomerName}' غير متطابق مع البريد المسجل. من فضلك استخدم البريد المسجل '{customer.Email}'.");
            }
            if (billDto.Phone != customer.Phone)
            {
                throw new ArgumentException($"رقم الهاتف للعميل '{billDto.CustomerName}' غير متطابق مع الرقم المسجل. من فضلك استخدم الرقم المسجل '{customer.Phone}'.");
            }
            if (department == null)
            {
                throw new ArgumentException($"القسم '{billDto.DepartmentName}' غير متواجد من فضلك اضف قسم جديد.");
            }
            if (status == null)
            {
                throw new ArgumentException($"الحالة '{billDto.StatusName}' غير متواجدة من فضلك اضف الحالة جديدة.");
            }
            if (priority == null)
            {
                throw new ArgumentException($"الأولوية '{billDto.PriorityName}' غير متواجدة من فضلك اضف أولوية جديدة.");
            }
            if (customer == null)
            {
                throw new ArgumentException($"العميل '{billDto.CustomerName}' غير متواجد من فضلك اضف عميل جديد.");
            }
            if (employee == null)
            {
                throw new ArgumentException($"الموظف '{billDto.EmployeeName}' غير متواجد من فضلك اضف موظف جديد.");
            }

            // Update IDs for related entities in the Bill entity
            bill.OrderId = billDto.OrderId.Value;
            bill.DepartmentId = department.Id;
            bill.StatusId = status.Id;
            bill.PriorityId = priority.Id;
            bill.CustomerId = customer.Id;
            bill.EmployeeId = employee.Id;

            // Remove existing BillProducts entries
            _context.BillProducts.RemoveRange(bill.BillProducts);
            await _context.SaveChangesAsync();

            // Add new BillProducts entries
            var billProducts = new List<BillProducts>();
            decimal totalPrice = 0;

            foreach (var productDto in billDto.Products)
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productDto.Id);
                if (existingProduct != null)
                {
                    // تحقق من صحة السعر والوحدة والضريبة
                    if (productDto.Price != existingProduct.Price)
                    {
                        throw new ArgumentException($"السعر المدخل للمنتج '{existingProduct.Name}' غير صحيح. السعر الصحيح هو {existingProduct.Price}.");
                    }
                    if (productDto.Unit != existingProduct.Unit)
                    {
                        throw new ArgumentException($"الوحدة المدخلة للمنتج '{existingProduct.Name}' غير صحيحة. الوحدة الصحيحة هي {existingProduct.Unit}.");
                    }
                    if (productDto.SalesTax != existingProduct.SalesTax)
                    {
                        throw new ArgumentException($"الضريبة المدخلة للمنتج '{existingProduct.Name}' غير صحيحة. الضريبة الصحيحة هي {existingProduct.SalesTax}.");
                    }

                    decimal taxAmount = existingProduct.Price * (existingProduct.SalesTax ?? 0);
                    decimal priceAfterTax = existingProduct.Price + taxAmount;

                    var billProduct = new BillProducts
                    {
                        BillId = bill.Id,
                        ProductId = existingProduct.Id,
                        Quantity = productDto.Quantity,
                        Price = existingProduct.Price
                    };

                    var existingProductDate = await _context.ProductDates.FirstOrDefaultAsync(pd => pd.ProductId == existingProduct.Id);
                    if (existingProductDate == null)
                    {
                        existingProductDate = new ProductDates
                        {
                            ExpiryDate = productDto.ExpiryDate,
                            ProductId = existingProduct.Id
                        };
                        await _context.ProductDates.AddAsync(existingProductDate);
                    }
                    else
                    {
                        existingProductDate.ExpiryDate = productDto.ExpiryDate;
                        _context.ProductDates.Update(existingProductDate);
                    }

                    billProducts.Add(billProduct);

                    decimal productTotalPrice = priceAfterTax * (productDto.Quantity ?? 0);
                    totalPrice += productTotalPrice;

                    // Update the product quantity based on the department
                    if (department.Name == "المبيعات")
                    {
                        existingProduct.Quantity -= productDto.Quantity ?? 0;
                    }
                    else if (department.Name == "المشتريات")
                    {
                        existingProduct.Quantity += productDto.Quantity ?? 0;
                        existingProduct.TotalStock += productDto.Quantity ?? 0;
                    }

                    // Save the updated product quantity
                    _context.Products.Update(existingProduct);
                }
                else
                {
                    throw new ArgumentException($"المنتج '{productDto.Id}' غير متواجد.");
                }
            }

            await _context.BillProducts.AddRangeAsync(billProducts);
            await _context.SaveChangesAsync();

            // Update the total price of the bill
            bill.TotalPrice = totalPrice;

            // Update the bill entity
            _context.Bill.Update(bill);
            await _context.SaveChangesAsync();
        }


        public async Task<ProductionBillWithPaginationDto> GetSupplyFormForProductionAsync(int pageNumber, int pageSize)
        {
            // Calculate total number of records
            var totalRecords = await _context.Bill
                .Where(b => b.Department.Name == "الانتاج")
                .CountAsync();

            // Retrieve the paged data
            var productionBills = await _context.Bill
                .Where(b => b.Department.Name == "الانتاج")
                .OrderBy(b => b.Date) // Ensure there's an order, adjust as needed
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new ProductionBillDto
                {
                    Id = b.Id,
                    DepartmentName = b.Department.Name,
                    PriorityName = b.Priority.Name,
                    Date = b.Date,
                    Notes = b.Notes
                })
                .ToListAsync();

            // Prepare the pagination info
            var pagination = new PaginationDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = totalRecords,
                HasNextPage = (pageNumber * pageSize) < totalRecords,
                HasPreviousPage = pageNumber > 1
            };

            // Return the paginated result
            return new ProductionBillWithPaginationDto
            {
                Pagination = pagination,
                ProductionBills = productionBills
            };
        }

        public async Task<IEnumerable<ProductionBillDto>> SearchProductionBillsAsync(DateTime? date, int? month, int? year)
        {
            var query = _context.Bill
                .Where(b => b.Department.Name == "الانتاج")
                .AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(b => b.Date.HasValue && b.Date.Value.Date == date.Value.Date);
            }
            else
            {
                if (month.HasValue)
                {
                    query = query.Where(b => b.Date.HasValue && b.Date.Value.Month == month.Value);
                }
                if (year.HasValue)
                {
                    query = query.Where(b => b.Date.HasValue && b.Date.Value.Year == year.Value);
                }
            }

            return await query
                .Select(b => new ProductionBillDto
                {
                    Id = b.Id,
                    DepartmentName = b.Department != null ? b.Department.Name : "Unknown",
                    PriorityName = b.Priority != null ? b.Priority.Name : "Unknown",
                    Date = b.Date,
                    Notes = b.Notes
                })
                .ToListAsync();
        }

        public async Task<SalesSupplyFormWithPaginationDto> GetSalesSupplyFormAsync(int pageNumber, int pageSize)
        {
            // Calculate total number of records
            var totalRecords = await _context.Bill
                .Where(b => b.Department.Name == "المبيعات")
                .CountAsync();

            // Retrieve the paged data
            var salesSupplyForms = await _context.Bill
                .Where(b => b.Department.Name == "المبيعات")
                .OrderBy(b => b.Date) // Ensure there's an order, adjust as needed
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new SalesSupplyFormDto
                {
                    Id = b.Id,
                    ProductId = b.BillProducts.FirstOrDefault().ProductId,
                    DepartmentName = b.Department.Name,
                    Date = b.Date,
                    Notes = b.Notes,
                    Quantity = b.BillProducts.FirstOrDefault().Quantity
                })
                .ToListAsync();

            // Prepare the pagination info
            var pagination = new PaginationDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Total = totalRecords,
                HasNextPage = (pageNumber * pageSize) < totalRecords,
                HasPreviousPage = pageNumber > 1
            };

            // Return the paginated result
            return new SalesSupplyFormWithPaginationDto
            {
                Pagination = pagination,
                SalesSupplyForms = salesSupplyForms
            };
        }

        public async Task<IEnumerable<SalesSupplyFormDto>> SearchSalesSupplyFormAsync(DateTime? date, int? month, int? year)
        {
            var query = _context.Bill
                .Where(b => b.Department.Name == "المبيعات")
                .AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(b => b.Date.HasValue && b.Date.Value.Date == date.Value.Date);
            }
            else
            {
                if (month.HasValue)
                {
                    query = query.Where(b => b.Date.HasValue && b.Date.Value.Month == month.Value);
                }
                if (year.HasValue)
                {
                    query = query.Where(b => b.Date.HasValue && b.Date.Value.Year == year.Value);
                }
            }

            return await query
                .Select(b => new SalesSupplyFormDto
                {
                    Id = b.Id,
                    ProductId = b.BillProducts.FirstOrDefault() != null ? b.BillProducts.FirstOrDefault().ProductId : 0,
                    DepartmentName = b.Department != null ? b.Department.Name : "Unknown",
                    Date = b.Date,
                    Notes = b.Notes,
                    Quantity = b.BillProducts.FirstOrDefault() != null ? b.BillProducts.FirstOrDefault().Quantity : 0
                })
                .ToListAsync();
        }







    }


}


