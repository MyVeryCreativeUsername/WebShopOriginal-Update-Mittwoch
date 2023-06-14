using Bogus;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebShop.Data;
using WebShop.Models.ShopEntities;
using WebShop.Models.UserEntities;

namespace WebShopTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedingController : ControllerBase
    {

        private readonly ShopDb _dbHandle;
        public SeedingController(ShopDb dbHandle) { _dbHandle = dbHandle; }



        [HttpPost]
        [Route("SeedCategorys")]
        public ActionResult SeedCategorys()
        {
            Random random = new Random();

            var category = new Faker<Category>().RuleFor(s => s.CategoryName, f => f.Commerce.Department());

            var categories = category.Generate(10);

            _dbHandle.Categories.AddRange(categories);

            _dbHandle.SaveChanges();

            return Ok(_dbHandle.Categories);
        }



        // SeedCategory needs to be executed first 
        [HttpPost]
        [Route("SeedProducts")]
        public ActionResult SeedProducts()
        {
            var categories = _dbHandle.Categories.ToList();

            Random random = new Random();

            var product = new Faker<Product>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductName())
                .RuleFor(s => s.Price, f => f.Random.Float(1, 200))
                .RuleFor(s => s.Categorys, f => categories.OrderBy(x => random.Next()).Take(random.Next(1, 6)).ToList())
                .RuleFor(s => s.Qty, f => f.Random.Int(1, 500));


            var products = product.Generate(300);

            _dbHandle.Products.AddRange(products);

            _dbHandle.SaveChanges();

            return Ok(_dbHandle.Products);
        }



        [HttpPost]
        [Route("SeedUsers")]
        public ActionResult SeedUsers()
        {
            var FakeAddress = new Faker<WebShop.Models.UserEntities.Address>()
                .RuleFor(s => s.Country, f => f.Address.Country())
                .RuleFor(s => s.Region, f => f.Address.County())
                .RuleFor(s => s.City, f => f.Address.City());

            var adresses = FakeAddress.Generate(30);

            Random random = new Random();

            var newUser = new Faker<User>()
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.Email, (f, x) => f.Internet.Email(x.FirstName, x.LastName))
                .RuleFor(s => s.Address, f => adresses[random.Next(adresses.Count)]);

            var users = newUser.Generate(100);

            _dbHandle.AddRange(users);

            _dbHandle.SaveChanges();

            return Ok(_dbHandle.Users);
        }




    }
}
