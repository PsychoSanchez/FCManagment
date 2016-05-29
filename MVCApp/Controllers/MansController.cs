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
    public class MansController : Controller
    {
        private FCEntities db = new FCEntities();

        // GET: Mans
        public ActionResult Index()
        {
            try
            {
                ViewBag.Players = db.PlayerInfo;
                ViewBag.Nationalities = db.Nationaites.ToList();
                ViewBag.Mans = db.Mans.ToList();
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Ошибка чтения из базы данных");
            }
            return View();
        }

        public PartialViewResult CoachTab()
        {
            try
            {
                IEnumerable<MVCApp.Coachs> coach = db.Coachs.ToList();
                return PartialView("_Coach", coach);
            }
            catch
            {
                return PartialView();
            }


        }
        public PartialViewResult PlayersTab()
        {
            try
            {
                ViewBag.Players = db.PlayerInfo;
                ViewBag.Nationalities = db.Nationaites.ToList();
            }
            catch
            {
            }
            return PartialView("PlayersTab", ViewBag);
        }

        public PartialViewResult AgentsTab()
        {
            try
            {
                IEnumerable<MVCApp.Agents> agents = db.Agents.ToList();
                return PartialView("AgentsTab", agents);
            }
            catch
            {
                return PartialView();
            }
        }
        public PartialViewResult All()
        {
            try
            {
                IEnumerable<MVCApp.Mans> Mans = db.Mans.ToList();
                return PartialView("All", Mans);
            }
            catch
            {
                return PartialView();
            }
        }

        // GET: Mans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mans mans = db.Mans.Find(id);
            if (mans == null)
            {
                return HttpNotFound();
            }
            return View(mans);
        }

        // GET: Mans/Create
        public ActionResult Create()
        {
            ViewBag.NationalityID = new SelectList(db.Nationaites, "NationalityID", "Nationality");
            ViewBag.PersonalPositionID = new SelectList(db.PersonalPosition, "PersonalPositionID", "Description");
            return View();
        }

        // POST: Mans/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManID,MiddleName,FirstName,LastName,Age,Birthday,NationalityID,Height,Weight,Adress,PhoneNumber,Photo,IsDeleted,PersonalPositionID")] Mans mans)
        {
            if (ModelState.IsValid)
            {
                db.Mans.Add(mans);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NationalityID = new SelectList(db.Nationaites, "NationalityID", "Nationality", mans.NationalityID);
            ViewBag.PersonalPositionID = new SelectList(db.PersonalPosition, "PersonalPositionID", "Description", mans.PersonalPositionID);
            return View(mans);
        }

        // GET: Mans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mans mans = db.Mans.Find(id);
            if (mans == null)
            {
                return HttpNotFound();
            }
            ViewBag.NationalityID = new SelectList(db.Nationaites, "NationalityID", "Nationality", mans.NationalityID);
            ViewBag.PersonalPositionID = new SelectList(db.PersonalPosition, "PersonalPositionID", "Description", mans.PersonalPositionID);
            return View(mans);
        }

        // POST: Mans/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManID,MiddleName,FirstName,LastName,Age,Birthday,NationalityID,Height,Weight,Adress,PhoneNumber,Photo,IsDeleted,PersonalPositionID")] Mans mans)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mans).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NationalityID = new SelectList(db.Nationaites, "NationalityID", "Nationality", mans.NationalityID);
            ViewBag.PersonalPositionID = new SelectList(db.PersonalPosition, "PersonalPositionID", "Description", mans.PersonalPositionID);
            return View(mans);
        }

        // GET: Mans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mans mans = db.Mans.Find(id);
            if (mans == null)
            {
                return HttpNotFound();
            }
            return View(mans);
        }

        // POST: Mans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mans mans = db.Mans.Find(id);
            db.Mans.Remove(mans);
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
