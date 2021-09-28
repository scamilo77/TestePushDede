using System;
using System.IO;

namespace Logging
{
    #region Dependency Injection

    public interface ILogger
    {
        void Log(string text);
    }
    public class Logger : ILogger
    {
        public void Log(string text)
        {
            using (var sw = new StreamWriter("Log.txt", true))
            {
                sw.WriteLine(text);
            }
        }
    }

    #endregion

    #region Static

    //public class Logger
    //{
    //    public static void Log(string text)
    //    {
    //        using (var sw = new StreamWriter("Log.txt", true))
    //        {
    //            sw.WriteLine(text);
    //        }
    //    }
    //}

    #endregion
}
