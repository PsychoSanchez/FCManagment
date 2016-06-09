using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCApp.Controllers
{
    public class AutotizationController : Controller
    {
        FClubEntities db = new FClubEntities();
        public static int user;
        public static int role;
        public static string Username;
        public static bool isAutorized = false;
        public static bool firsttry = true;
        // GET: Autotization
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (db.Users.Where(x => x.e_mail == username && x.Password == password).Count() > 0)
            {
                var CurrUsr = db.Users.Where(x => x.e_mail == username && x.Password == password).FirstOrDefault();
                user = CurrUsr.ManID;
                Username = CurrUsr.Mans.MiddleName;
                role = (int)CurrUsr.Mans.PersonalPositionID;
                isAutorized = true;
                return RedirectToAction("Index", "Mans");
            }
            else
            {
                firsttry = false;
                return View("Login");
            }
        }
        [HttpPost]
        public ActionResult Logoff()
        {
            role = -1;
            user = -1;
            isAutorized = false;
            firsttry = true;
            return RedirectToAction("Index", "Home");
        }
    }
}