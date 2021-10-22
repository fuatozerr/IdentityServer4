using IdentityServer.API1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [Authorize(Policy = "ReadProduct")]
        public IActionResult GetProducts()
        {
            var productList = new List<Product>()
            {
                new Product {Id=1,Name="kalem",Price=100,Stock=500},
                new Product {Id=2,Name="silgi",Price=100,Stock=500},
                new Product {Id=3,Name="defter",Price=100,Stock=500},
                new Product {Id=4,Name="kalem",Price=100,Stock=500},
                new Product {Id=51,Name="kalem",Price=100,Stock=500},
                new Product {Id=61,Name="kalem",Price=100,Stock=500},

            };

            return Ok(productList);
        }

        [Authorize(Policy = "UpdateOrCreate")]
        public IActionResult UpdateProduct(int id)
        {
            return Ok($"Idsi {id} olan ürün güncellendi ");
        }
    }
}
