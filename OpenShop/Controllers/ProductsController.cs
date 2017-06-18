using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OpenShop.Models;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace OpenShop.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.ProductQuantities);
            return View(product.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Details/5
        [HttpPost]
        public ActionResult Details(int? id, int quantity)
        {
            Product product = new Product();
            product = db.Product.Find(id);
            if (ModelState.IsValid)
            {
                
                Debug.Print(product.Id.ToString());
                Debug.Print(product.product_name);
                Debug.Print(quantity.ToString());
                Debug.Print((product.price * quantity).ToString());
                Orders order = new Orders();
                order.ProductId = product.Id;
                order.Product = product;
                order.Quantity = quantity;
                order.UserId = User.Identity.GetUserId();
                order.Price = product.price * quantity;
                db.Order.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.ProductQuantities, "ProductId", "ProductId", product.Id);
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.ProductQuantities, "ProductId", "ProductId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Product product)
        {
            if (ModelState.IsValid)
            {
                ProductQuantities prodQty = new ProductQuantities();
                prodQty.Product = product;
                prodQty.Quantity = product.ProductQuantities.Quantity;               
                db.Product.Add(product);
                db.SaveChanges();
                db.ProductQuantities.Add(prodQty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.ProductQuantities, "ProductId", "ProductId", product.Id);
            return View(product);
        }


        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                int prodId = product.Id;
                ProductQuantities prodQty = db.ProductQuantities.Find(prodId);
                Product prod = db.Product.Find(prodId);
                prodQty.Quantity = product.ProductQuantities.Quantity;
                prod.product_name = product.product_name;
                prod.price = product.price;
                db.SaveChanges();

               
                db.SaveChanges();
                return RedirectToAction("Index");
            }           
            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}
