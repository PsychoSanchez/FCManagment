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
    public class CoachssController : Controller
    {
        private FClubEntities db = new FClubEntities();

        // GET: Coachss
        public ActionResult Index()
        {
            var coachs = db.Coachs.Include(c => c.Mans);
            return View(coachs.ToList());
        }

        // GET: Coachss/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coachs coachs = db.Coachs.Find(id);
            if (coachs == null)
            {
                return HttpNotFound();
            }
            return View(coachs);
        }

        // GET: Coachss/Create
        public ActionResult Create()
        {
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName");
            return View();
        }

        // POST: Coachss/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CoachID,ManID,Info")] Coachs coachs)
        {
            if (ModelState.IsValid)
            {
                db.Coachs.Add(coachs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", coachs.ManID);
            return View(coachs);
        }

        // GET: Coachss/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coachs coachs = db.Coachs.Find(id);
            if (coachs == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", coachs.ManID);
            return View(coachs);
        }

        // POST: Coachss/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CoachID,ManID,Info")] Coachs coachs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coachs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", coachs.ManID);
            return View(coachs);
        }

        // GET: Coachss/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coachs coachs = db.Coachs.Find(id);
            if (coachs == null)
            {
                return HttpNotFound();
            }
            return View(coachs);
        }

        // POST: Coachss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coachs coachs = db.Coachs.Find(id);
            db.Coachs.Remove(coachs);
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
