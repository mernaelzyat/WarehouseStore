using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseStores.Core.Models;

namespace WarehouseStores.Repository.DBContext
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BillProducts>()
       .Property(b => b.Price)
       .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<CustomerOrdersProducts>()
                .Property(c => c.Weight)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Products>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Products>()
                .Property(p => p.Weight)
                .HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Customers)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
          .HasIndex(b => b.OrderId)
          .IsUnique();

            modelBuilder.Entity<Bill>()
                .Property(b => b.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<BillProducts>()
       .HasKey(bp => new { bp.BillId, bp.ProductId });

            modelBuilder.Entity<BillProducts>()
                .HasOne(bp => bp.Bill)
                .WithMany(b => b.BillProducts)
                .HasForeignKey(bp => bp.BillId);

            modelBuilder.Entity<BillProducts>()
                .HasOne(bp => bp.Product)
                .WithMany(p => p.BillProducts)
                .HasForeignKey(bp => bp.ProductId);

            modelBuilder.Entity<Products>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ReceivedOrders>()
                .HasOne(ro => ro.Department)
                .WithMany(d => d.ReceivedOrders)
                .HasForeignKey(ro => ro.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerOrders>()
                .HasMany(co => co.CustomerOrdersProducts)
                .WithOne(cop => cop.CustomerOrder)
                .HasForeignKey(cop => cop.CustomerOrderId) // Add this line for foreign key specification
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReceivedOrders>()
                .HasOne(ro => ro.Storage)
                .WithMany(s => s.ReceivedOrders)
                .HasForeignKey(ro => ro.StorageId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReceivedOrders>()
                .HasOne(ro => ro.PurchaseOrder)
                .WithOne(po => po.ReceivedOrder)
                .HasForeignKey<ReceivedOrders>(ro => ro.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReadyOrdersProducts>()
        .Property(p => p.Weight)
        .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ReadyOrdersProducts>()
                .HasOne(rop => rop.ReadyOrder)
                .WithMany(ro => ro.ReadyOrderProducts)
                .HasForeignKey(rop => rop.ReadyOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReadyOrdersProducts>()
                .HasOne(rop => rop.Product)
                .WithMany(p => p.ReadyOrderProducts)
                .HasForeignKey(rop => rop.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();
            });
            modelBuilder.Entity<PurchaseOrderProducts>()
           .HasKey(po => po.Id);

            modelBuilder.Entity<PurchaseOrderProducts>()
                .HasOne(po => po.PurchaseOrder)
                .WithMany(p => p.PurchaseOrderProducts)
                .HasForeignKey(po => po.PurchaseOrderId);

            modelBuilder.Entity<PurchaseOrderProducts>()
                .HasOne(po => po.Product)
                .WithMany(p => p.PurchaseOrderProducts)
                .HasForeignKey(po => po.ProductId);

            modelBuilder.Entity<StorageDepartment>()
           .HasKey(sd => new { sd.StorageId, sd.DepartmentId });

            modelBuilder.Entity<StorageDepartment>()
                .HasOne(sd => sd.Storage)
                .WithMany(s => s.StorageDepartments)
                .HasForeignKey(sd => sd.StorageId);

            modelBuilder.Entity<StorageDepartment>()
                .HasOne(sd => sd.Department)
                .WithMany(d => d.StorageDepartments)
                .HasForeignKey(sd => sd.DepartmentId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Bill> Bill { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<CustomerOrders> CustomerOrders { get; set; }
        public DbSet<CustomerOrdersProducts> CustomerOrderProducts { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Priority> Priority { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ProductDates> ProductDates { get; set; }
        public DbSet<PurchaseOrders> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderProducts> PurchaseOrderProducts { get; set; }
        public DbSet<ReceivedOrders> ReceivedOrders { get; set; }
        public DbSet<ReadyOrdersProducts> ReadyOrderProducts { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<Floor> Floor { get; set; }
        public DbSet<Shelf> Shelf { get; set; }
        public DbSet<ReadyOrders> ReadyOrders { get; set; }
      
        public DbSet<BillProducts> BillProducts { get; set; }
        public DbSet<StorageDepartment> StorageDepartments { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<MissingProducts> MissingProducts { get; set; }


    }

}
