using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SQLite
{
    class Program
    {
        /*
         * Migrations https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/
         * Add-Migration Init
         * Update-Database
         * Testing in-memory https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sqlite
         * Enable-Migrations looks to be obsolete
         * 
         * Loading related date https://docs.microsoft.com/en-us/ef/core/querying/related-data
         */
        static void Main(string[] args)
        {

            using (var db = new BloggingContext())
            {
                db.Database.Migrate();

                var blog = new Blog { Url = "http://blogs.msdn.com/adonet", Posts = new List<Post>() };
                var post = new Post { Title = "Interesting", Content = "Curious" };
                blog.Posts.Add(post);

                db.Blogs.Add(blog);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
                Console.WriteLine("{0} BlogId", blog.BlogId);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");

                var blogs = db.Blogs
                    .Include(b => b.Posts)
                    .ToList();

                foreach (var b in blogs)
                {
                    Console.WriteLine(" - {0}", b.Url);
                    b.Posts.Add(new Post { CreateDate = DateTime.Now, Title = "title", Content = "content" });
                }
                count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
            }
        }
    }
}
