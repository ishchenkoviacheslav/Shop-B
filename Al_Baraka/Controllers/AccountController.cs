using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Al_Baraka.Models;
using Al_Baraka.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Al_Baraka.Controllers
{
    public class AccountController : Controller
    {
        private ProductContext db;
        public AccountController(ProductContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid && model.Login.Length>=6 && model.Password.Length>=6)
            {
                User user = db.Users.Include((u) => u.Role).FirstOrDefault(u => u.Login == model.Login && model.Password == EncryptionHelper.Decrypt(model.Password, u.Password));
                if (user != null)
                {
                    await Authenticate(model.Login); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            bool status = false;
            string token = HttpContext.Request.Form["g-recaptcha-response"];
            using (var webClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("secret", "6LdspTQUAAAAAFRHofxs1W21d3ZDvmFIpB7yZsSQ"),
                        new KeyValuePair<string, string>("response", token)
                    });
                HttpResponseMessage googleResponse = await webClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                string json = await googleResponse.Content.ReadAsStringAsync();
                //var reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(json);
                var obj = JObject.Parse(json);
                status = (bool)obj.SelectToken("success");
                
            }

            if (ModelState.IsValid)
            {
                //captcha + pass & login must be longer than 6 symbol
                if (model.Login.Length < 6 || model.Password.Length < 6 || status == false)
                {
                    return View(model);
                }
                User user = db.Users.FirstOrDefault(u => u.Login == model.Login);
                if (user == null)
                {
                    Role role = db.Roles.Where((r) => r.Name == "user").FirstOrDefault();
                    // добавляем пользователя в бд
                    db.Users.Add(new User { Login = model.Login, Password = EncryptionHelper.Encrypt(model.Password), Role = role });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Login); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                //else
                //    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            string role = userName == "albaraka" ? "admin" : "user";
            // создаем один claim
            var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                    };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public async Task<ActionResult> FormSubmitAsync()
        {
            //string token = HttpContext.Request.Form["g-recaptcha-response"];
            //using (var webClient = new HttpClient())
            //{
            //    var content = new FormUrlEncodedContent(new[]
            //    {
            //            new KeyValuePair<string, string>("secret", "6LdspTQUAAAAAFRHofxs1W21d3ZDvmFIpB7yZsSQ"),
            //            new KeyValuePair<string, string>("response", token)
            //        });
            //    HttpResponseMessage googleResponse = await webClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            //    string json = await googleResponse.Content.ReadAsStringAsync();
            //    var reCaptchaResponse = JsonConvert.DeserializeObject<ReCaptchaResponse>(json);
            //    if (reCaptchaResponse == null)
            //    {
            //        throw new Exception("Unable To Read Response From Server");
            //    }
            //    else if (!reCaptchaResponse.success)
            //    {
            //        throw new Exception("Invalid reCaptcha");
            //    }
            //}

            //var response = Request[];
            //string secretKey = ;
            //var client = new HttpClient();
            //var result = client.PostAsync(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            //var obj = JObject.Parse(result);
            //var status = (bool)obj.SelectToken("success");
            //ViewBag.Message = status ? "Google reCaptcha validation success" : "Google reCaptcha validation failed";

            return View("Index");
        }
    }
}
