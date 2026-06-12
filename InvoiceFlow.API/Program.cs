using InvoiceFlow.API.Services;
using InvoiceFlow.DAL.Models;
using InvoiceFlow.DAL.Repositories.Implementations;
using InvoiceFlow.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceFlow.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<InvoiceFlowDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IInvoiceFlowRepository, InvoiceFlowRepository>();
            builder.Services.AddScoped<JwtService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());

            app.MapControllers();

            app.Run();
        }
    }
}