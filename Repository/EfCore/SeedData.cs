using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using entity.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Repository.EfCore
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MyContext>();
                if (context != null)
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }

                    if (!context.Users.Any())
                    {
                        context.Users.AddRange(
                            new AppUser { UserName = "serdalozsoy", Email = "info@serdalozsoy.com", PasswordHash = "123456", Image = "p1.jpg" },
                            new AppUser { UserName = "erensarı", Email = "info@erensarı.com", PasswordHash = "123456", Image = "p2.jpg" }
                        );
                        context.SaveChanges();
                    }

                    if (!context.Posts.Any())
                    {
                        context.Posts.AddRange(
                            new Post
                            {
                                Title = "Asp.net core",
                                Description = "Asp.net core dersleri",
                                Content = "Asp.net core dersleri",
                                Url = "aspnet-core",
                                IsActive = true,
                                PublishedOn = DateTime.Now.AddDays(-10),
                                UserId = 1,
                                Likes = new List<Like>
                                {
                                    new Like { UserId = 1, LikedOn = DateTime.Now.AddDays(-5) },
                                    new Like { UserId = 2, LikedOn = DateTime.Now.AddDays(-3) }
                                },
                                Image = "1.jpg",
                                Comments = new List<Comment>
                                {
                                    new Comment { Text = "iyi bir kurs", PublishedOn = DateTime.Now.AddDays(-20), UserId = 1 },
                                    new Comment { Text = "çok faydalandığım bir kurs", PublishedOn = DateTime.Now.AddDays(-10), UserId = 2 }
                                }
                            },
                            new Post
                            {
                                Title = "Php",
                                Description = "Php core dersleri",
                                Content = "Php core dersleri",
                                Url = "php",
                                IsActive = true,
                                Image = "2.jpg",
                                PublishedOn = DateTime.Now.AddDays(-20),
                                UserId = 1,               
                                Likes = new List<Like>
                                {
                                    new Like { UserId = 1, LikedOn = DateTime.Now.AddDays(-5) },
                                    new Like { UserId = 2, LikedOn = DateTime.Now.AddDays(-3) }
                                },
                                

                            },
                            new Post
                            {
                                Title = "Django",
                                Description = "Django dersleri",
                                Content = "Django dersleri",
                                Url = "django",
                                IsActive = true,
                                Image = "3.jpg",
                                PublishedOn = DateTime.Now.AddDays(-30),
                                Likes = new List<Like>
                                {
                                    new Like { UserId = 1, LikedOn = DateTime.Now.AddDays(-5) },
                                    new Like { UserId = 2, LikedOn = DateTime.Now.AddDays(-3) }
                                },
                                UserId = 2
                            },
                            new Post
                            {
                                Title = "React Dersleri",
                                Content = "React dersleri",
                                Url = "react-dersleri",
                                IsActive = true,
                                Image = "3.jpg",
                                PublishedOn = DateTime.Now.AddDays(-40),
                                Likes = new List<Like>
                                {
                                    new Like { UserId = 1, LikedOn = DateTime.Now.AddDays(-5) },
                                    new Like { UserId = 2, LikedOn = DateTime.Now.AddDays(-3) }
                                },
                                UserId = 2
                            },
                            new Post
                            {
                                Title = "Angular",
                                Content = "Angular dersleri",
                                Url = "angular",
                                IsActive = true,
                                Image = "3.jpg",
                                PublishedOn = DateTime.Now.AddDays(-50),
                                Likes = new List<Like>
                                {
                                    new Like { UserId = 1, LikedOn = DateTime.Now.AddDays(-5) },
                                    new Like { UserId = 2, LikedOn = DateTime.Now.AddDays(-3) }
                                },
                                UserId = 1
                            },
                            new Post
                            {
                                Title = "Web Tasarım",
                                Content = "Web tasarım dersleri",
                                Url = "web-tasarim",
                                IsActive = true,
                                Image = "3.jpg",
                                PublishedOn = DateTime.Now.AddDays(-60),
                                Likes = new List<Like>
                                {
                                    new Like { UserId = 1, LikedOn = DateTime.Now.AddDays(-5) },
                                    new Like { UserId = 2, LikedOn = DateTime.Now.AddDays(-3) }
                                },
                                UserId = 1
                            }
                        );
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
