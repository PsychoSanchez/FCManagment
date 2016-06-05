#define LOG
using System;
using System.IO;
using System.Threading;

namespace MVCApp
{
    public class Logger
    {
        public static AutoResetEvent EventSemaphore = new AutoResetEvent(true);
        /// <summary>
        /// Путь у папке логов с разметкой текущей даты
        /// </summary>
        #region StaticWriteLogMethods
        /// <summary>
        /// Событие и дополнительная информация
        /// </summary>
        /// <param name="_event"></param>
        /// <param name="addInfo"></param>
        public static string WriteLog(string _event, string addInfo)
        {
#if LOG
            EventSemaphore.WaitOne();
            try
            {
                StreamWriter log = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DBLog.log", true);
                DateTime date = DateTime.Now;
                log.WriteLine(date);
                log.WriteLine(_event);
                log.WriteLine(addInfo);
                log.WriteLine();
                log.Close();
            }
            catch (Exception)
            {
                EventSemaphore.Set();
                return _event;
            }
            EventSemaphore.Set();
            return _event;
#endif
        }
        /// <summary>
        /// Только событие без дополнительной информации
        /// </summary>
        /// <param name="_event"></param>
        public static string WriteLog(string _event)
        {
#if LOG
            EventSemaphore.WaitOne();
            try
            {
                StreamWriter log = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\DBLog.log", true);
                DateTime date = DateTime.Now;
                log.WriteLine(date);
                log.WriteLine(_event);
                log.WriteLine();
                log.Close();
            }
            catch (Exception)
            {
                EventSemaphore.Set();
                return _event;
            }
            EventSemaphore.Set();
            return _event;
#endif
        }
        #endregion
    }
}
