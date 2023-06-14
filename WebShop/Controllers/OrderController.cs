using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebShop.Data;
using WebShop.Models.UserEntities;
using WebShopTest.DTOs.OrderDTOs;

namespace WebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly ShopDb _dbHandle;
        public OrderController(ShopDb dbHandle) { _dbHandle = dbHandle; }



        [HttpPost]
        [Route("GetOrdersOfUser")]
        public ActionResult GetOrdersOfUser(int userId)
        {
            var UserOrders = _dbHandle.Orders.Where(o => o.UserId == userId);
            return Ok(UserOrders);
        }




        [HttpPost]
        [Route("AddOrderToUser")]
        public ActionResult AddOrderToUser(int userId)
        {
            Order order = new Order();

            order.UserId = userId;

            _dbHandle.Orders.Add(order);

            _dbHandle.SaveChanges();

            return Ok(_dbHandle.Orders);
        }





        [HttpPost]
        [Route("RemoveOrderFromUser")]
        public ActionResult RemoveOrderFromUser(int userId, int orderId)
        {
            var order = _dbHandle.Orders.FirstOrDefault(o => o.UserId == userId && o.Id == orderId);

            _dbHandle.Orders.Remove(order);

            _dbHandle.SaveChanges();

            return Ok(_dbHandle.Orders);
        }





        [HttpPost]
        [Route("AddProductToOrder")]
        public ActionResult AddProductToOrder(AddProductToOrderDTO dto)
        {

            var order = _dbHandle.Orders.FirstOrDefault(o => o.Id == dto.OrderId);
            
            var product = _dbHandle.Products.FirstOrDefault(p => p.Id == dto.ProductId);
            
            if (product.Qty < dto.Quantity)
            {
                return Unauthorized("Not enough product in stock");
            }


            product.Qty -= dto.Quantity;
            
            order.Products.Add(product);
            
            _dbHandle.SaveChanges();

            var productsOrdersMapping = _dbHandle.ProductsOrdersMapping.FirstOrDefault(o => o.OrderId == dto.OrderId && o.ProductId == dto.ProductId);
            
            productsOrdersMapping.Qty = dto.Quantity;
            
            _dbHandle.SaveChanges();


            return Ok(order);

        }






        [HttpPost]
        [Route("RemoveProductFromOrder")]
        public ActionResult RemoveProductFromOrder(int orderId, int productId)
        {


            return Ok();

        }






    }
}

