using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCApp;

namespace MVCApp.Controllers
{
    public class StadiumsController : Controller
    {
        private FClubEntities db = new FClubEntities();

        // GET: Stadiums
        public ActionResult Index()
        {
            return View(db.Stadiums.ToList());
        }

        // GET: Stadiums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stadiums stadiums = db.Stadiums.Find(id);
            if (stadiums == null)
            {
                return HttpNotFound();
            }
            return View(stadiums);
        }

        // GET: Stadiums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stadiums/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StadiumID,StadiumDescription,PeopleAmount,Name")] Stadiums stadiums)
        {
            if (ModelState.IsValid)
            {
                db.Stadiums.Add(stadiums);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stadiums);
        }

        // GET: Stadiums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stadiums stadiums = db.Stadiums.Find(id);
            if (stadiums == null)
            {
                return HttpNotFound();
            }
            return View(stadiums);
        }

        // POST: Stadiums/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StadiumID,StadiumDescription,PeopleAmount,Name")] Stadiums stadiums)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stadiums).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stadiums);
        }

        // GET: Stadiums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stadiums stadiums = db.Stadiums.Find(id);
            if (stadiums == null)
            {
                return HttpNotFound();
            }
            return View(stadiums);
        }

        // POST: Stadiums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stadiums stadiums = db.Stadiums.Find(id);
            db.Stadiums.Remove(stadiums);
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
