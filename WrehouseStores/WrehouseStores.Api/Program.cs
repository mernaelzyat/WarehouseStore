
using Microsoft.EntityFrameworkCore;
using WarehouseStores.Api.Helper;
using WarehouseStores.Core.Interfaces;
using WarehouseStores.Repository.DBContext;
using WarehouseStores.Repository.Repositories;

namespace WrehouseStores.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var configuration = builder.Configuration;


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDbContext<StorageDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IPurchaseRepository , PurchaseOrderRepository>();
            builder.Services.AddScoped<IReceivedOrder , RecievedOrderRepository>();
            builder.Services.AddScoped<IStorageRepository, StorageRepository>();
            builder.Services.AddScoped<IBillRepository, BillRepository>();
            builder.Services.AddScoped<ICustomerOrdersRepository, CustomerOrderRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ISuppliersRepository, SupplierRepository>();
            builder.Services.AddScoped<IStatusRepository, StatusRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            builder.Services.AddLogging();

            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });

            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");

            app.MapControllers();

            app.Run();
        }
    }
}
