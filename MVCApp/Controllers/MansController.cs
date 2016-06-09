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
                Mans = db.Mans.Where(x => x.IsDeleted != true).ToList();
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
        public ActionResult Index(string SearchTerm, string StartAge, string EndAge, string Nationality)
        {
            try
            {
                ViewBag.Players = db.PlayerInfo;
                ViewBag.Nationalities = db.Nationalities.ToList();
                Mans = db.Mans.ToList();
                ViewBag.Failure = failure;
                ViewBag.FMessage = FMessage;
                failure = false;
                if (!string.IsNullOrEmpty(SearchTerm))
                {
                    //x => x.FirstName.ToLower().StartsWith(SearchTerm) || || x.LastName.ToLower().StartsWith(SearchTerm)
                    List<Mans> persons = new List<MVCApp.Mans>();
                    foreach (var man in Mans)
                    {
                        try
                        {
                            if (man.MiddleName.ToLower().Contains(SearchTerm))
                            {
                                persons.Add(man);
                            }
                            else if (man.FirstName.ToLower().Contains(SearchTerm))
                            {
                                persons.Add(man);
                            }
                            else if (man.LastName.ToLower().Contains(SearchTerm))
                            {
                                persons.Add(man);
                            }
                        }
                        catch
                        {

                        }
                    }
                    Mans = persons;
                }
                if (!string.IsNullOrEmpty(StartAge))
                {
                    Mans = Mans.Where(x => x.Age > int.Parse(StartAge)).ToList();
                }
                if (!string.IsNullOrEmpty(EndAge))
                {
                    Mans = Mans.Where(x => x.Age < int.Parse(EndAge)).ToList();
                }
                if (!string.IsNullOrEmpty(Nationality))
                {
                    Mans = Mans.Where(x => x.NationalityID == int.Parse(Nationality)).ToList();
                }
                ViewBag.Mans = Mans;
                return View("Index");
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Произошла ошибка при поиске ", ex.Message);
                return View("Index");
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


        public PartialViewResult All()
        {
            try
            {
                Mans = db.Mans.Where(x => x.IsDeleted != true).ToList();
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при загрузке ", ex.Message);
                return PartialView();
            }
        }
        public PartialViewResult OnlyWorking()
        {
            try
            {
                Mans = Mans.Where(x => x.IsDeleted != true).ToList();
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при загрузке ", ex.Message);
                return PartialView();
            }
        }
        static IEnumerable<MVCApp.Mans> Mans;
        public PartialViewResult AllWithDeleted()
        {
            try
            {
                Mans = db.Mans.ToList();
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
                Mans = Mans.Where(x => x.IsDeleted == true).ToList();
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при фильтрации", ex.Message);
                return PartialView();
            }
        }
        public PartialViewResult SortFIO()
        {
            try
            {
                if (sortValue)
                {
                    Mans = Mans.OrderBy(x => x.MiddleName).ToList();
                }
                else
                {
                    Mans = Mans.OrderByDescending(x => x.MiddleName).ToList();
                }
                sortValue = !sortValue;
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при фильтрации", ex.Message);
                return PartialView();
            }
        }
        public PartialViewResult SortAge()
        {
            try
            {
                if (sortValue)
                {
                    Mans = Mans.OrderBy(x => x.Age).ToList();
                }
                else
                {
                    Mans = Mans.OrderByDescending(x => x.Age).ToList();
                }
                sortValue = !sortValue;
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при фильтрации", ex.Message);
                return PartialView();
            }
        }
        public PartialViewResult SortDate()
        {
            try
            {
                if (sortValue)
                {
                    Mans = Mans.OrderBy(x => x.Birthday).ToList();
                }
                else
                {
                    Mans = Mans.OrderByDescending(x => x.Birthday).ToList();
                }
                sortValue = !sortValue;
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при фильтрации", ex.Message);
                return PartialView();
            }
        }
        public PartialViewResult SortPos()
        {
            try
            {
                if (sortValue)
                {
                    Mans = Mans.OrderBy(x => x.PersonalPositionID).ToList();
                }
                else
                {
                    Mans = Mans.OrderByDescending(x => x.PersonalPositionID).ToList();
                }
                sortValue = !sortValue;
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при фильтрации", ex.Message);
                return PartialView();
            }
        }
        static bool sortValue = false;
        public PartialViewResult SortNat()
        {
            try
            {
                if (sortValue)
                {
                    Mans = Mans.OrderBy(x => x.NationalityID).ToList();
                }
                else
                {
                    Mans = Mans.OrderByDescending(x => x.NationalityID).ToList();
                }
                sortValue = !sortValue;
                return PartialView("All", Mans);
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при фильтрации", ex.Message);
                return PartialView();
            }
        }





        public class Statistics
        {
            public int Goals;
            public int Assists;
            public int TimeOnField;
            public int YellowCards;
            public int RedCards;
        }
        // GET: Mans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {

                if (db.Players.Where(x => x.ManID == id).Count() > 0)
                {
                    var player = db.Players.Where(x => x.ManID == id).FirstOrDefault();
                    //var stat = db.GetPlayerStat(id);
                    var statistics = db.GetPlayerStatistics(player.PlayerID);
                    //foreach(var s in statistics)
                    //{
                    //    s.Go
                    //}
                    ViewBag.PlayerStat = statistics;
                    ViewBag.LastMatch = db.PlayerSquad.Where(x => x.PlayerID == player.PlayerID);
                    //Statistics stat = new Statistics();
                    //ViewBag.PlayerStat=db.GetPlayerStatistics(id,out stat.Goals, out stat.Assists, out stat.TimeOnField, out stat.YellowCards, out stat.RedCards)

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Не удалось получить статистику по игроку", ex.Message);
            }
            Mans mans = db.Mans.Find(id);
            ViewBag.mans = mans;
            if (mans == null)
            {
                return HttpNotFound();
            }
            return View(ViewBag);
        }

        // GET: Mans/Create
        public ActionResult Create()
        {
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "Nationality");
            ViewBag.PersonalPositionID = new SelectList(db.PersosnalPosition.Where(x => new[] { 5, 6, 7, 9, 14 }.Contains(x.PersonalPositionID)), "PersonalPositionID", "Description");
            return View();
        }

        // POST: Mans/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManID,MiddleName,FirstName,LastName,Age,Birthday,NationalityID,Height,Weight,Adress,PhoneNumber,Photo,IsDeleted,PersonalPositionID")] Mans mans)
        {
            if (string.IsNullOrEmpty(mans.MiddleName) || string.IsNullOrEmpty(mans.FirstName))
            {
                failure = true;
                ViewBag.Failure = failure;
                ViewBag.FMessage = "Не удалось добавить работника";
                return RedirectToAction("Index");
            }
            if (mans.Birthday != null)
                mans.Age = (short?)(DateTime.Now.Year - mans.Birthday.Value.Year);
            if (ModelState.IsValid)
            {
                try
                {
                    db.Mans.Add(mans);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("Ошибка при добавлении пользователя в бд", ex.Message);
                }
                Logger.WriteLog("Работник успешно добавлен ID " + mans.ManID);
                return RedirectToAction("Index");
            }

            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "Nationality", mans.NationalityID);
            ViewBag.PersonalPositionID = new SelectList(db.PersosnalPosition.Where(x => new[] { 5, 6, 7, 9, 14 }.Contains(x.PersonalPositionID)), "PersonalPositionID", "Description", mans.PersonalPositionID);
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
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.WriteLog("Ошибка при редактировании пользователя в бд", ex.Message);
                }
                Logger.WriteLog("Пользователь успешно отредактирован в бд ID " + mans.ManID);
                return RedirectToAction("Index");
            }
            ViewBag.NationalityID = new SelectList(db.Nationalities, "NationalityID", "Nationality", mans.NationalityID);
            ViewBag.PersonalPositionID = new SelectList(db.PersosnalPosition, "PersonalPositionID", "Description", mans.PersonalPositionID);
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
            try
            {
                mans.IsDeleted = true;
                db.SaveChanges();
                Logger.WriteLog("Удаление прошло успешно: ID ", mans.ManID.ToString());
            }
            catch (Exception ex)
            {
                FMessage = Logger.WriteLog("Ошибка при удалении", ex.Message);
                failure = true;
                return View("Index");
            }
            ViewBag.Players = db.PlayerInfo;
            ViewBag.Nationalities = db.Nationalities.ToList();
            ViewBag.Mans = db.Mans.Where(x => x.IsDeleted != true).ToList();
            ViewBag.Failure = failure;
            ViewBag.FMessage = FMessage;
            failure = false;
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
