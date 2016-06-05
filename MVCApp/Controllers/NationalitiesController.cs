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
    public class NationalitiesController : Controller
    {
        private FClubEntities db = new FClubEntities();

        // GET: Nationalities
        public ActionResult Index()
        {
            return View(db.Nationalities.ToList());
        }

        // GET: Nationalities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationalities nationalities = db.Nationalities.Find(id);
            if (nationalities == null)
            {
                return HttpNotFound();
            }
            return View(nationalities);
        }

        // GET: Nationalities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nationalities/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NationalityID,Nationality")] Nationalities nationalities)
        {
            if (ModelState.IsValid)
            {
                db.Nationalities.Add(nationalities);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nationalities);
        }

        // GET: Nationalities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationalities nationalities = db.Nationalities.Find(id);
            if (nationalities == null)
            {
                return HttpNotFound();
            }
            return View(nationalities);
        }

        // POST: Nationalities/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NationalityID,Nationality")] Nationalities nationalities)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nationalities).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nationalities);
        }

        // GET: Nationalities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nationalities nationalities = db.Nationalities.Find(id);
            if (nationalities == null)
            {
                return HttpNotFound();
            }
            return View(nationalities);
        }

        // POST: Nationalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Nationalities nationalities = db.Nationalities.Find(id);
            db.Nationalities.Remove(nationalities);
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
