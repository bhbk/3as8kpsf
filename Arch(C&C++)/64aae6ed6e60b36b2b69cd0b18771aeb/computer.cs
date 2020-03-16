using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Management;

namespace Bhbk.Lib.Msft.Win.Sys.WMI
{
    public static class Computer
    {
        public static String Name()
        {
            String rslt = String.Empty;
            try
            {
                ManagementObjectSearcher moc = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
                foreach (ManagementObject mo in moc.Get())
                {
                    foreach (PropertyData p in mo.Properties)
                    {
                        if (p.Name.Equals("Name"))
                        {
                            rslt = p.Value.ToString().ToLower();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString());
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
            }
            return rslt;
        }
        public static Boolean Rename(String name, String user, String pass)
        {
            int rslt = 0;
            ManagementBaseObject rename = null, query = null;
            ManagementObject mo = new ManagementObject("Win32_ComputerSystem.Name='" + Name() + "'");

            try
            {
                query = mo.GetMethodParameters("Rename");
                query["Name"] = name;
                query["UserName"] = user;
                query["Password"] = pass;

                rename = mo.InvokeMethod("Rename", query, null);
                rslt = Int32.Parse(rename["ReturnValue"].ToString());

                if (!rslt.Equals(0))
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. ERROR:" + rslt);
                    Console.WriteLine("http://www.google.com/search?q=wmi+rename+error+" + rslt);
                    return false;
                }
                else
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " executed successfully.");
                    return true;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. INVALIDOPERATIONEXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "INVALIDOPERATIONEXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
                return false;
            }
            catch (ManagementException ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. MANAGEMENTEXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "MANAGEMENTEXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. EXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
                return false;
            }
        }
        public static Boolean Shutdown(String level)
        {
            String flags = String.Empty;
            if (level.Equals("down"))
            {
                flags = "1";
            }
            else if (level.Equals("bounce"))
            {
                flags = "2";
            }
            try
            {
                ManagementBaseObject shutdown = null;
                ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
                mc.Get();

                // You can't shutdown without security privileges
                mc.Scope.Options.EnablePrivileges = true;
                ManagementBaseObject mboShutdownParams = mc.GetMethodParameters("Win32Shutdown");

                // Flag 1 means we want to shut down the system. Use "2" to reboot.
                mboShutdownParams["Flags"] = flags;
                mboShutdownParams["Reserved"] = "0";
                foreach (ManagementObject manObj in mc.GetInstances())
                {
                    shutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
                }

                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " executed successfully.");
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString());
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACK TRACE: " + ex.StackTrace);
                return false;
            }
        }
        public static String GetATAPIDrives(String computer)
        {
            String drive = String.Empty;

            try
            {
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\" + computer + "\\root\\cimv2");

                //get Fixed disk stats 
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_CDROMDrive");

                //Execute the query  
                ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

                //Get the results
                foreach (ManagementObject mo in moc.Get())
                {
                    drive += " " + mo["Drive"].ToString() + "[" + mo["Caption"].ToString() + "]";
                }

                return drive;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetHardDrives(String computer)
        {
            String hd = String.Empty;

            try
            {
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\" + computer + "\\root\\cimv2");

                //get Fixed disk stats 
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_LogicalDisk WHERE DriveType=3");

                //Execute the query  
                ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

                //Get the results
                foreach (ManagementObject mo in moc.Get())
                {
                    /* Match the SID against all entries in collection. */
                    if (mo["FileSystem"].ToString().Equals("NTFS"))
                    {
                        Int64 size = Int64.Parse(mo["Size"].ToString()) / 1073741824;
                        Int64 avail = Int64.Parse(mo["FreeSpace"].ToString()) / 1073741824;

                        Int64 used = size - avail;

                        hd += " " + mo["DeviceID"].ToString() + "[" + used + "/" + size + "]GB";
                    }
                }

                return hd;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetSystemMemory(String computer)
        {
            String mem = String.Empty;
            Int64 size = 0;

            try
            {
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\" + computer + "\\root\\cimv2");

                //get Fixed disk stats 
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_PhysicalMemory");

                //Execute the query  
                ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

                //Get the results
                foreach (ManagementObject mo in moc.Get())
                {

                    /* Match the SID against all entries in collection. */
                    if (mo["Caption"].ToString().Equals("Physical Memory"))
                    {
                        size += Int64.Parse(mo["Capacity"].ToString()) / 1048576;

                    }
                }
                return mem += size + "MB";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
        public static String GetOSVersion(String computer)
        {
            String os = String.Empty;

            try
            {
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\" + computer + "\\root\\cimv2");

                //get Fixed disk stats 
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_OperatingSystem");

                //Execute the query  
                ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

                //Get the results
                foreach (ManagementObject mo in moc.Get())
                {
                    os += mo["Caption"].ToString() + " " + mo["Version"].ToString();
                }

                return os;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static bool IsPartOfDomain()
        {
            ManagementObjectSearcher moc = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            bool isOnDomain = false;

            foreach (ManagementObject mo in moc.Get())
            {
                if ((bool)mo["PartOfDomain"] != true)
                {
                    isOnDomain = true;
                }
                else
                {
                    isOnDomain = false;
                }
            }

            return isOnDomain;
        }
        public static String GetComputerName(String computer)
        {
            String name = String.Empty;

            try
            {
                System.Management.ManagementScope oMs = new System.Management.ManagementScope("\\\\" + computer + "\\root\\cimv2");

                //get Fixed disk stats 
                System.Management.ObjectQuery oQuery = new System.Management.ObjectQuery("SELECT * FROM Win32_ComputerSystem");

                //Execute the query  
                ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

                //Get the results
                foreach (ManagementObject mo in moc.Get())
                {
                    name += mo["Caption"].ToString();
                }

                return name;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetMachineName()
        {
            ManagementObjectSearcher moc = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            String machineName = String.Empty;

            foreach (ManagementObject mo in moc.Get())
            {
                foreach (PropertyData p in mo.Properties)
                {
                    if (p.Name.Equals("Name"))
                    {
                        machineName = p.Value.ToString();
                        break;
                    }
                }
            }

            return machineName;
        }
        public static String GetWorkgroupName()
        {
            ManagementObjectSearcher moc = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            String workgroupName = String.Empty;

            foreach (ManagementObject mo in moc.Get())
            {
                foreach (PropertyData p in mo.Properties)
                {
                    if (p.Name.Equals("Domain"))
                    {
                        workgroupName = p.Value.ToString();
                        break;
                    }
                }
            }

            return workgroupName;
        }
    }
}
