using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Dto;
using WarehouseStores.Core.Dto.BillDtos;
using WarehouseStores.Core.Dto.CategoryDtos;
using WarehouseStores.Core.Dto.ProductDtos;
using WarehouseStores.Core.Dto.PurchaseOrderDtos;
using WarehouseStores.Core.Dto.StatusDtos;
using WarehouseStores.Core.Models;
using WarehouseStores.Repository.DBContext;

namespace WarehouseStores.Api.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Department mappings
            CreateMap<Departments, DepartmentDto>().ReverseMap();

            // Bill mappings
            CreateMap<BillAddDto, Bill>()
                .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
                .ForMember(dest => dest.StatusId, opt => opt.Ignore())
                .ForMember(dest => dest.PriorityId, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.ReceivedOrders, opt => opt.Ignore());

            CreateMap<AddBillRequestDto, Bill>()
                .ForMember(dest => dest.SupplierId, opt => opt.Ignore())
                .ForMember(dest => dest.DepartmentId, opt => opt.Ignore())
                .ForMember(dest => dest.StatusId, opt => opt.Ignore())
                .ForMember(dest => dest.PriorityId, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.ReceivedOrders, opt => opt.Ignore());

            CreateMap<Bill, BillDto>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customers))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.BillProducts.Select(bp => bp.Product)))
                .ForMember(dest => dest.RecievedOrders, opt => opt.MapFrom(src => src.ReceivedOrders));

            // Priority mappings
            CreateMap<Priority, PriorityDto>().ReverseMap();

            // Status mappings
            CreateMap<Status, StatusDto>().ReverseMap();

            // Category mappings
            CreateMap<CategoryDto, Categories>().ReverseMap();
            CreateMap<SimpleCategoryDto, Categories>().ReverseMap();

            // Product mappings
            CreateMap<ProductDto, Products>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Products, ProductDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null
                    ? new SimpleCategoryDto
                    {
                        Id = src.Category.Id,
                        Name = src.Category.Name
                    }
                    : null
                ));

            CreateMap<ProductAddDto, Products>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (src.Category != null)
                    {
                        dest.CategoryId = src.Category.Id;
                    }
                });

            CreateMap<Products, SimpleProductDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
              // .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));

            // ProductDates mappings
            CreateMap<ProductDates, ProductDatesDto>().ReverseMap();

            CreateMap<ProductWithExpiryDto, Products>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<ProductWithExpiryDto, ProductDates>()
                .ForMember(dest => dest.AddDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(src => src.ExpiryDate))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // PurchaseOrder mappings
            CreateMap<PurchaseOrders, PurchaseOrderDto>()
                .ForMember(dest => dest.DepartmentDto, opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.PriorityDto, opt => opt.MapFrom(src => src.Priority))
                .ForMember(dest => dest.StatusDto, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.PurchaseOrderProducts != null
                    ? src.PurchaseOrderProducts.Select(pop => new PurchaseOrderProductDto
                    {
                        ProductId = pop.ProductId,
                        ProductName = pop.Product.Name,
                        Quantity = pop.Quantity,
                        Weight = pop.Weight
                    }).ToList()
                    : new List<PurchaseOrderProductDto>()
                ));

            CreateMap<PurchaseOrderAddDto, PurchaseOrders>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => new Departments
                {
                    Name = src.DepartmentName
                }))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => new Priority
                {
                    Name = src.PriorityName
                }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new Status
                {
                    Name = src.StatusName
                }))
                .ForMember(dest => dest.PurchaseOrderProducts, opt => opt.MapFrom(src => src.Products.Select(p => new PurchaseOrderProducts
                {
                    ProductId = p.Id,
                    Quantity = p.Quantity,
                    Weight = p.Weight
                }).ToList()));

            CreateMap<PurchaseOrders, PurchaseOrderAddDto>().ReverseMap();

            
           
        }
    }





}








