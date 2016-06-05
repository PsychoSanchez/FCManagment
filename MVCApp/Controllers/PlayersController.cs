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
        static bool failure = false;
        static string FMessage = string.Empty;
        [HttpPost]
        public ActionResult AddPlayerActionResult(string PlayerName, string ShirtName, string Nationality, short? PlayerNumber, DateTime Birthday, DateTime ContractStart, DateTime ContractEnd, double ContractMoney, double TransferPrice, short? Height, short? Weight)
        {
            Mans man = new Mans();
            Players player = new Players();
            Contracts contract = new Contracts();
            try
            {
                man.NationalityID = int.Parse(Nationality);
                man.Birthday = Birthday;
                man.Age = (short?)(DateTime.Now.Year - Birthday.Year);
                man.LastName = string.Empty;
                man.Height = Height;
                man.Weight = Weight;
                var playername = PlayerName.Split(' ');
                if (playername.Length == 0)
                {
                    throw new Exception("»м¤ не определено");
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
                man.PersonalPositionID = 1;
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
                player.ManID = man.ManID;
                if (db.Players.Where(x => x.PlayerNumber == PlayerNumber).Count() > 0)
                    throw new Exception("Игрок с таким номером уже существует");
                player.PlayerNumber = PlayerNumber;
                player.TransferCost = (decimal?)TransferPrice;
                player.ShirtName = ShirtName;
                if (db.Players.Where(x => x.ShirtName == ShirtName).Count() > 0)
                    throw new Exception("Игрок с таким именем уже существует");
                db.Players.Add(player);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                failure = true;
                FMessage = Logger.WriteLog("Ошибка при добавлении игрока", ex.Message);
                db.Mans.Remove(man);
                try
                {
                    db.Players.Remove(player);
                }
                catch { }
                return RedirectToAction("Index");
            }
            try
            {
                contract.PlayerID = player.PlayerID;
                contract.ContractTypeID = 1;
                contract.StartDate = ContractStart;
                contract.ExpireDate = ContractEnd;
                contract.Money = (decimal?)ContractMoney;
                contract.Tax = (decimal?)ContractMoney / 100 * 13;
                contract.PlayerID = player.PlayerID;
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
                    db.Players.Remove(player);
                    db.Contracts.Remove(contract);
                }
                catch { }
                return RedirectToAction("Index");
            }
            Logger.WriteLog("Игрок успешно добавлен в базу данных: " + man.MiddleName + " ID игрока: " + player.PlayerID + " ID профиля: " + man.ManID + " ID контракта: " + contract.ContractID);
            return RedirectToAction("Index");
        }
        // GET: Players
        public ActionResult Index()
        {
            try
            {
                ViewBag.Players = db.PlayerInfo;
                ViewBag.Nationalities = db.Nationalities.ToList();
                ViewBag.Mans = db.Mans.ToList();
                ViewBag.Failure = failure;
                ViewBag.FMessage = FMessage;
                ViewBag.CurrentSort = true;
                failure = false;
                ViewBag.Postions = new SelectList(db.Positions, "PositionID", "PositionName");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Ошибка чтения из базы данных");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(string SearchTerm)
        {
            try
            {
                ViewBag.Players = db.PlayerInfo.Where(x => x.FirstName.StartsWith(SearchTerm) || x.MiddleName.StartsWith(SearchTerm) || x.LastName.StartsWith(SearchTerm) || x.PlayerNumber.ToString().StartsWith(SearchTerm) || x.ShirtName.StartsWith(SearchTerm)).ToList();
                ViewBag.Nationalities = db.Nationalities.ToList();
                ViewBag.Mans = db.Mans.ToList();
                ViewBag.Failure = failure;
                ViewBag.FMessage = FMessage;
                ViewBag.CurrentSort = true;
                failure = false;
                ViewBag.Postions = new SelectList(db.Positions, "PositionID", "PositionName");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
                return new HttpStatusCodeResult(HttpStatusCode.Conflict, "Ошибка чтения из базы данных");
            }
            return View();
        }
        static bool sortValue = false;
        public PartialViewResult All()
        {
            try
            {
                ViewBag.Players = db.PlayerInfo;
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
            }
            return PartialView("PlayersTab", ViewBag);
        }
        public PartialViewResult SortByNumber()
        {
            try
            {
                if (sortValue)
                {
                    ViewBag.Players = db.PlayerInfo.OrderBy(x => x.PlayerNumber).ToList();
                }
                else
                {
                    ViewBag.Players = db.PlayerInfo.OrderByDescending(x => x.PlayerNumber).ToList();
                }
                sortValue = !sortValue;
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
            }
            return PartialView("PlayersTab", ViewBag);
        }
        public PartialViewResult SortByFIO()
        {
            try
            {
                if (sortValue)
                {
                    ViewBag.Players = db.PlayerInfo.OrderBy(x => x.MiddleName).ToList();
                }
                else
                {
                    ViewBag.Players = db.PlayerInfo.OrderByDescending(x => x.MiddleName).ToList();
                }
                sortValue = !sortValue;
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
            }
            return PartialView("PlayersTab", ViewBag);
        }
        public PartialViewResult SortByNationality()
        {
            try
            {
                if (sortValue)
                {
                    ViewBag.Players = db.PlayerInfo.OrderBy(x => x.NationalityID).ToList();
                }
                else
                {
                    ViewBag.Players = db.PlayerInfo.OrderByDescending(x => x.NationalityID).ToList();
                }
                sortValue = !sortValue;
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
            }
            return PartialView("PlayersTab", ViewBag);
        }
        public PartialViewResult SortByAge()
        {
            try
            {
                if (sortValue)
                {
                    ViewBag.Players = db.PlayerInfo.OrderBy(x => x.Age).ToList();
                }
                else
                {
                    ViewBag.Players = db.PlayerInfo.OrderByDescending(x => x.Age).ToList();
                }
                sortValue = !sortValue;
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
            }
            return PartialView("PlayersTab", ViewBag);
        }
        public PartialViewResult SortByWeight()
        {
            try
            {
                if (sortValue)
                {
                    ViewBag.Players = db.PlayerInfo.OrderBy(x => x.Weight).ToList();
                }
                else
                {
                    ViewBag.Players = db.PlayerInfo.OrderByDescending(x => x.Weight).ToList();
                }
                sortValue = !sortValue;
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
            }
            return PartialView("PlayersTab", ViewBag);
        }
        public PartialViewResult SortByHeight()
        {
            try
            {
                if (sortValue)
                {
                    ViewBag.Players = db.PlayerInfo.OrderBy(x => x.Height).ToList();
                }
                else
                {
                    ViewBag.Players = db.PlayerInfo.OrderByDescending(x => x.Height).ToList();
                }
                sortValue = !sortValue;
                ViewBag.Nationalities = db.Nationalities.ToList();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка: ", ex.Message);
            }
            return PartialView("PlayersTab", ViewBag);
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

        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Players players = db.Players.Find(id);
            try
            {
                players.Mans.IsDeleted = true;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Ошибка при удалении игрока", ex.Message);
            }
            Logger.WriteLog("Игрок " + players.Mans.MiddleName + " удален");
            return View(players);
        }
        //// POST: Players/Delete/5
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Players players = db.Players.Find(id);
        //    db.Players.Remove(players);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
