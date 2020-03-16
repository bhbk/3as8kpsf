using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Management;

namespace Bhbk.Lib.Msft.Win.Sys.WMI
{
    public static class network
    {
        public static string MAC()
        {
            String address = String.Empty;
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        address = mo["MacAddress"].ToString();
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString());
                        Console.WriteLine(mo["MacAddress"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString());
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACK TRACE: " + ex.StackTrace);
            }

            return address;
        }
    }
}
