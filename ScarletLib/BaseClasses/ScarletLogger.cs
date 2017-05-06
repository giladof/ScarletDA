using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarletLib.BaseClasses
{
   public static class ScarletLogger
    {
        public static void LogMessage(string msg, string location)
        {

            try
            {
                using (System.IO.StreamWriter writer = System.IO.File.AppendText(location))
                {
                    writer.WriteLine(string.Format("{0:MM/dd/yy H:mm:ss} - {1}",DateTime.Now,msg));
                }

                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
