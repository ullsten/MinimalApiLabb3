
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using MinimalApiLabb3.Data;
using Microsoft.AspNetCore.Mvc;
using MinimalApiLabb3.DTO.PersonDTO;
using System;
using MinimalApiLabb3.DTO.InterestDTO;
using MinimalApiLabb3.DTO.LinkDTO;
using MinimalApiLabb3.Endpoints;
using Microsoft.OpenApi.Models;

namespace MinimalApiLabb3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<Labb3MinmalContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc(name: "v1", info: new OpenApiInfo { Title = "Asp.Net Core Minimal API", Version = "v1" });
            });
            

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPersonEndpoints();
                endpoints.MapInterestEndpoints();
                endpoints.MapLinkEndpoints();

                //with repository
                endpoints.MapPersonRepoEndpoints();
            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerUI();
               //app.UseSwagger();
            }

            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.DocumentTitle = "Learning minimal API";
                swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", name: "Api that gives you a possibility to learn.");
                swaggerUiOptions.RoutePrefix = string.Empty;
            });



            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<Labb3MinmalContext>();
                //check if database exist, if not create database
                context.Database.EnsureCreated();
                //Add fake data to database
                SeedData.Initialize(context);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

      
            app.Run();
        }
    }
}
