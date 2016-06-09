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
    public class TournamentsController : Controller
    {
        private FClubEntities db = new FClubEntities();

        // GET: Tournaments
        public ActionResult Index()
        {
            return View(db.Tournaments.ToList());
        }

        // GET: Tournaments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournaments tournaments = db.Tournaments.Find(id);
            if (tournaments == null)
            {
                return HttpNotFound();
            }
            return View(tournaments);
        }

        // GET: Tournaments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tournaments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TournamentID,Name,Season,Description")] Tournaments tournaments)
        {
            if (ModelState.IsValid)
            {
                db.Tournaments.Add(tournaments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tournaments);
        }

        // GET: Tournaments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournaments tournaments = db.Tournaments.Find(id);
            if (tournaments == null)
            {
                return HttpNotFound();
            }
            return View(tournaments);
        }

        // POST: Tournaments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TournamentID,Name,Season,Description")] Tournaments tournaments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tournaments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tournaments);
        }

        // GET: Tournaments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tournaments tournaments = db.Tournaments.Find(id);
            if (tournaments == null)
            {
                return HttpNotFound();
            }
            return View(tournaments);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tournaments tournaments = db.Tournaments.Find(id);
            db.Tournaments.Remove(tournaments);
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
