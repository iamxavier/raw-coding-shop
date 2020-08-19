﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RawCoding.Shop.Database;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManger = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    if (env.IsDevelopment())
                    {
                        context.Add(new Product
                        {
                            Name = "Test",
                            Description = "Test Product",
                            Collection = "Original",
                            Slug = "original-test",
                            Value = 1000,
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "book.jpg"},
                                new Image {Index = 1, Path = "book3.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "Test2",
                            Description = "Test Product 22",
                            Value = 2220,
                            Collection = "Original",
                            Slug = "original-test2",
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "pen.jpg"},
                                new Image {Index = 1, Path = "shirt3.jpg"},
                            }
                        });


                        context.Add(new Product
                        {
                            Name = "Test 33",
                            Description = "Test Product 313",
                            Value = 333,
                            Collection = "Original",
                            Slug = "original-test-33",
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "shirt.jpg"},
                                new Image {Index = 1, Path = "book1.jpg"},
                            }
                        });


                        context.SaveChanges();
                    }

                    if (!context.Users.Any())
                    {
                        var adminUser = new IdentityUser()
                        {
                            UserName = "Admin"
                        };

                        userManger.CreateAsync(adminUser, "password").GetAwaiter().GetResult();

                        var adminClaim = new Claim("Role", "Admin");

                        userManger.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}