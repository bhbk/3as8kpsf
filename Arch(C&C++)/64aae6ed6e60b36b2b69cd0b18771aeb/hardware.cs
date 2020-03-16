using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Management;

namespace Bhbk.Lib.Msft.Win.Sys.WMI
{
    public class Hardware
    {
        public static String GetDDNS(String ip)
        {
            String rslt = String.Empty;
            ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

            foreach (ManagementObject mo in moc.Get())
            {
                try
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        String[] addresses = (String[])mo["IPAddress"];

                        foreach (String address in addresses)
                        {
                            if (address.Equals(ip))
                            {
                                rslt += ((bool)mo["FullDNSRegistrationEnabled"]).ToString();
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    rslt = ex.Message;
                }
            }

            return rslt;
        }
        public static String GetDHCP(String ip)
        {
            String rslt = String.Empty;
            ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

            foreach (ManagementObject mo in moc.Get())
            {
                try
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        String[] addresses = (String[])mo["IPAddress"];

                        foreach (String address in addresses)
                        {
                            if (address.Equals(ip))
                            {
                                rslt += ((bool)mo["DHCPEnabled"]).ToString();
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    rslt += ex.Message;
                }
            }

            return rslt;
        }
        public static String GetDNS(String ip)
        {
            String result = String.Empty;

            try
            {
                ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
                ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);
                String[] dnses = null;

                foreach (ManagementObject mo in moc.Get())
                {
                    /* Make sure this is a IP enabled device. Not something like memory card or VM Ware. */
                    if ((bool)mo["ipEnabled"] && !mo["MACAddress"].ToString().Equals(String.Empty))
                    {
                        dnses = (String[])mo["DNSServerSearchOrder"];
                    }
                }

                if (dnses != null)
                {
                    foreach (String dns in dnses)
                    {
                        result += dns + " ";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return result;
        }
        public static String GetMAC(String ip)
        {
            String rslt = String.Empty;
            ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

            foreach (ManagementObject mo in moc.Get())
            {
                try
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        String[] addresses = (String[])mo["IPAddress"];

                        foreach (String address in addresses)
                        {
                            if (address.Equals(ip))
                            {
                                rslt += mo["MACAddress"].ToString().ToLower().Replace(":", "-");

                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    rslt += ex.Message;
                }
            }
            return rslt;
        }
        public static String GetNICs(String ip)
        {
            String rslt = String.Empty;
            ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

            foreach (ManagementObject mo in moc.Get())
            {
                try
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        rslt += "[DDNS=" + (bool)mo["FullDNSRegistrationEnabled"]
                            + " DHCP=" + (bool)mo["DHCPEnabled"]
                            + " \"" + mo["Description"]
                            + "\" " + mo["MACAddress"].ToString().ToLower().Replace(":", "-");

                        String[] addresses = (String[])mo["IPAddress"];

                        foreach (String address in addresses)
                        {
                            rslt += "," + address;
                        }

                        rslt += "] ";
                    }
                }
                catch (Exception ex)
                {
                    rslt += ex.Message;
                }
            }

            return rslt;
        }
        public static String GetNetBIOS(String ip)
        {
            String result = String.Empty;

            try
            {
                ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
                ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

                foreach (ManagementObject mo in moc.Get())
                {
                    result += mo["Caption"].ToString().ToLower();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return result;
        }
        public static String SetDHCPIP(String ip)
        {

            object[,] o = new object[3, 3];


            StreamWriter sw = new StreamWriter("\\\\" + ip + "\\c$\\windows\\temp\\dhcp.vbs");
            String cmd = "strComputer = \".\"" + Environment.NewLine
                + "Set objWMIService = GetObject(\"winmgmts:\\\\\" & strComputer & \"\\root\\cimv2\")" + Environment.NewLine
                + "Set colNetAdapters = objWMIService.ExecQuery _" + Environment.NewLine
                + "  (\"Select * from Win32_NetworkAdapterConfiguration where IPEnabled = True\")" + Environment.NewLine
                + "For Each objNetAdapter In colNetAdapters" + Environment.NewLine
                + "  For Each objNetAdapterAddress in objNetAdapter.IPAddress" + Environment.NewLine
                + "    If objNetAdapterAddress = \"" + ip + "\" Then" + Environment.NewLine
                + "      errEnable = objNetAdapter.EnableDHCP()" + Environment.NewLine
                + "    End If" + Environment.NewLine
                + "  Next" + Environment.NewLine
                + "Next" + Environment.NewLine
                + "Set WshShell = WScript.CreateObject(\"WScript.Shell\")" + Environment.NewLine
                + "WshShell.Run \"%windir%\\system32\\ipconfig.exe /flushdns\"" + Environment.NewLine
                + "WshShell.Run \"%windir%\\system32\\ipconfig.exe /registerdns\"" + Environment.NewLine;

            //                + Environment.NewLine
            //              + "Set OpSysSet = GetObject(\"winmgmts:{(Shutdown)}//./root/cimv2\").ExecQuery(\"select * from Win32_OperatingSystem where Primary=true\")" + Environment.NewLine
            //            + "  for each OpSys in OpSysSet" + Environment.NewLine
            //          + "    OpSys.Reboot()" + Environment.NewLine
            //        + "next";

            sw.WriteLine(cmd);
            sw.Close();

            String rslt = String.Empty;
            ConnectionOptions connOptions = new ConnectionOptions();
            connOptions.Impersonation = ImpersonationLevel.Impersonate;
            connOptions.EnablePrivileges = true;

            ManagementScope manScope = new ManagementScope("\\\\" + ip + "\\root\\cimv2", connOptions);
            manScope.Connect();

            ObjectGetOptions objectGetOptions = new ObjectGetOptions();
            ManagementPath managementPath = new ManagementPath("Win32_Process");
            ManagementClass processClass = new ManagementClass(manScope, managementPath, objectGetOptions);

            ManagementBaseObject inParams = processClass.GetMethodParameters("Create");

            inParams["CommandLine"] = "cscript.exe c:\\windows\\temp\\dhcp.vbs";
            ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);

            rslt = "[DHCPIP enabled]";

            return rslt;
        }
        public static String SetDHCPDNS(String ip)
        {
            try
            {
                File.Delete("\\\\" + ip + "\\c$\\windows\\temp\\dhcp.vbs");
            }
            catch (Exception ex)
            {
            }

            String rslt = String.Empty;
            ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

            foreach (ManagementObject mo in moc.Get())
            {
                /* Make sure this is a IP enabled device. Not something like memory card or VM Ware. */
                if ((bool)mo["IPEnabled"] && (bool)mo["DHCPEnabled"])
                {
                    try
                    {
                        rslt += "[DHCPDNS enabled \"";

                        ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                        newDNS["DNSServerSearchOrder"] = null;
                        ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                        rslt += mo["Description"].ToString() + "\"]";
                    }
                    catch (Exception ex)
                    {
                        rslt += ex.Message;
                    }
                }
            }

            return rslt;
        }
        public static String SetDDNS(String ip, Boolean value)
        {
            String rslt = String.Empty;
            ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
            ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

            foreach (ManagementObject mo in moc.Get())
            {
                try
                {
                    if ((bool)mo["IPEnabled"])
                    {
                        String[] addresses = (String[])mo["IPAddress"];

                        foreach (String address in addresses)
                        {
                            if (address.Equals(ip))
                            {
                                rslt += "[DDNS enabled \"";

                                ManagementBaseObject objNewIP = null;
                                ManagementBaseObject objSetIP = null;
                                objNewIP = mo.GetMethodParameters("SetDynamicDNSRegistration");
                                objNewIP["FullDNSRegistrationEnabled"] = value;
                                objNewIP["DomainDNSRegistrationEnabled"] = value;
                                objSetIP = mo.InvokeMethod("SetDynamicDNSRegistration", objNewIP, null);

                                rslt += mo["Description"].ToString() + "\"]";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    rslt += ex.Message;
                }
            }

            return rslt;
        }
        public static String SetDDNSRegister(String ip)
        {
            String rslt = String.Empty;
            ConnectionOptions connOptions = new ConnectionOptions();
            connOptions.Impersonation = ImpersonationLevel.Impersonate;
            connOptions.EnablePrivileges = true;

            ManagementScope manScope = new ManagementScope("\\\\" + ip + "\\root\\cimv2", connOptions);
            manScope.Connect();

            ObjectGetOptions objectGetOptions = new ObjectGetOptions();
            ManagementPath managementPath = new ManagementPath("Win32_Process");
            ManagementClass processClass = new ManagementClass(manScope, managementPath, objectGetOptions);

            ManagementBaseObject inParams = processClass.GetMethodParameters("Create");

            inParams["CommandLine"] = "ipconfig /registerdns";
            ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null);

            rslt = "[DDNS registration forced]";

            return rslt;
        }
        public static String SetRestartOS(String ip)
        {
            String rslt = String.Empty;
            ManagementScope oMs = new ManagementScope("\\\\" + ip + "\\root\\cimv2");
            ObjectQuery oQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher moc = new ManagementObjectSearcher(oMs, oQuery);

            foreach (ManagementObject mo in moc.Get())
            {
                try
                {
                    string[] ss = { "" };
                    mo.InvokeMethod("Reboot", ss);

                    rslt += "[RESTART sent...]";

                    break;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            return rslt;
        }
        /* Enable DHCP on the NIC. */
        public static void SetDHCP(String NicDesc)
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    /* Make sure this is a IP enabled device. Not something like memory card or VM Ware. */
                    if ((bool)mo["IPEnabled"])
                    {
                        if (mo["Caption"].ToString().EndsWith(NicDesc))
                        {
                            ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");
                            newDNS["DNSServerSearchOrder"] = null;
                            ManagementBaseObject enableDHCP = mo.InvokeMethod("EnableDHCP", null, null);
                            ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        /* Set IP for the specified network card name. */
        public static void SetIP(String NicDesc, String IpAddresses, String SubnetMask, String Gateway, String DnsSearchOrder)
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    /* Make sure this is a IP enabled device. Not something like memory card or VM Ware. */
                    if ((bool)mo["IPEnabled"])
                    {
                        if (mo["Caption"].ToString().EndsWith(NicDesc))
                        {

                            ManagementBaseObject newIP = mo.GetMethodParameters("EnableStatic");
                            ManagementBaseObject newGate = mo.GetMethodParameters("SetGateways");
                            ManagementBaseObject newDNS = mo.GetMethodParameters("SetDNSServerSearchOrder");

                            newGate["DefaultIPGateway"] = new String[] { Gateway };
                            newGate["GatewayCostMetric"] = new int[] { 1 };

                            newIP["IPAddress"] = IpAddresses.Split(',');
                            newIP["SubnetMask"] = new String[] { SubnetMask };

                            newDNS["DNSServerSearchOrder"] = DnsSearchOrder.Split(',');

                            ManagementBaseObject setIP = mo.InvokeMethod("EnableStatic", newIP, null);
                            ManagementBaseObject setGateways = mo.InvokeMethod("SetGateways", newGate, null);
                            ManagementBaseObject setDNS = mo.InvokeMethod("SetDNSServerSearchOrder", newDNS, null);

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        /* Returns the network card configuration of the specified NIC. */
        public static void GetIP(String NicDesc, out String[] ipAdresses, out String[] subnets, out String[] gateways, out String[] dnses)
        {
            ipAdresses = null;
            subnets = null;
            gateways = null;
            dnses = null;

            try
            {
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    /* Make sure this is a IP enabled device. Not something like memory card or VM Ware. */
                    if ((bool)mo["IpEnabled"])
                    {
                        if (mo["Caption"].ToString().EndsWith(NicDesc))
                        {
                            ipAdresses = (String[])mo["IPAddress"];
                            subnets = (String[])mo["IPSubnet"];
                            gateways = (String[])mo["DefaultIPGateway"];
                            dnses = (String[])mo["DNSServerSearchOrder"];

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
