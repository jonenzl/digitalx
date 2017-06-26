using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DigitalX.Models;
using System.Collections;

namespace DigitalX.Controllers
{
    public class ProductController : Controller
    {
        ServiceReference1.Service1Client service = new ServiceReference1.Service1Client();

        // GET: Product
        public ActionResult Index()
        {
            return View(service.GetAllProducts());

            //return View(service.GetProducts(10, 1));
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View(service.ProductDetail(id));
        }

        // GET: Orders for the currently logged in customer
        public ActionResult Orders()
        {
            return View(service.Orders(1).ToList());
        }

        // GET: Shopping cart of current customer
        public ActionResult ShoppingCart()
        {
            List<CartModel> products = (List<CartModel>)Session["cart"];

            // If the shopping cart has no items, return to the home page
            if (products == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View(products);
        }

        // Add an item to the shopping cart
        public ActionResult AddToCart(int id)
        {
            // Get the ProductId of the item to add to the cart
            var product = service.ProductDetail(id);

            if (Session["cart"] == null)
            {
                // Add the product to the shopping cart
                List<CartModel> cart = new List<CartModel>();
                cart.Add(new CartModel() { product = product, quantity = 1 });
                Session["cart"] = cart;

                // Update the cart badge value
                ViewBag.cart = cart.Count();
                Session["count"] = 1;
            }
            else
            {
                List<CartModel> cart = (List<CartModel>)Session["cart"];

                // Check to see whether the specific product already exists in the shopping cart
                if (cart.Find(x => x.product.ProductID == product.ProductID) != null)
                {
                    // Add one more to the quantity for that product
                    var obj = cart.FirstOrDefault(x => x.product.ProductID == product.ProductID);
                    obj.quantity += 1;
                }
                else
                {
                    // Add the new product to the shopping cart
                    cart.Add(new CartModel() { product = product, quantity = 1 });
                    Session["cart"] = cart;

                    // Update the cart badge value
                    ViewBag.cart = cart.Count();
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }  
            }

            // Return to the current page
            string url = this.Request.UrlReferrer.AbsolutePath;
            return Redirect(url);
        }

        // Increase the quantity of the product in the shopping cart
        public ActionResult AddQuantity(int id)
        {
            // Get the ProductId of the item in the cart
            var product = service.ProductDetail(id);

            // Update the item's quantity
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            var obj = cart.FirstOrDefault(x => x.product.ProductID == product.ProductID);
            obj.quantity += 1;

            return RedirectToAction("ShoppingCart", "Product");
        }

        // Decrease the quantity of the product in the shopping cart
        public ActionResult RemoveQuantity(int id)
        {
            // Get the ProductId of the item in the cart
            var product = service.ProductDetail(id);

            // Get the cart item
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            var obj = cart.FirstOrDefault(x => x.product.ProductID == product.ProductID);

            // If the quantity of the product is 1, delete the product from the cart
            if (obj.quantity == 1)
            {
                // Remove the product from the cart
                cart.RemoveAll(item => item.product.ProductID == product.ProductID);
                Session["cart"] = cart;

                // Update the cart badge value
                Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            }
            else
            {
                obj.quantity -= 1;
            }

            return RedirectToAction("ShoppingCart", "Product");
        }

        // Remove an item from the shopping cart
        public ActionResult DeleteFromCart(int id)
        {
            // Get the ProductId of the item to delete from the cart
            var product = service.ProductDetail(id);

            // Remove the product from the cart
            List<CartModel> cart = (List<CartModel>)Session["cart"];
            cart.RemoveAll(item => item.product.ProductID == product.ProductID);
            Session["cart"] = cart;

            // Update the cart badge value
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;

            return RedirectToAction("ShoppingCart", "Product");
        }

        // Proceed to the checkout page
        public ActionResult Checkout()
        {
            // If the user is not currently logged in, make them log in or register before they can proceed to checkout
            bool loggedIn = User.Identity.IsAuthenticated;
            if (!loggedIn)
            {
                return RedirectToAction("Login", "Account");
            }

            List<CartModel> products = (List<CartModel>)Session["cart"];

            // If the shopping cart has no items, return to the home page
            if (products == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Calculate order subtotal
            decimal subtotal = products.Sum(item => item.product.Price * item.quantity);
            string subtotalMoney = subtotal.ToString("C");
            Session["subtotal"] = subtotalMoney;

            // Calculate order total
            decimal total = decimal.Add(subtotal, 9.99M);
            string totalMoney = total.ToString("C");
            Session["total"] = totalMoney;

            // Retrieve customer's name
            var customer = service.CurrentCustomer(User.Identity.Name);
            Session["firstname"] = customer.FirstName;
            Session["lastname"] = customer.LastName;

            // Retrieve customer's address, if it exists
            var address = service.GetAddress(customer.UserName);
            if (address != null)
            {
                Session["street"] = address.Street;
                Session["suburb"] = address.Suburb;
                Session["city"] = address.City;
                Session["postalcode"] = address.PostalCode;
                Session["country"] = address.Country;
            }
            else
            {
                Session["street"] = "";
                Session["suburb"] = "";
                Session["city"] = "";
                Session["postalcode"] = "";
                Session["country"] = "";
            }

            return View(products);
        }

        // Allow the customer to add or edit their address details
        public ActionResult Address()
        {
            return View();
        }

        // Return to the product listing page
        public ActionResult ReturnToShopping()
        {
            return RedirectToAction("Index", "Product");
        }
    }
}