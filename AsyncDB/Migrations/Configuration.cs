namespace AsyncDB.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    internal sealed class Configuration : DbMigrationsConfiguration<AsyncDB.BenchmarkModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AsyncDB.BenchmarkModelContext context)
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

            Random r = new Random();
            List<Blog> blogs = new List<Blog>();

            string loremipsum = File.ReadAllText(@"D:\VSO\Sandbox\Samples\AsyncDB\loremipsum.txt");

            for (int i = 1; i < 500; i++)
            {
                blogs.Add(new Blog()
                {
                    Name = $"Blog #{i}",
                    Description = loremipsum.Substring(0, r.Next(0, loremipsum.Length)),
                    Image = new byte[r.Next(1024, 102400)],
                    Posts = new List<Post>()
                });

                if (i <= 10)
                {
                    for (int j = 1; j < 500; j++)
                    {
                        blogs[i-1].Posts.Add(new Post() { Title = $"Post #{j}", Content = loremipsum.Substring(0, r.Next(0, loremipsum.Length)), CreatedDate = DateTime.Now });
                    }
                }
            }

            context.Blogs.AddOrUpdate(blogs.ToArray());
        }
    }
}
