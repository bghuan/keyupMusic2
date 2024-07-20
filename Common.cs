using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keyupMusic2
{
    public partial class Huan : Form
    {
        public static void log(string message)
        {
            //log(GetWindowText(GetForegroundWindow()));
            File.AppendAllText("log.txt", "\r" + DateTime.Now.ToString("") + " " + message);
        }
    }
}
