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
        static bool failure = false;
        static string FMessage = string.Empty;
        [HttpPost]
        public ActionResult AddCoach(string Name, string Nationality, string Position, DateTime Birthday, DateTime ContractEnd, double ContractMoney)
        {
            Mans man = new Mans();
            Coachs coach = new Coachs();
            Contracts contract = new Contracts();
            try
            {
                man.NationalityID = int.Parse(Nationality);
                man.Birthday = Birthday;
                man.Age = (short?)(DateTime.Now.Year - Birthday.Year);
                man.LastName = string.Empty;
                man.PersonalPositionID = int.Parse(Position);
                var playername = Name.Split(' ');
                if (playername.Length == 0)
                {
                    throw new Exception("Имя не определено");
                }
                for (int i = 0; i < playername.Length; i++)
                {
                    switch (i)
                    {
                        case 0:
                            man.MiddleName = playername[i];
                            break;
                        case 1:
                            man.FirstName = playername[i];
                            break;
                        default:
                            man.LastName += playername[i] + " ";
                            break;
                    }
                }
                db.Mans.Add(man);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                failure = true;
                FMessage = Logger.WriteLog("Ошибка при добавлении игрока", ex.Message);
                try
                {
                    db.Mans.Remove(man);
                }
                catch { }
                return RedirectToAction("Index");
            }
            try
            {
                coach.ManID = man.ManID;
                db.Coachs.Add(coach);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                failure = true;
                FMessage = Logger.WriteLog("Ошибка при добавлении игрока", ex.Message);
                db.Mans.Remove(man);
                try
                {
                    db.SaveChanges();
                    db.Coachs.Remove(coach);
                    db.SaveChanges();
                }
                catch { }
                return RedirectToAction("Index");
            }
            try
            {
                contract.CoachID = coach.CoachID;
                contract.ContractTypeID = 1;
                contract.StartDate = DateTime.Now;
                contract.ExpireDate = ContractEnd;
                contract.Money = (decimal?)ContractMoney;
                contract.Tax = (decimal?)ContractMoney / 100 * 13;
                db.Contracts.Add(contract);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                failure = true;
                FMessage = Logger.WriteLog("Ошибка при добавлении игрока", ex.Message);
                try
                {

                    db.Mans.Remove(man);
                    db.SaveChanges();
                    db.Coachs.Remove(coach);
                    db.SaveChanges();
                    db.Contracts.Remove(contract);
                    db.SaveChanges();
                }
                catch { }
                return RedirectToAction("Index");
            }
            Logger.WriteLog("Игрок успешно добавлен в базу данных: " + man.MiddleName + " ID игрока: " + coach.CoachID + " ID профиля: " + man.ManID + " ID контракта: " + contract.ContractID);
            return RedirectToAction("Index");
        }
        // GET: Coachss
        public ActionResult Index()
        {
            ViewBag.Coachs = db.Coachs.Where(x => x.Mans.IsDeleted != true);
            ViewBag.Positions = db.PersosnalPosition.Where(x => x.Description.ToLower().Contains("тренер")).ToList();
            ViewBag.Nationalities = db.Nationalities.ToList();
            ViewBag.Failure = failure;
            ViewBag.FMessage = FMessage;
            failure = false;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string SearchTerm)
        {
            ViewBag.Coachs = db.Coachs.Where(x => x.Mans.FirstName.StartsWith(SearchTerm) || x.Mans.MiddleName.StartsWith(SearchTerm) || x.Mans.LastName.StartsWith(SearchTerm));
            ViewBag.Positions = db.PersosnalPosition.Where(x => x.Description.ToLower().Contains("тренер")).ToList();
            ViewBag.Nationalities = db.Nationalities.ToList();
            ViewBag.FMessage = FMessage;
            ViewBag.Failure = failure;
            failure = false;
            return View();
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
            ViewBag.Positions = new SelectList(db.PersosnalPosition, "PersonalPositionID", "Description");
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
