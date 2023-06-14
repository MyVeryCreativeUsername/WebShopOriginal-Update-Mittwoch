using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebShop.Data;
using WebShop.DTOs.UserDTOs;
using WebShop.Models.UserEntities;
using WebShop.Utilities;

namespace WebShopTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly ShopDb _dbHandle;
        public AccountController(ShopDb dbHandle) { _dbHandle = dbHandle; }





        [HttpPost]
        [Route("RegisterUser")]
        public ActionResult RegisterUser(RegistrationDTO registrationDTO)
        {
            User user = new User();

            var existEmail = _dbHandle.Users.FirstOrDefault(x => x.Email == registrationDTO.Email);

            if (existEmail != null)
            {
                return Unauthorized("Email already registered!");
            }
            else if (registrationDTO.Password != registrationDTO.ConfirmPassword)
            {
                return Unauthorized("Passwords are not matching!");
            }
            else
            {
                user.Email = registrationDTO.Email;
                user.FirstName = registrationDTO.FirstName;
                user.LastName = registrationDTO.LastName;

                (user.PasswordSalt, user.PasswordHash) = new UserControllerHelper().SaltHashCreator(registrationDTO.Password);

                var existUserAddress = _dbHandle.Addresses.FirstOrDefault(x =>
                    x.Country == registrationDTO.Address.Country &&
                    x.Region == registrationDTO.Address.Region &&
                    x.City == registrationDTO.Address.City &&
                    x.StreetAddress == registrationDTO.Address.StreetAddress);

                if (existUserAddress != null)
                {
                    user.Address = existUserAddress;
                }
                else
                {
                    user.Address = registrationDTO.Address;
                }
                _dbHandle.Users.Add(user);
                _dbHandle.SaveChanges();

                return Ok("Registration Successfull!");
            }
        }




        [HttpPost]
        [Route("LoginUser")]
        public ActionResult LoginUser(LoginDTO loginDTO)
        {
            var existEmail = _dbHandle.Users.FirstOrDefault(x => x.Email == loginDTO.Email);

            if (existEmail == null)
            {
                return Unauthorized("Email or Passwords are not matching!");
            }
            else
            {
                (string loginPwHashString, string existEmailPwHashstring) = new UserControllerHelper().LoginByteToString(loginDTO, existEmail);

                if (existEmailPwHashstring != loginPwHashString)
                {
                    return Unauthorized("Email or Passwords are not matching!");
                }
                else
                {
                    return Ok("Login Successfull!");
                }
            }
        }





        [HttpPost]
        [Route("DeleteUser")]
        public ActionResult DeleteUser(LoginDTO loginDTO)
        {

            var existEmail = _dbHandle.Users.FirstOrDefault(x => x.Email == loginDTO.Email);

            if (existEmail == null)
            {
                return Unauthorized("Email or Passwords are not matching!");
            }
            else
            {
                (string loginPwHashString, string existEmailPwHashstring) = new UserControllerHelper().LoginByteToString(loginDTO, existEmail);

                if (existEmailPwHashstring != loginPwHashString)
                {
                    return Unauthorized("Email or Passwords are not matching!");
                }
                else
                {
                    _dbHandle.Users.Remove(existEmail);
                    _dbHandle.SaveChanges();
                    return Ok("User deletion Successfull!");
                }
            }
        }




        [HttpPost]
        [Route("ChangeUserPassword")]
        public ActionResult ChangeUserPassword(LoginDTO loginDTO, string password)
        {

            var existEmail = _dbHandle.Users.FirstOrDefault(x => x.Email == loginDTO.Email);

            if (existEmail == null)
            {
                return Unauthorized("Email or Passwords are not matching!");
            }
            else
            {
                (string loginPwHashString, string existEmailPwHashstring) = new UserControllerHelper().LoginByteToString(loginDTO, existEmail);

                if (existEmailPwHashstring != loginPwHashString)
                {
                    return Unauthorized("Email or Passwords are not matching!");
                }
                else
                {
                    (existEmail.PasswordSalt, existEmail.PasswordHash) = new UserControllerHelper().SaltHashCreator(loginDTO.Password);
                    _dbHandle.SaveChanges();
                    return Ok("Password change Successfull!");
                }
            }
        }





    }
}
