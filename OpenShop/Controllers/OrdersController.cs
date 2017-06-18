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
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            // var order = db.Order.Include(o => o.Product);
            string userId = User.Identity.GetUserId();
            var order = db.Order.Where(o => o.UserId == userId);
            return View(order.ToList());
        }

        // POST: Orders
        [HttpPost]
        public ActionResult Index(string id)
        {
            string userId = User.Identity.GetUserId();
            int successes = 0;
           IList<Orders> order = db.Order.Where(o => o.UserId == userId).ToList();
            

            foreach (var curOrder in order)
            {
                int prodId = curOrder.ProductId;
                int availableQty = db.ProductQuantities.Where(q => q.ProductId == prodId).Select(q => q.Quantity).FirstOrDefault();
                ProductQuantities prodQty = db.ProductQuantities.Find(prodId);

                if (curOrder.Quantity <= availableQty)
                {                    
                    successes++;
                    prodQty.Quantity = availableQty - curOrder.Quantity;
                    db.SaveChanges();
                }

                db.Order.Remove(curOrder);
                db.SaveChanges();
            }

            TempData["Message"] = successes;
            return RedirectToAction("Success", "Orders");

        }

        public ActionResult Success()
        {
            int successes;
            Int32.TryParse(TempData["Message"].ToString(), out successes);
            if (successes == 0)
            {
                ViewBag.Message = "0 orders processed. Out of stock!";
            }
            else
            {
                ViewBag.Message = successes + " orders processed successfuly!";
            }
            return View();
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Order.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Product, "Id", "product_name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,ProductId,Quantity,Price")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                orders.UserId = User.Identity.GetUserId();
                db.Order.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Product, "Id", "product_name", orders.ProductId);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Order.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Product, "Id", "product_name", orders.ProductId);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,ProductId,Quantity,Price")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Product, "Id", "product_name", orders.ProductId);
            return View(orders);
        }



        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            Orders orders = db.Order.Find(id);
            db.Order.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Index");
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
