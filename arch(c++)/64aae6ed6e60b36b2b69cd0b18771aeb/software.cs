using System;
using System.Collections;
using System.Diagnostics;
using System.Management;
using System.Text;

namespace Bhbk.Lib.Msft.Win.Sys.WMI
{
    public class Software
    {
        public static String GetInstalledSoftware(String computer)
        {
            String apps = String.Empty;

            try
            {
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\" + computer + "\\root\\cimv2");

                //get Fixed disk stats 
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_Product");

                //Execute the query  
                ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

                //Get the results
                foreach (ManagementObject mo in moc.Get())
                {
                    apps += mo["Caption"].ToString() + ",";
                }

                return apps;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
