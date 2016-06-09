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
    public class ContractsController : Controller
    {
        private FClubEntities db = new FClubEntities();
        static bool failure = false;
        // GET: Contracts
        public ActionResult Index()
        {
            try
            {
                var contracts = db.Contracts.Include(c => c.Agents).Include(c => c.Coachs).Include(c => c.ContractTypes).Include(c => c.Mans).Include(c => c.Players);
                return View(contracts.ToList());
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка при получении информации о контрактах", ex.Message);
            }
            return View();
        }
        public PartialViewResult ShowAll()
        {
            try
            {
                var contracts = db.Contracts.Include(c => c.Agents).Include(c => c.Coachs).Include(c => c.ContractTypes).Include(c => c.Mans).Include(c => c.Players);
                return PartialView("_ContractsTable", contracts);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка при получении информации о контрактах", ex.Message);
            }
            return PartialView();
        }
        public PartialViewResult ShowExpired()
        {
            try
            {
                var contracts = db.Contracts.Where(x => x.ExpireDate < DateTime.Now).Include(c => c.Agents).Include(c => c.Coachs).Include(c => c.ContractTypes).Include(c => c.Mans).Include(c => c.Players);
                return PartialView("_ContractsTable", contracts);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка при получении информации о контрактах", ex.Message);
            }
            return PartialView();
        }
        public PartialViewResult ShowClub()
        {
            try
            {
                var contracts = db.Contracts.Where(x => x.ContractTypeID == 1 || x.ContractTypeID == 3 || x.ContractTypeID == 5).Include(c => c.Agents).Include(c => c.Coachs).Include(c => c.ContractTypes).Include(c => c.Mans).Include(c => c.Players);
                return PartialView("_ContractsTable", contracts);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка при получении информации о контрактах", ex.Message);
            }
            return PartialView();
        }

        // GET: Contracts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contracts contracts = db.Contracts.Find(id);
            if (contracts == null)
            {
                return HttpNotFound();
            }
            return View(contracts);
        }

        // GET: Contracts/Create
        public ActionResult Create()
        {
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "Info");
            ViewBag.CoachID = new SelectList(db.Coachs, "CoachID", "Info");
            ViewBag.ContractTypeID = new SelectList(db.ContractTypes, "ContractTypeID", "ContractTypeDescription");
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName");
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "ShirtName");
            return View();
        }

        // POST: Contracts/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContractID,StartDate,ExpireDate,Money,PassportNumber,Tax,PlayerID,CoachID,AgentID,ManID,ContractTypeID,ClearMoney")] Contracts contracts)
        {
            if (ModelState.IsValid)
            {
                db.Contracts.Add(contracts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "Info", contracts.AgentID);
            ViewBag.CoachID = new SelectList(db.Coachs, "CoachID", "Info", contracts.CoachID);
            ViewBag.ContractTypeID = new SelectList(db.ContractTypes, "ContractTypeID", "ContractTypeDescription", contracts.ContractTypeID);
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", contracts.ManID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "ShirtName", contracts.PlayerID);
            return View(contracts);
        }

        // GET: Contracts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contracts contracts = db.Contracts.Find(id);
            if (contracts == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "Info", contracts.AgentID);
            ViewBag.CoachID = new SelectList(db.Coachs, "CoachID", "Info", contracts.CoachID);
            ViewBag.ContractTypeID = new SelectList(db.ContractTypes, "ContractTypeID", "ContractTypeDescription", contracts.ContractTypeID);
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", contracts.ManID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "ShirtName", contracts.PlayerID);
            return View(contracts);
        }

        // POST: Contracts/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContractID,StartDate,ExpireDate,Money,PassportNumber,Tax,PlayerID,CoachID,AgentID,ManID,ContractTypeID,ClearMoney")] Contracts contracts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contracts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentID = new SelectList(db.Agents, "AgentID", "Info", contracts.AgentID);
            ViewBag.CoachID = new SelectList(db.Coachs, "CoachID", "Info", contracts.CoachID);
            ViewBag.ContractTypeID = new SelectList(db.ContractTypes, "ContractTypeID", "ContractTypeDescription", contracts.ContractTypeID);
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", contracts.ManID);
            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "ShirtName", contracts.PlayerID);
            return View(contracts);
        }

        // GET: Contracts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contracts contracts = db.Contracts.Find(id);
            if (contracts == null)
            {
                return HttpNotFound();
            }
            return View(contracts);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Contracts contracts = db.Contracts.Find(id);
                db.Contracts.Remove(contracts);
                db.SaveChanges();
                Logger.WriteLog("Контракт успешно удален. ID " + contracts.ContractID);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Произошла ошибка при удалении контракта", ex.Message);
            }
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
