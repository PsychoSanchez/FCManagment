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
    public class AgentsController : Controller
    {
        private FCEntities db = new FCEntities();

        // GET: Agents
        public ActionResult Index()
        {
            var agents = db.Agents.Include(a => a.Mans);
            return View(agents.ToList());
        }

        // GET: Agents/Details/5
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

        // GET: Agents/Create
        public ActionResult Create()
        {
            ViewBag.ManID = new SelectList(db.Mans, "ManID", "MiddleName");
            return View();
        }

        // POST: Agents/Create
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

        // GET: Agents/Edit/5
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

        // POST: Agents/Edit/5
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

        // GET: Agents/Delete/5
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

        // POST: Agents/Delete/5
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
