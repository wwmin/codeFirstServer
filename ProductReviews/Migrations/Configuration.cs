namespace ProductReviews.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;
    internal sealed class Configuration : DbMigrationsConfiguration<ProductReviews.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProductReviews.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Products.AddOrUpdate(p => p.ProductId,
                new Product { Name = "Product 1", Category = "Category 1", Price = 200 },
                new Product { Name = "Product 2", Category = "Category 2", Price = 500 },
                new Product { Name = "Product 3", Category = "Category 3", Price = 700 }
                );

            context.Reviews.AddOrUpdate(r => r.ReviewId,
                 new Review { Title = "Review 1", Description = "Test review 1", ProductId = 1 },
                 new Review { Title = "Review 2", Description = "Test review 2", ProductId = 1 }
                 );
        }
    }
}
