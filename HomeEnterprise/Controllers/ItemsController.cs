using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HomeEnterprise.Models;
using Microsoft.AspNet.Identity;

namespace HomeEnterprise.Controllers
{
    public class ItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Items
        public ActionResult Index()
        {
            var items = db.Items.Include(i => i.ItemType).Include(i => i.Owner).Include(i => i.Quality);
            return View(items.ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName");
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email");
            ViewBag.QualityId = new SelectList(db.Qualities, "Id", "QualityName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Quantity,Price,OwnerId,QualityId,ItemTypeId")] Item item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    item.OwnerId = User.Identity.GetUserId();
                    db.Items.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e) {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", item.OwnerId);
            ViewBag.QualityId = new SelectList(db.Qualities, "Id", "QualityName", item.QualityId);
            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", item.OwnerId);
            ViewBag.QualityId = new SelectList(db.Qualities, "Id", "QualityName", item.QualityId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Quantity,Price,OwnerId,QualityId,ItemTypeId")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", item.OwnerId);
            ViewBag.QualityId = new SelectList(db.Qualities, "Id", "QualityName", item.QualityId);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
