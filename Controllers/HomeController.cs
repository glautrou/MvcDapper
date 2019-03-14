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

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var connString = @"Server=127.0.0.1,1433;Database=Dapper;User Id=sa;Password=yourStrong(!)Password";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                var contacts = connection.Query<Inventory>("select * from Inventory");
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
