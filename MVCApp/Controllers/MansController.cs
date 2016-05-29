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
        private FClubEntities db = new FClubEntities();

        // GET: Mans
        public ActionResult Index()
        {
            try
            {
                //ViewBag.Players = db.PlayerInfo;
                //ViewBag.Nationalities = db.Nationaites.ToList();
                //ViewBag.Mans = db.Mans.ToList();
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
                IEnumerable<MVCApp.Coachs> coach = db.Coachs.Where(x => x.Mans.IsDeleted != true).ToList();
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
                ViewBag.Nationalities = db.Nationalities.ToList();
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
                IEnumerable<MVCApp.Mans> Mans = db.Mans.Where(x => x.IsDeleted != true).ToList();
                return PartialView("All", Mans);
            }
            catch
            {
                return PartialView();
            }
        }
        [HttpPost]
        public ActionResult All(string searchTerm)
        {
            try
            {
                IEnumerable<MVCApp.Mans> Mans;
                if (string.IsNullOrEmpty(searchTerm))
                {
                    Mans = db.Mans.Where(x => x.IsDeleted != true).ToList();
                }
                else
                {
                    Mans = db.Mans.Where(x => x.FirstName.StartsWith(searchTerm) || x.MiddleName.StartsWith(searchTerm) || x.LastName.StartsWith(searchTerm)).ToList();
                }
                return PartialView("All", Mans);
            }
            catch
            {
                return PartialView();
            }
        }
        public JsonResult FindMan(string term)
        {
            List<string> Mans;
            Mans = db.Mans.Where(x => x.FirstName.StartsWith(term) || x.MiddleName.StartsWith(term) || x.LastName.StartsWith(term))
                .Select(y => y.MiddleName).ToList();

            return Json(Mans, JsonRequestBehavior.AllowGet);

        }
        public PartialViewResult ShowFired()
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
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "Nationality");
            ViewBag.PersonalPositionID = new SelectList(db.PersosnalPosition, "PersonalPositionID", "Description");
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

            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "Nationality", mans.NationalityID);
            ViewBag.PersonalPositionID = new SelectList(db.PersosnalPosition, "PersonalPositionID", "Description", mans.PersonalPositionID);
            return View(mans);
        }

        // GET: Mans/Edit/5
        public ActionResult Edit(int? id)
        {
            try
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
                ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "Nationality", mans.NationalityID);
                ViewBag.PersonalPositionID = new SelectList(db.PersosnalPosition, "PersonalPositionID", "Description", mans.PersonalPositionID);
                return View(mans);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.Message);
            }
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
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "Nationality", mans.NationalityID);
            ViewBag.PersonalPositionID = new SelectList(db.PersosnalPosition, "PersonalPositionID", "Description", mans.PersonalPositionID);
            return View(mans);
        }

        // GET: Mans/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mans mans = db.Mans.Find(id);
            mans.IsDeleted = true;
            db.SaveChanges();
            if (mans == null)
            {
                return HttpNotFound();
            }
            return View("Index");
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
