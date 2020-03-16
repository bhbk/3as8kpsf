using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Management;

namespace Bhbk.Lib.Msft.Win.Sys.WMI
{
    public class Service
    {
        public static Boolean EnableAllowInteractWithDesktop(String service)
        {
            // http://www.codeproject.com/KB/system/ScreenMonitor.aspx?display=Print
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Service");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    // Make sure we find the correct service to adjust the setting on.
                    if ((bool)mo["Caption"].Equals(service))
                    {
                        ManagementBaseObject desktopInteract = mo.GetMethodParameters("Change");

                        desktopInteract["DesktopInteract"] = true;
                        mo.InvokeMethod("Change", desktopInteract, null);

                        // We found the service, set it, no need to continue iteration.
                        break;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);

                return false;
            }
        }
        public static String GetStartupType(String name)
        {
            if (name != null)
            {
                //construct the management path
                String path = "Win32_Service.Name='" + name + "'";
                ManagementPath p = new ManagementPath(path);

                //construct the management object
                ManagementObject ManagementObj = new ManagementObject(p);

                String rslt = ManagementObj["StartMode"].ToString();

                if (rslt.Equals("Auto"))
                {
                    rslt = "Automatic";
                }

                return rslt;
            }
            else
            {
                return String.Empty;
            }
        }
        public static Boolean SetStartupType(String name, String type)
        {
            if (type.Equals("Automatic") || type.Equals("Manual") || type.Equals("Disabled"))
            {
                //construct the management path
                string path = "Win32_Service.Name='" + name + "'";
                ManagementPath p = new ManagementPath(path);

                //construct the management object
                ManagementObject ManagementObj = new ManagementObject(p);

                //we will use the invokeMethod method of the ManagementObject class
                object[] parameters = new object[1];

                parameters[0] = type;
                ManagementObj.InvokeMethod("ChangeStartMode", parameters);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
