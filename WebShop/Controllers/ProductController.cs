using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebShop.Data;
using WebShop.DTOs.ShopDTOs;
using WebShop.Models.ShopEntities;

namespace WebShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ShopDb _dbHandle;
        public ProductController(ShopDb dbHandle) { _dbHandle = dbHandle; }




        [HttpPost]
        [Route("AddReviewToProduct")]
        public ActionResult AddReviewToProduct(ReviewDTO reviewDto)
        {

            var review = new Review();

            review.UserId = reviewDto.UserId;
            review.ProductId = reviewDto.ProductId;
            review.Comment = reviewDto.Comment;
            review.Rating = reviewDto.Rating;


            _dbHandle.Reviews.Add(review);
            _dbHandle.SaveChanges();

            return Ok(_dbHandle.Reviews);
        }



        [HttpPost]
        [Route("ShowReviewOfProduct")]
        public ActionResult ShowReviewOfProduct(int id)
        {
            var reviewedProduct = _dbHandle.Products
                .Include(x => x.ProductReviews)
                .Where(x => x.Id == id).ToList();

            return Ok(reviewedProduct);
        }




        [HttpPost]
        [Route("DeleteReviewFromProduct")]
        public ActionResult DeleteReviewFromProduct(RemoveReviewDTO RemoveReviewDTO)
        {
            var deleteReview = _dbHandle.Reviews.FirstOrDefault(x => x.Id == RemoveReviewDTO.ReviewId && x.ProductId == RemoveReviewDTO.ProductId);

            _dbHandle.Reviews.Remove(deleteReview);

            _dbHandle.SaveChanges();

            return Ok(_dbHandle.Reviews);
        }



        [HttpPost]
        [Route("AddDiscountToProduct")]
        public ActionResult AddDiscountToProduct(DiscountDTO discountDto)
        {
            var discount = new Discount();

            discount.ProductId = discountDto.ProductId;
            discount.Description = discountDto.Description;
            discount.NewPrice = discountDto.NewPrice;

            _dbHandle.Add(discount);
            _dbHandle.SaveChanges();

            return Ok(_dbHandle.Discounts);
        }



        [HttpPost]
        [Route("ShowDiscountOfProduct")]
        public ActionResult ShowDiscountOfProduct(int id)
        {
            var discountedProduct = _dbHandle.Products
                .Include(x => x.ProductDiscount)
                .Where(x => x.Id == id).ToList();


            return Ok(discountedProduct);
        }




        [HttpPost]
        [Route("RemoveDiscountFromProduct")]
        public ActionResult RemoveDiscountFromProduct(int id)
        {
            var itemToRemove = _dbHandle.Discounts.SingleOrDefault(x => x.ProductId == id); //returns a single item.

            if (itemToRemove != null)
            {
                _dbHandle.Discounts.Remove(itemToRemove);
                _dbHandle.SaveChanges();
            }

            return Ok("Discound Removed!");
        }







    }
}
