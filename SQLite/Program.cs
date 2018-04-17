using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SQLite
{
    class Program
    {
        /*
         * https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/
         * Add-Migration Init
         * Update-Database
         * Testing in-memory https://docs.microsoft.com/en-us/ef/core/miscellaneous/testing/sqlite
         * Enable-Migrations looks to be obsolete
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

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var b in db.Blogs)
                {
                    Console.WriteLine(" - {0}", b.Url);
                }
            }
        }
    }
}
