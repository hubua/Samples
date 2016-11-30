using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncDB
{

    /*
    
        EF http://msdn.com/data/ef
    

        Multiple repositories and UoW http://www.asp.net/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    
        Not quite http://stackoverflow.com/questions/28543293/entity-framework-async-operation-takes-ten-times-as-long-to-complete

        Do not use proxies
        Use SQL stored procedures
    */
    class Program
    {
        private static SqlConnection GetOpenSQLConnection()
        {
            var result = new SqlConnection(@"Data Source=KBP1-LHP-F54065\SQLEXPRESS2014;Initial Catalog=Benchmark;User ID=sa;Password=12345678;Persist Security Info=true;Integrated Security=true;MultipleActiveResultSets=True;App=EntityFramework");
            result.Open();
            return result;
        }

        public static Blog ReadDB(int blogId)
        {
            // Read Blog and include non-lazy Posts
            using (var context = new BenchmarkModelContext())
            {
                return context.Blogs.Where(item => item.Id == blogId).Include(item => item.Posts).Single();
            }
        }

        public static async Task<Blog> ReadDBAsync(int blogId, bool delay = false)
        {
            // Read Blog and include non-lazy Posts
            using (var context = new BenchmarkModelContext())
            {
                if (delay)
                {
                    await Task.Delay(10000);
                }
                return await context.Blogs.Where(item => item.Id == blogId).Include(item => item.Posts).SingleAsync();
            }
        }

        public static Blog ReadDBSimple(int blogId)
        {
            // Read Blog only
            using (var context = new BenchmarkModelContext())
            {
                return context.Blogs.Where(item => item.Id == blogId).Single();
            }
        }

        public static async Task<Blog> ReadDBSimpleAsync(int blogId)
        {
            // Read Blog only
            using (var context = new BenchmarkModelContext())
            {
                return await context.Blogs.Where(item => item.Id == blogId).SingleAsync();
            }
        }

        static void Main(string[] args)
        {
            const int BIG_BLOG_1_ID = 1;
            const int BIG_BLOG_2_ID = 2;
            const int BIG_BLOG_3_ID = 3;
            
            const string RANDOM_POST_TITLE = "Post #10";

            System.Diagnostics.Stopwatch sw;

            #region ADO benchmark
            
            sw = System.Diagnostics.Stopwatch.StartNew();
            using (var sqlConnection = GetOpenSQLConnection())
            {
                var command = $"SELECT * FROM Blogs b WHERE b.Id = {BIG_BLOG_1_ID}";
                var sda = new SqlDataAdapter(command, sqlConnection);
                var dataset = new DataSet();
                sda.Fill(dataset);
                Console.WriteLine($"ADO warmup in {sw.ElapsedMilliseconds}ms");
            }

            sw = System.Diagnostics.Stopwatch.StartNew();
            using (var sqlConnection = GetOpenSQLConnection())
            {
                var command = $"SELECT * FROM Blogs b INNER JOIN Posts p ON p.BlogId = b.Id WHERE b.Id = {BIG_BLOG_1_ID}";
                var sda = new SqlDataAdapter(command, sqlConnection);
                var dataset = new DataSet();
                sda.Fill(dataset);
                Console.WriteLine($"ADO {dataset.Tables[0].Rows.Count} posts read in {sw.ElapsedMilliseconds}ms");
            }

            sw = System.Diagnostics.Stopwatch.StartNew();
            using (var sqlConnection = GetOpenSQLConnection())
            {
                var command = $"SELECT * FROM Blogs b INNER JOIN Posts p ON p.BlogId = b.Id WHERE b.Id = {BIG_BLOG_1_ID} AND p.Title = '{RANDOM_POST_TITLE}'";
                var sda = new SqlDataAdapter(command, sqlConnection);
                var dataset = new DataSet();
                sda.Fill(dataset);
                Console.WriteLine($"ADO {dataset.Tables[0].Rows[0]["Title"]} read in {sw.ElapsedMilliseconds}ms");
            }

            #endregion

            #region EF

            // Warmup

            using (var context = new BenchmarkModelContext())
            {
                sw = System.Diagnostics.Stopwatch.StartNew();
                var warmup = context.Blogs.FirstOrDefault().Name;
                var warmupa = context.Blogs.FirstOrDefaultAsync().Result.Name;
                Console.WriteLine($"EF warmup in {sw.ElapsedMilliseconds}ms");
            }
            
            // Blogs

            using (var context = new BenchmarkModelContext())
            {
                sw = System.Diagnostics.Stopwatch.StartNew();
                var posts = context.Blogs.OrderBy(item => item.Id).ToPageList(1, 10);
                Console.WriteLine($"EF Sync paged {posts.Count} blogs read in {sw.ElapsedMilliseconds}ms");
            }

            using (var context = new BenchmarkModelContext())
            {
                sw = System.Diagnostics.Stopwatch.StartNew();
                var posts = context.Blogs.OrderBy(item => item.Id).ToList();
                Console.WriteLine($"EF Sync all (non-paged) {posts.Count} blogs read in {sw.ElapsedMilliseconds}ms");
            }

            // Posts

            using (var context = new BenchmarkModelContext())
            {
                sw = System.Diagnostics.Stopwatch.StartNew();
                var posts = context.Blogs.Where(item => item.Id == BIG_BLOG_1_ID).Single().Posts;
                Console.WriteLine($"EF Sync {posts.Count} posts read in {sw.ElapsedMilliseconds}ms");
            }
            using (var context = new BenchmarkModelContext(false))
            {
                sw = System.Diagnostics.Stopwatch.StartNew();
                var posts = context.Blogs.Where(item => item.Id == BIG_BLOG_2_ID).Include(item => item.Posts).Single().Posts;
                Console.WriteLine($"EF Sync non-lazy {posts.Count} posts read in {sw.ElapsedMilliseconds}ms");
            }

            using (var context = new BenchmarkModelContext())
            {
                sw = System.Diagnostics.Stopwatch.StartNew();
                var t = context.Blogs.Where(item => item.Id == BIG_BLOG_3_ID).SingleAsync();
                var postsa = t.Result.Posts;

                Console.WriteLine($"EF Async {postsa.Count} posts read in {sw.ElapsedMilliseconds}ms");
            }

            #endregion

            Console.WriteLine(new String('*', 10));

            sw = System.Diagnostics.Stopwatch.StartNew();

            var r1 = ReadDB(1);
            var r3 = ReadDB(1501);
            var r2 = ReadDB(2);
            var r4 = ReadDB(1502);

            Console.WriteLine($"{r1.Posts.Count} / {r3.Posts.Count} / {r2.Posts.Count} / {r4.Posts.Count}");
            Console.WriteLine($"4 sync reads done in {sw.ElapsedMilliseconds}ms");

            sw = System.Diagnostics.Stopwatch.StartNew();

            var t1 = ReadDBAsync(1, true);
            var t3 = ReadDBAsync(1501);
            var t2 = ReadDBAsync(2);
            var t4 = ReadDBAsync(1502);
            
            Console.WriteLine($"[1] {t1.Result.Posts.Count} / [1501] {t3.Result.Posts.Count} / [2] {t2.Result.Posts.Count} / [1502] {t4.Result.Posts.Count}");
            Console.WriteLine($"4 async reads done in {sw.ElapsedMilliseconds}ms");
        }
    }
}
