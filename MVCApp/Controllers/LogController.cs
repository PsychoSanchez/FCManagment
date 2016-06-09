using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCApp.Controllers
{
    public class LogController : Controller
    {
        public class Entity
        {
            public Entity (DateTime d, string mes)
            {
                date = d;
                Message = mes;
            }
            public DateTime date;
            public string Message;
        }
        // GET: Log
        public ActionResult Index()
        {
            List<Entity> list = new List<Entity>();
            StreamReader file = new StreamReader(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DBLog.log");
            string Message = string.Empty;
            DateTime TryMessageTime = new DateTime();
            DateTime MessageTime = new DateTime();
            while (file.Peek() > 0)
            {
                string _line = file.ReadLine();
                if (DateTime.TryParse(_line, out TryMessageTime))
                {
                    MessageTime = TryMessageTime;
                    continue;
                }
                else
                {
                    if (_line != string.Empty)
                        Message += _line + "\r\n";
                    else if (Message != string.Empty)
                    {
                        Message += "\r\n";
                    }
                }

                if (Message.EndsWith("\r\n\r\n"))
                {
                    if (!string.IsNullOrEmpty(Message))
                    {
                        list.Add(new Entity(MessageTime, Message));
                        Message = string.Empty;
                    }
                }
            }
            return View(list);
        }
    }
}