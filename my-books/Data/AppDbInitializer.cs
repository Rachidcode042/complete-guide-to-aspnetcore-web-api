using my_books.Data.Models;
using System.Reflection;

namespace my_books.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Books.Any())
                {
                    context.Books.AddRange(new Book()
                    {
                        Title = "1er Book Title",
                        Description = "1er Book Description",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(-10),
                        Rate = 4,
                        Genre = "Commedy",
                        CoverUrl = "https...",
                        DateAdded = DateTime.Now
                    },
                    new Book() 
                    {
                        Title = "2nd Book Title",
                        Description = "2nd Book Description",
                        IsRead = true,
                        Genre = "Action",
                        CoverUrl = "https....",
                        DateAdded = DateTime.Now

                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
