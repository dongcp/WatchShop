using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WatchShop.Areas.Client.Models;
using WatchShop.Common;

namespace WatchShop.Areas.Client.Controllers
{
    public class CartController : Controller
    {
        // GET: Client/Cart
        public ActionResult Index()
        {
            List<CartItem> cart = (List<CartItem>)Session[Constants.SESSION_CART];
            if (cart == null || cart.Count == 0) return RedirectToAction("EmptyCart");
            return View(cart);
        }

        public ActionResult AddItem(string productId, int quantity)
        {
            CartItem cartItem = new CartItem();
            cartItem.Product = ProductDAO.Instance.GetProductById(productId);
            cartItem.Quantity = quantity;

            List<CartItem> cart = (List<CartItem>)Session[Constants.SESSION_CART];

            if (cart == null || cart.Count == 0)
            {
                cart = new List<CartItem>();
                cart.Add(cartItem);
            }
            else
            {
                int index = GetCartItemIndex(cart, productId);
                if (index == -1)
                    cart.Add(cartItem);
                else
                    cart[index].Quantity += quantity;

            }

            Session[Constants.SESSION_CART] = cart;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            List<CartItem> cart = (List<CartItem>)Session[Constants.SESSION_CART];
            if (cart != null)
            {
                foreach (CartItem item in cart)
                {
                    var jsonItem = jsonCart.SingleOrDefault(x => x.Product.Id.Equals(item.Product.Id));
                    if (jsonItem != null)
                        item.Quantity = jsonItem.Quantity;
                }
                Session[Constants.SESSION_CART] = cart;
            }

            return Json(new
            {
                status = true
            });
        }

        public ActionResult RemoveItem(string productId)
        {
            List<CartItem> cart = (List<CartItem>)Session[Constants.SESSION_CART];
            if (cart != null)
            {
                int index = GetCartItemIndex(cart, productId);
                if (index != -1)
                {
                    cart.RemoveAt(index);
                }
                Session[Constants.SESSION_CART] = cart;
            }
            return RedirectToAction("Index");
        }

        public JsonResult DeleteAll()
        {
            Session[Constants.SESSION_CART] = null;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult EmptyCart()
        {
            return View();
        }

        private int GetCartItemIndex(List<CartItem> cart, string productId)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(productId))
                    return i;
            }
            return -1;
        }
    }
}