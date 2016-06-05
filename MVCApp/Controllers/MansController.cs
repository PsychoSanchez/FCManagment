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
        bool failure = false;
        string FMessage = string.Empty;
        // GET: Mans
        public ActionResult Index()
        {
            try
            {
                ViewBag.Players = db.PlayerInfo;
                ViewBag.Nationalities = db.Nationalities.ToList();
                ViewBag.Mans = db.Mans.Where(x => x.IsDeleted != true).ToList();
                ViewBag.Failure = failure;
                ViewBag.FMessage = FMessage;
                failure = false;
            }
            catch (Exception ex)
            {
                failure = true;
                FMessage = Logger.WriteLog("Ошибка при загрузке страницы", ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Ошибка чтения из базы данных");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(string SearchTerm)
        {
            try
            {
                if (string.IsNullOrEmpty(SearchTerm))
                {
                    ViewBag.Players = db.PlayerInfo;
                    ViewBag.Nationalities = db.Nationalities.ToList();
                    ViewBag.Mans = db.Mans.Where(x => x.IsDeleted != true).ToList();
                    ViewBag.Failure = failure;
                    ViewBag.FMessage = FMessage;
                    failure = false;
                }
                else
                {
                    ViewBag.Players = db.PlayerInfo;
                    ViewBag.Nationalities = db.Nationalities.ToList();
                    ViewBag.Mans = db.Mans.Where(x => x.FirstName.StartsWith(SearchTerm) || x.MiddleName.StartsWith(SearchTerm) || x.LastName.StartsWith(SearchTerm)).ToList();
                    ViewBag.Failure = failure;
                    ViewBag.FMessage = FMessage;
                    failure = false;
                }
                return View("Index");
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при поиске ", ex.Message);
                return View();
            }
        }
        public PartialViewResult CoachTab()
        {
            try
            {
                IEnumerable<MVCApp.Coachs> coach = db.Coachs.Where(x => x.Mans.IsDeleted != true).ToList();
                return PartialView("_Coach", coach);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при загрузке тренеров", ex.Message);
                return PartialView();
            }
        }
        public PartialViewResult PlayersTab()
        {
            try
            {
                ViewBag.Players = db.PlayerInfo.Where(x => x.IsDeleted != true);
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка при добавлении игрока", ex.Message);
            }
            return PartialView("PlayersTab", ViewBag);
        }
        public PartialViewResult ShowAllPlayers()
        {
            try
            {
                ViewBag.Players = db.PlayerInfo;
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка при добавлении игрока", ex.Message);
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
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка при добавлении игрока", ex.Message);
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
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при загрузке ", ex.Message);
                return PartialView();
            }
        }
        public PartialViewResult AllWithDeleted()
        {
            try
            {
                IEnumerable<MVCApp.Mans> Mans = db.Mans.ToList();
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при загрузке таблицы", ex.Message);
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
                IEnumerable<MVCApp.Mans> Mans = db.Mans.Where(x => x.IsDeleted == true).ToList();
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при фильтрации", ex.Message);
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
