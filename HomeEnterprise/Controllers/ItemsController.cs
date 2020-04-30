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
using PagedList;

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
        // GET: ItemsList
        public ActionResult List(int? page)
        {
            /*
            //ApplicationUser u = new ApplicationUser();
            //u.UserName = "";
            //ViewBag.Owners = db.Users.Where(u => u.Items.Count > 0);
            //ViewBag.Owners = new SelectList(db.Users, "UserName");
            var users = db.Users.ToList();
            ViewBag.Owners = users
            var items = db.Items.Include(i => i.ItemType).Include(i => i.Owner).Include(i => i.Quality);
            List<Item> its = new List<Item>();
            its = items.ToList();
            its.Reverse();
            return View(its);
            */
            if(Session["OwnerFilter"] != null)
            {
                string ownerFilter = Session["OwnerFilter"].ToString();
                ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");
                var items = db.Items.Include(i => i.ItemType).Include(i => i.Owner).Include(i => i.Quality).Where(x => x.OwnerId == ownerFilter);
                List<Item> its = new List<Item>();
                its = items.ToList();
                its.Reverse();
                return View(its.ToPagedList(page ?? 1, 6));
            }
            else
            {
                ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");
                var items = db.Items.Include(i => i.ItemType).Include(i => i.Owner).Include(i => i.Quality);
                List<Item> its = new List<Item>();
                its = items.ToList();
                its.Reverse();
                //Session.Remove("OwnerFilter");
                Session["OwnerFilter"] = null;
                return View(its.ToPagedList(page ?? 1, 6));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(string ownerId, int? page)
        {
            List<Item> its = new List<Item>();
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "UserName");
            ViewBag.fil = ownerId;
            Session["OwnerFilter"] = ownerId;
            //string sfd = Session["OwnerFilter"].ToString();
            var sellerId = (from c in db.Items
                            where c.OwnerId == ownerId
                            select c.OwnerId).FirstOrDefault();
            //if owner has items listed
            if (sellerId != null)
            {
                var items = db.Items.Include(i => i.ItemType).Include(i => i.Owner).Include(i => i.Quality).Where(x => x.OwnerId == ownerId);
                its = items.ToList();
                //List<Item> its = new List<Item>();

            }
            else
            {
                var items = db.Items.Include(i => i.ItemType).Include(i => i.Owner).Include(i => i.Quality);
                its = items.ToList();
                //List<Item> its = new List<Item>();
            }            
            its.Reverse();
            return View(its.ToPagedList(page ?? 1, 6));

            //var items = db.Items.Include(i => i.ItemType).Include(i => i.Owner).Include(i => i.Quality).OrderByDescending(x => x.Id);

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
            if (item.OwnerId == User.Identity.GetUserId())
            {
                ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName", item.ItemTypeId);
                //ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", item.OwnerId);
                ViewBag.QualityId = new SelectList(db.Qualities, "Id", "QualityName", item.QualityId);
                return View(item);
            }
            else
            {
                return RedirectToAction("Index");
            }
            /*
            ViewBag.ItemTypeId = new SelectList(db.ItemTypes, "Id", "TypeName", item.ItemTypeId);
            //ViewBag.OwnerId = new SelectList(db.Users, "Id", "Email", item.OwnerId);
            ViewBag.QualityId = new SelectList(db.Qualities, "Id", "QualityName", item.QualityId);
            return View(item);
            */
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Quantity,Price,OwnerId,QualityId,ItemTypeId")] Item item)
        {
            if (item.OwnerId == User.Identity.GetUserId())
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
            return RedirectToAction("Index");
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
            if (item.OwnerId == User.Identity.GetUserId())
            {
                db.Items.Remove(item);
                db.SaveChanges();
            }
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
