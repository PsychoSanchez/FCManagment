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
    public class AgentController : Controller
    {
        private FClubEntities db = new FClubEntities();
        static bool failure = false;
        static string FMessage = string.Empty;
        [HttpPost]
        public ActionResult AddAgent(string Name, string Nationality, DateTime Birthday)
        {
            Mans man = new Mans();
            Agents agent = new Agents();
            Contracts contract = new Contracts();
            try
            {
                man.NationalityID = int.Parse(Nationality);
                man.Birthday = Birthday;
                man.Age = (short?)(DateTime.Now.Year - Birthday.Year);
                man.LastName = string.Empty;
                man.PersonalPositionID = 15;
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
                agent.ManID = man.ManID;
                db.Agents.Add(agent);
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
                    db.Agents.Remove(agent);
                    db.SaveChanges();
                }
                catch { }
                return RedirectToAction("Index");
            }
            Logger.WriteLog("Игрок успешно добавлен в базу данных: " + man.MiddleName + " ID Agents: " + agent.AgentID + " ID профиля: " + man.ManID + " ID контракта: " + contract.ContractID);
            return RedirectToAction("Index");
        }
        // GET: Agent
        public ActionResult Index()
        {
            ViewBag.Agents = db.Agents.Where(x => x.Mans.IsDeleted != true);
            ViewBag.Failure = failure;
            ViewBag.Nationalities = db.Nationalities.ToList();
            ViewBag.FMessage = FMessage;
            failure = false;
            return View();
        }
        [HttpPost]
        public ActionResult Index(string SearchTerm)
        {
            try
            {
                if (string.IsNullOrEmpty(SearchTerm))
                {
                    ViewBag.Agents = db.Agents.Where(x => x.Mans.IsDeleted != true);
                    ViewBag.Failure = failure;
                    ViewBag.Nationalities = db.Nationalities.ToList();
                    ViewBag.FMessage = FMessage;
                    failure = false;
                }
                else
                {
                    ViewBag.Agents = db.Agents.Where(x => x.Mans.FirstName.StartsWith(SearchTerm) || x.Mans.MiddleName.StartsWith(SearchTerm) || x.Mans.LastName.StartsWith(SearchTerm));
                    ViewBag.Failure = failure;
                    ViewBag.Nationalities = db.Nationalities.ToList();
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
        // GET: Agent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agents agents = db.Agents.Find(id);
            if (agents == null)
            {
                return HttpNotFound();
            }
            return View(agents);
        }

        // GET: Agent/Create
        public ActionResult Create()
        {
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName");
            return View();
        }

        // POST: Agent/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgentID,ManID,Info")] Agents agents)
        {
            if (ModelState.IsValid)
            {
                db.Agents.Add(agents);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", agents.ManID);
            return View(agents);
        }

        // GET: Agent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agents agents = db.Agents.Find(id);
            if (agents == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", agents.ManID);
            return View(agents);
        }

        // POST: Agent/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentID,ManID,Info")] Agents agents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", agents.ManID);
            return View(agents);
        }

        // GET: Agent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agents agents = db.Agents.Find(id);
            if (agents == null)
            {
                return HttpNotFound();
            }
            return View(agents);
        }

        // POST: Agent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agents agents = db.Agents.Find(id);
            db.Agents.Remove(agents);
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
