using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Al_Baraka.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Al_Baraka.ViewModel;

namespace Al_Baraka.Controllers
{
    public class HomeController : Controller
    {
        private ProductContext pc;
        public HomeController(ProductContext prodCont)
        {
            pc = prodCont;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<List<Product>> AllGroups = new List<List<Product>>();

            List<Product> group0 = new List<Product>();
            group0 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.DriedFruits == true select g).ToList();
            AllGroups.Add(group0);

            List<Product> group1 = new List<Product>();
            group1 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.EasternMed == true select g).ToList();
            AllGroups.Add(group1);

            List<Product> group2 = new List<Product>();
            group2 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.Grocery == true select g).ToList();
            AllGroups.Add(group2);

            List<Product> group3 = new List<Product>();
            group3 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.Italian == true select g).ToList();
            AllGroups.Add(group3);

            List<Product> group4 = new List<Product>();
            group4 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.Nuts == true select g).ToList();
            AllGroups.Add(group4);

            List<Product> group5 = new List<Product>();
            group5 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.Oils == true select g).ToList();
            AllGroups.Add(group5);

            List<Product> group6 = new List<Product>();
            group6 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.Sauces == true select g).ToList();
            AllGroups.Add(group6);

            List<Product> group7 = new List<Product>();
            group7 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.Spice == true select g).ToList();
            AllGroups.Add(group7);

            List<Product> group8 = new List<Product>();
            group8 = (from g in pc.Products.Include(g => g.Groups).ToList() where g.Groups.Sweets == true select g).ToList();
            AllGroups.Add(group8);

            return View(AllGroups);
        }
        [HttpPost]
        public IActionResult Index(string searchPattern)
        {
            if(searchPattern == null || searchPattern == "")
            {
                return Index();
            }
            List<Product> result = (from p in pc.Products where p.Name.ToLower().Contains(searchPattern.ToLower()) || p.Id.ToString() == searchPattern select p).Include(p => p.Groups).ToList();
                return View("searchResult", result);
        }
        public IActionResult ShowOneGroup(int idgroup)
        {
            List<Product> result = null;
            switch (idgroup)
            {
                case 1:
                    View(pc.Products.Include(g => g.Groups).ToList());
                    break;
                case 2:
                    result = (from p in pc.Products where p.Groups.Sweets == true select p).Include(g=>g.Groups).ToList();
                    View(result);
                    break;

                case 3:
                    result = (from p in pc.Products where p.Groups.DriedFruits == true select p).Include(g => g.Groups).ToList();
                    View(result);
                    break;

                case 4:
                    result = (from p in pc.Products where p.Groups.Spice == true select p).Include(g => g.Groups).ToList();
                    View(result);
                    break;

                case 5:
                    result = (from p in pc.Products where p.Groups.Nuts == true select p).Include(g => g.Groups).ToList();
                    View(result);
                    break;

                case 6:
                    result = (from p in pc.Products where p.Groups.Oils == true select p).Include(g => g.Groups).ToList();
                    View(result);
                    break;

                case 7:
                    result = (from p in pc.Products where p.Groups.Sauces == true select p).Include(g => g.Groups).ToList();
                    View(result);
                    break;

                case 8:
                    result = (from p in pc.Products where p.Groups.Italian == true select p).Include(g => g.Groups).ToList();
                    View(result);
                    break;

                case 9:
                    result = (from p in pc.Products where p.Groups.EasternMed == true select p).Include(g => g.Groups).ToList();
                    View(result);
                    break;
                case 10:
                    result = (from p in pc.Products where p.Groups.Grocery == true select p).Include(g => g.Groups).ToList();
                    View(result);
                    break;

                default:
                    throw new Exception("Fail of group product...");
            }
            return View(null);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddNew()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddNew(ViewProduct product)
        {
            Product p = new Product();
            byte[] byteArray = null;
            if (product.Image != null)
            {
                using (BinaryReader reader = new BinaryReader(product.Image.OpenReadStream()))
                {
                    byteArray = reader.ReadBytes((int)product.Image.Length);
                }
            }

            p.Groups = new ProductGroups()
            {
                DriedFruits = product.Groups.DriedFruits,
                EasternMed = product.Groups.EasternMed,
                Italian = product.Groups.Italian,
                Nuts = product.Groups.Nuts,
                Oils = product.Groups.Oils,
                Sauces = product.Groups.Sauces,
                Spice = product.Groups.Spice,
                Sweets = product.Groups.Sweets,
                Grocery = product.Groups.Grocery
            };
            p.Country = product.Country;
            p.Description = product.Description;
            p.Name = product.Name;
            p.Price = product.Price;
            p.Image = byteArray;
            p.Measure = product.Measure;

            pc.Products.Add(p);
            //pc.SaveChanges();

            //p.Groups.ProductId = p.Id;
            pc.ProductGroups.Add(p.Groups);
            await pc.SaveChangesAsync();

            return RedirectToAction("listofproducts", "Home");
        }
        [Authorize]
        [HttpGet]
        public IActionResult Tips(string message = "")
        {
            return View("Tips");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Tips(string ClientName, string ClientContact, string Message)
        {
            if (Message != null)
            {
               User curruser = pc.Users.FirstOrDefault((str)=>str.Login == HttpContext.User.Identity.Name);
                pc.Reviews.Add(new Review() {ReviewText = Message, Name = ClientName, Contact  = ClientContact, UserId = curruser.Id});
                await pc.SaveChangesAsync();
                return View("Tips", "Ваше повідомлення було надіслано, дякуємо");
            }
            else
            {
                return View("Tips", "Введіть, будь-ласка, текст повідомлення");
            }
        }
        //public IActionResult Grocery()
        //{
        //    ViewData["Message"] = "Grocery";

        //    return View();
        //}
        public IActionResult Action()
        {
            ViewData["Message"] = "Action";

            return View();
        }
        public IActionResult Services()
        {
            ViewData["Message"] = "Services";

            return View();
        }
        public IActionResult Cooperation()
        {
            ViewData["Message"] = "Cooperation";

            return View();
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Details(int IdProduct)
        {
            ViewData["Message"] = "Details.";
            Product prod = pc.Products.First((p) => p.Id == IdProduct);
            if (prod == null)
                throw new Exception("Product by 'IdProduct' was not found!");
            return View(prod);
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public IActionResult ListOfProducts()
        {
            return View(pc.Products.Include(g=>g.Groups).ToList());
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult ListOfProducts(string searchPattern)
        {
            if (searchPattern == null)
                searchPattern = "";
            List<Product> result = (from p in pc.Products where p.Name.ToLower().Contains(searchPattern.ToLower()) || p.Id.ToString() == searchPattern select p).Include(p=>p.Groups).ToList();
            return View(result);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Edit(int idproduct)
        {
            Product Editingproduct = pc.Products.Include((p) => p.Groups).Where(p => p.Id == idproduct).FirstOrDefault();
            if (Editingproduct != null)
                return View(Editingproduct);
            throw new Exception("Not find product by id");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(ViewProduct EditingProduct)
        {
            Product product = pc.Products.Include(p => p.Groups).SingleOrDefault((s) => s.Id == EditingProduct.Id);

            if (product != null)
            {
                byte[] byteArray = null;
                if (EditingProduct.Image != null)
                {
                    using (BinaryReader reader = new BinaryReader(EditingProduct.Image.OpenReadStream()))
                    {
                        byteArray = reader.ReadBytes((int)EditingProduct.Image.Length);
                    }
                    product.Image = byteArray;
                }
                product.Name = EditingProduct.Name;
                product.Description = EditingProduct.Description;
                product.Country = EditingProduct.Country;
                product.Measure = EditingProduct.Measure;
                product.Price = EditingProduct.Price;
                product.Groups.DriedFruits = EditingProduct.Groups.DriedFruits;
                product.Groups.EasternMed = EditingProduct.Groups.EasternMed;
                product.Groups.Grocery = EditingProduct.Groups.Grocery;
                product.Groups.Italian = EditingProduct.Groups.Italian;
                product.Groups.Nuts = EditingProduct.Groups.Nuts;
                product.Groups.Oils = EditingProduct.Groups.Oils;
                product.Groups.Sauces = EditingProduct.Groups.Sauces;
                product.Groups.Spice = EditingProduct.Groups.Spice;
                product.Groups.Sweets = EditingProduct.Groups.Sweets;

                TryUpdateModelAsync<Product>(product);
                pc.SaveChanges();
            }
            return RedirectToAction("listofproducts", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int idproduct)
        {
            return View(idproduct);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Delete(int idproduct, string name)
        {
            Product product = pc.Products.Include(p => p.Groups).FirstOrDefault((p) => p.Id == idproduct);
            if (product != null)
            {
                pc.Products.Remove(product);
                pc.SaveChanges();
                return RedirectToAction("listofproducts", "Home");
            }
            throw new Exception("Not find product by id");
        }
        public IActionResult Error()
        {
            return View();
        }
        [Authorize(Roles ="user")]
        public async Task<IActionResult> Order()
        {
            User currUser = await pc.Users.FirstOrDefaultAsync((u) => u.Login == User.Identity.Name);
            int userId;
            if (currUser != null)
            {
                userId = currUser.Id;
            }
            else
                throw new Exception("didn't find current user");
            return View(pc.Orders.Include((o)=>o.ProductItem).Where((o)=>o.UserId==userId && o.IsDone==false).ToList());
        }
        
        [Authorize(Roles= "admin")]
        public IActionResult Reviews()
        {
            List<Review> Reviews = (from r in pc.Reviews orderby r.Id descending select r).ToList();
            return View(Reviews);
        }
        [Authorize]
        public async Task<IActionResult> OrderDelete(int idOrder)
        {
            Order delOrder = pc.Orders.FirstOrDefault((o) => o.Id == idOrder);
            if (delOrder != null)
            {
                pc.Orders.Remove(delOrder);
                await pc.SaveChangesAsync();
            }
            else
            {
                throw new Exception("didn't find Order for delete");
            }
            if (User.IsInRole("user"))
            {
                User currUser = await pc.Users.FirstOrDefaultAsync((u) => u.Login == User.Identity.Name);
                return View("Order", pc.Orders.Include((o) => o.ProductItem).Where((o) => o.UserId == currUser.Id && o.IsDone == false).ToList());
            }
            else
            {
                IEnumerable<IGrouping<int, Order>> OrdersList = from o in (await pc.Orders.Include(o => o.ProductItem).Where((o) => o.IsDone == true).ToListAsync()) orderby o.TimeOfBye group o by o.UserId;
                return View("OrdersList", OrdersList);
            }
        }
        [Authorize(Roles ="user")]
        [HttpGet]
        public async Task<IActionResult> MakeOrder(int idproduct)
        {

            Product currProd = await pc.Products.FirstOrDefaultAsync((p) => p.Id == idproduct);
            User currUser = await pc.Users.FirstOrDefaultAsync((u) => u.Login == User.Identity.Name);
            if(currProd != null && currUser != null)//prof user and product
            {
                Order ExsistingOrder = await pc.Orders.FirstOrDefaultAsync((o) => o.ProductId == idproduct && o.UserId == currUser.Id && o.IsDone == false);
                if (ExsistingOrder != null)
                {//you cant make twice, if you want make two position of this product than must go to the 'Мої товари' page.
                    return View("MakeOrder", "Ви вже додавали цю позицію до корзини, для того щоб збільшити кількість одиниць данного товару перейдить до списку ваших товарів");//if change here must also to change in MakeOrder.cshtml!!!!
                }
                else//current user didn't add current product to his order
                {
                    //price must be toggled in this moment, because price will be change.
                    //Quantity is 1, than it can be changed.
                    Order NewOrder = new Order() { IsDone = false, Price = currProd.Price, TimeOfBye = DateTime.Now, UserId = currUser.Id, ProductItem = currProd, ProductId = idproduct, Quantity = 1 };
                    pc.Orders.Add(NewOrder);
                    await pc.SaveChangesAsync();
                    
                }
            }
            else
            {
                throw new Exception("don't find Product or User by id");
            }
            if (pc.Orders.Where((o)=>o.UserId == currUser.Id && o.IsDone == false).Any((o)=>o.Contact != null))//if user didn't give his contact, than ask
            {
               return View("MakeOrder", "Товар був доданий до корзини, якщо ви бажаете змінити ваші контактні дані або кількість одиниць товару перейдіть до розділу 'Мої товари'.");
            }
            else
            {
                 return View("MakeOrder", "Товар був доданий до корзини, будь-ласка, надайте нам ваші контактні дані. Після цього ви можете відправити нам ваше замовлення з розілку 'Мої товари'.");
            }
            
        }
        [Authorize(Roles = "user")]
        [HttpPost]
        public async Task<IActionResult> MakeOrder(string Contacts)
        {
            if (Contacts == null)//if clien didn't write some contact
                return View("MakeOrder","Товар був доданий до корзини, будь-ласка, надайте нам ваші контактні дані. Після цього ви можете відправити нам ваше замовлення з розілку 'Мої товари'.");//if change here than must to change in MakeOrder.cshtml
            User currUser = await pc.Users.FirstOrDefaultAsync((u) => u.Login == User.Identity.Name);
            var orders = pc.Orders.Where((o) => o.UserId == currUser.Id && o.IsDone == false);
            if(orders != null)
            {
                foreach (var ord in orders)
                {
                    ord.Contact = Contacts;
                }
                await pc.SaveChangesAsync();
            }
            else
            {
                throw new Exception("No one product exists");
            }
            return View("Order", pc.Orders.Include((o) => o.ProductItem).Where((o) => o.UserId == currUser.Id && o.IsDone == false).ToList());
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> OrderChangeQuantityAsync(int quantity, int idproduct, int iduser)
        {
            Order currOrder = await pc.Orders.FirstOrDefaultAsync((o) => o.ProductId == idproduct && iduser == o.UserId);
            currOrder.Quantity = quantity;
            await pc.SaveChangesAsync();
            if (User.IsInRole("user"))
                return View("Order", pc.Orders.Include((o) => o.ProductItem).Where((o) => o.UserId == iduser && o.IsDone == false).ToList());
            else
            {
                //take orders that are NO IsDone(not closed orders) and group by client id and sort by order time(from newest)
                IEnumerable<IGrouping<int, Order>> OrdersList = from o in (await pc.Orders.Include(o => o.ProductItem).Where((o) => o.IsDone == true).ToListAsync()) orderby o.TimeOfBye group o by o.UserId;
                return View("OrdersList", OrdersList);
            }
                
        }
        [Authorize(Roles ="user")]
        [HttpGet]
        public async Task<IActionResult> OrderChangeContacts(string contacts, int idproduct, int iduser)
        {
            if(contacts==null)
                return View("Order", pc.Orders.Include((o) => o.ProductItem).Where((o) => o.UserId == iduser && o.IsDone == false).ToList());

            List<Order> ordersList = await pc.Orders.Where((o) => iduser == o.UserId && o.IsDone == false).ToListAsync();
            foreach (var item in ordersList)
            {
                item.Contact = contacts;
            }
            await pc.SaveChangesAsync();

            return View("Order", pc.Orders.Include((o) => o.ProductItem).Where((o) => o.UserId == iduser && o.IsDone == false).ToList());
        }
        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<IActionResult> OrdersList()
        {
            //take orders that are NO IsDone(not closed orders) and group by client id and sort by order time(from newest)
            IEnumerable<IGrouping<int, Order>> OrdersList = from o in (await pc.Orders.Include(o => o.ProductItem).Where((o) => o.IsDone == true).ToListAsync()) orderby o.TimeOfBye group o by o.UserId;
            return View(OrdersList);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> CloseAndDeleteOrder(int idClient)
        {
            var OrdersForDelete = pc.Orders.Where((o) => o.UserId == idClient && o.IsDone == true);
            if(OrdersForDelete!=null)
            {
                pc.Orders.RemoveRange(OrdersForDelete);
                await pc.SaveChangesAsync();
            }
            //take orders that are NO IsDone(not closed orders) and group by client id and sort by order time(from newest)
            IEnumerable<IGrouping<int, Order>> OrdersList = from o in (await pc.Orders.Include(o => o.ProductItem).Where((o) => o.IsDone == true).ToListAsync()) orderby o.TimeOfBye group o by o.UserId;
            return View("OrdersList", OrdersList);
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> OrderNote(string note, int iduser)
        {
            Order order = await pc.Orders.Where((o) => o.UserId == iduser && o.IsDone == true).FirstOrDefaultAsync();
            if(order!= null)
            {
                order.Note = note;
                await pc.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Did't find order for add note");
            }
            IEnumerable<IGrouping<int, Order>> OrdersList = from o in (await pc.Orders.Include(o => o.ProductItem).Where((o) => o.IsDone == true).ToListAsync()) orderby o.TimeOfBye group o by o.UserId;
            return View("OrdersList", OrdersList);
        }

        [Authorize(Roles = "user")]
        public async Task<IActionResult> AcceptOrderAsync(int iduser)
        {
            List<Order> OrdersList = await pc.Orders.Where((o) => o.UserId == iduser && o.IsDone == false).ToListAsync();
            foreach (Order odr in OrdersList)
            {
                Order tempOrder = pc.Orders.Where((o) => o.Id == odr.Id).FirstOrDefault();
                if(tempOrder!=null)
                    tempOrder.IsDone = true;
                await pc.SaveChangesAsync();
            }
            User currUser = await pc.Users.FirstOrDefaultAsync((u) => u.Login == User.Identity.Name);
            if (currUser != null)
            {
                return View("Order",pc.Orders.Include((o) => o.ProductItem).Where((o) => o.UserId == currUser.Id && o.IsDone == false).ToList());
            }
            else
            {
                throw new Exception("Didn't find user");
            }
        }
    }
}
