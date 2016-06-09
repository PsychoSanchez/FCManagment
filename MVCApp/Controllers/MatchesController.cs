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
    public class MatchesController : Controller
    {
        private FClubEntities db = new FClubEntities();

        // GET: Matches
        public ActionResult Index()
        {
            var matches = db.Matches.Include(m => m.Stadiums).Include(m => m.Tournaments);
            return View(matches.ToList());
        }

        // GET: Matches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matches matches = db.Matches.Find(id);
            if (matches == null)
            {
                return HttpNotFound();
            }
            return View(matches);
        }

        // GET: Matches/Create
        public ActionResult Create()
        {
            ViewBag.StadiumID = new SelectList(db.Stadiums, "StadiumID", "Name");
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "Name");
            return View();
        }

        // POST: Matches/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MatchID,TournamentID,StadiumID,Home,Result,Date,EnemyTeam")] Matches matches)
        {
            if (ModelState.IsValid)
            {
                db.Matches.Add(matches);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StadiumID = new SelectList(db.Stadiums, "StadiumID", "StadiumDescription", matches.StadiumID);
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "Name", matches.TournamentID);
            return View(matches);
        }

        // GET: Matches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matches matches = db.Matches.Find(id);
            if (matches == null)
            {
                return HttpNotFound();
            }
            ViewBag.StadiumID = new SelectList(db.Stadiums, "StadiumID", "StadiumDescription", matches.StadiumID);
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "Name", matches.TournamentID);
            return View(matches);
        }

        // POST: Matches/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MatchID,TournamentID,StadiumID,Home,Result,Date,EnemyTeam")] Matches matches)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matches).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StadiumID = new SelectList(db.Stadiums, "StadiumID", "StadiumDescription", matches.StadiumID);
            ViewBag.TournamentID = new SelectList(db.Tournaments, "TournamentID", "Name", matches.TournamentID);
            return View(matches);
        }

        // GET: Matches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matches matches = db.Matches.Find(id);
            if (matches == null)
            {
                return HttpNotFound();
            }
            return View(matches);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matches matches = db.Matches.Find(id);
            db.Matches.Remove(matches);
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
