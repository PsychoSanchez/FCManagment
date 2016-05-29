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
    public class PlayersController : Controller
    {
        private FClubEntities db = new FClubEntities();

        // GET: Players
        public ActionResult Index()
        {
            try
            {
                ViewBag.Players = db.PlayerInfo;
                ViewBag.Nationalities = db.Nationalities.ToList();
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
                IEnumerable<MVCApp.Mans> Mans = db.Mans.ToList();
                return PartialView("All", Mans);
            }
            catch
            {
                return PartialView();
            }
        }

        // GET: Players/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Players players = db.Players.Find(id);
            if (players == null)
            {
                return HttpNotFound();
            }
            return View(players);
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName");
            return View();
        }

        // POST: Players/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PlayerID,ManID,AgentContractID,TransferCost,TransferStatus,NationalTeamInvite,Injury,PlayerNumber,PlayerPositionsID,ShirtName")] Players players)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(players);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", players.ManID);
            return View(players);
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Players players = db.Players.Find(id);
            if (players == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", players.ManID);
            return View(players);
        }

        // POST: Players/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PlayerID,ManID,AgentContractID,TransferCost,TransferStatus,NationalTeamInvite,Injury,PlayerNumber,PlayerPositionsID,ShirtName")] Players players)
        {
            if (ModelState.IsValid)
            {
                db.Entry(players).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName", players.ManID);
            return View(players);
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Players players = db.Players.Find(id);
            if (players == null)
            {
                return HttpNotFound();
            }
            return View(players);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Players players = db.Players.Find(id);
            db.Players.Remove(players);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
