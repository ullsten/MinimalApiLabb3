
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
            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

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

        //tables
        public class Person
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int PersonId { get; set; }

            [Required]
            [StringLength(30)]
            [DisplayName("First name")]
            public string? FirstName { get; set; } = default;

            [Required]
            [StringLength(30)]
            [DisplayName("Last name")]
            public string? LastName { get; set; } = default;

            [NotMapped]
            [DisplayName("Person")]
            public string FullName => $"{FirstName} {LastName}";

            [Required]
            [StringLength(20)]
            [DisplayName("Phone number")]
            public string? PhoneNumber { get; set; }

            public ICollection<Interest>? Interests { get; set; } //navigering
        }

        public class Interest
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int InterestId { get; set; }

            [Required]
            [StringLength(50)]
            [DisplayName("Title")]
            public string? InterestTitle { get; set; }

            [Required]
            [StringLength(250)]
            [DisplayName("Description")]
            public string? InterestDescription { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime Created { get; set; } = DateTime.Now;

            [ForeignKey("Persons")]
            public int? FK_PersonId { get; set; } = null;
            public Person? Persons { get; set; } //navigation
            public ICollection<Link>? Links { get; set; } //navigering
        }

            public class Link
            {
                [Key]
                [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public int LinkId { get; set; }

                [Required]
                [StringLength(20)]
                public string LinkTitle { get; set; }

                [Required]
                [StringLength(100)]
                public string URL { get; set; }

                [ForeignKey("Interest")]
                public int? FK_InterestId { get; set; } = null;
                public Interest? Interest { get; set; } // navigation property
            }
    }
}
