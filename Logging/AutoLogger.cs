using System;
using System.Collections.Generic;
using System.Text;

namespace GZZLogger.Logging
{
    class AutoLogger
    {
        public static void Log(string name, bool ok, bool load)
        {
            Console.Write(DateTime.Now.ToString("HH:mm:ss >"));
            if (load)
            {
                Console.Write(" Loading ");
            }
            if (ok)
            {
                Console.Write(name + " ...");
            }
            else
            {
                Console.WriteLine(name);
            }
        }
        public static void ok()
        {
            Console.WriteLine(" [OK]");
        }
    }
}
