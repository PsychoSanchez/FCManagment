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
    public class TrainingsController : Controller
    {
        private FClubEntities db = new FClubEntities();

        // GET: Trainings
        public ActionResult Index()
        {
            return View(db.Trainings.ToList());
        }

        // GET: Trainings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainings trainings = db.Trainings.Find(id);
            if (trainings == null)
            {
                return HttpNotFound();
            }
            return View(trainings);
        }

        // GET: Trainings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trainings/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrainingID,Date,Duration,TrainInfo,Address")] Trainings trainings)
        {
            if (ModelState.IsValid)
            {
                db.Trainings.Add(trainings);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trainings);
        }

        // GET: Trainings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainings trainings = db.Trainings.Find(id);
            if (trainings == null)
            {
                return HttpNotFound();
            }
            return View(trainings);
        }

        // POST: Trainings/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainingID,Date,Duration,TrainInfo,Address")] Trainings trainings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainings);
        }

        // GET: Trainings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trainings trainings = db.Trainings.Find(id);
            if (trainings == null)
            {
                return HttpNotFound();
            }
            return View(trainings);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trainings trainings = db.Trainings.Find(id);
            db.Trainings.Remove(trainings);
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
