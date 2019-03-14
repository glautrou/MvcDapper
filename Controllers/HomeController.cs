using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcDapper.Models;
using Dapper;
using System.Data.SqlClient;

namespace MvcDapper.Controllers
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Owner { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var connString = @"Server=127.0.0.1,1433;Database=Dapper;User Id=sa;Password=yourStrong(!)Password";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                //Simple select
                var inventories = connection.Query<Inventory>("select * from Inventory");

                // connection.Execute("CREATE TABLE [User] (Id int NOT NULL IDENTITY PRIMARY KEY, Name NVARCHAR(80))");

                // //1 insert (pas besoin de INTO)
                // var count = connection.Execute(@"insert [User](Name) values (@name)",
                //     new { name = "Gilles 1" }
                // );

                // //1 insert + return inserted data
                // var user = connection.Query<User>(@"insert into [User](Name) OUTPUT INSERTED.* values (@name)",
                //     new { name = "Gilles 2" }
                // );

                // //n inserts
                // var count2 = connection.Execute(@"insert into [User](Name) values (@name)",
                //     new[] { new { name = "Gilles 3" }, new { name = "Gilles 4" } }
                // );

                var users1 = connection.Query<User>("select * from [User] where Name like CONCAT(@name,'%')",
                    new { name = "Gilles" });

                var users2 = connection.Query<User>("select * from [User] where Name = @name",
                    new { name = "Gilles 1" });

                var users3 = connection.Query<User>("select * from [User]");

                var sql = @"
                    select *
                    from [User] where Name like CONCAT(@search,'%');
                    select * from [User] where Name = @name
                    select * from [User]";

                using (var multi = connection.QueryMultiple(sql, new { search = "Gilles", Name = "Gilles 1" }))
                {
                    var res1 = multi.Read<User>().ToList();
                    var res2 = multi.Read<User>().Single();
                    var res3 = multi.Read<User>().ToList();
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
