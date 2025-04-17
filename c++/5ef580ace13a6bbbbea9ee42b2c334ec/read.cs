using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace Bhbk.Lib.Msft.Win.Sys.Registry
{
    public class Reader
    {
        private static readonly RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
        private static readonly RegistryKey currentUser = Microsoft.Win32.Registry.Users;

        private static RegistryKey keys;
        private static RegistryKey key;

        public static String GetHDS(String pc, String val)
        {
            String rslt = String.Empty;
            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                key = keys.OpenSubKey(@"SOFTWARE\HDS\GM", false);

                rslt = key.GetValue(val).ToString();
                key.Close();

                return rslt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetHDSSubKeyList(String pc)
        {
            String rslt = String.Empty;
            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                RegistryKey sub = keys.OpenSubKey(@"SOFTWARE\HDS");
                String[] subs = sub.GetSubKeyNames();

                foreach (String s in subs)
                {
                    rslt += s + " ";
                }

                keys.Close();

                return rslt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetPowerPoint8(String pc, String val)
        {
            String rslt = String.Empty;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.ClassesRoot, pc);

                key = keys.OpenSubKey(@"PowerPoint.Show.8", false);

                rslt = key.GetValue(val).ToString();

                if (key.GetValueKind(val).Equals(RegistryValueKind.String))
                {
                    rslt = key.GetValue(val).ToString();
                }
                else if (key.GetValueKind(val).Equals(RegistryValueKind.Binary))
                {
                    rslt = System.Text.ASCIIEncoding.ASCII.GetString((Byte[])key.GetValue(val));
                }

                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            return rslt;
        }
        public static String GetPowerPoint12(String pc, String val)
        {
            String rslt = String.Empty;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.ClassesRoot, pc);

                key = keys.OpenSubKey(@"PowerPoint.Show.12", false);

                rslt = key.GetValue(val).ToString();

                if (key.GetValueKind(val).Equals(RegistryValueKind.String))
                {
                    rslt = key.GetValue(val).ToString();
                }
                else if (key.GetValueKind(val).Equals(RegistryValueKind.Binary))
                {
                    rslt = System.Text.ASCIIEncoding.ASCII.GetString((Byte[])key.GetValue(val));
                }

                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            return rslt;
        }
        public static String GetPowerPointSubKeyList(String pc)
        {
            String rslt = String.Empty;
            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.ClassesRoot, pc);

                RegistryKey sub = keys.OpenSubKey(@"PowerPoint.Show.12");
                String[] subs = sub.GetSubKeyNames();

                foreach (String s in subs)
                {
                    rslt += " \"" + s + "\" ";
                }

                keys.Close();

                return rslt;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetComputerDescription(String pc)
        {
            String desc = String.Empty;
            RegistryKey pcDesc;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                pcDesc = keys.OpenSubKey(@"SYSTEM\ControlSet001\Services\LanmanServer\Parameters", false);

                desc = pcDesc.GetValue("srvcomment").ToString();

                pcDesc.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return desc;
        }
        public static String GetInstalledSoftware(String pc)
        {
            String name = String.Empty;
            String ver = String.Empty;
            String installsrc = String.Empty;
            RegistryKey progPath;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                /* Microsoft Office Outlook 2003 */
                progPath = keys.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);

                foreach (String str in progPath.GetSubKeyNames())
                {
                    RegistryKey key = progPath.OpenSubKey(str);

                    foreach (String val in key.GetValueNames())
                    {
                        if (val.Equals("DisplayName"))
                        {
                            String tmp = (String)key.GetValue("DisplayName") + ",";

                            if (!tmp.Contains("Hotfix")
                                && !tmp.Contains("Update"))
                            {
                                name += key.GetValue("DisplayName") + ",";
                            }
                        }
                    }
                }

                return name;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetSoftwareInfo(String pc, String software)
        {
            String name = String.Empty;
            String ver = String.Empty;
            String installsrc = String.Empty;
            RegistryKey progPath;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                /* Microsoft Office Outlook 2003 */
                progPath = keys.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);

                foreach (String str in progPath.GetSubKeyNames())
                {
                    RegistryKey key = progPath.OpenSubKey(str);

                    foreach (String val in key.GetValueNames())
                    {
                        if (val.Equals("DisplayName"))
                        {
                            String tmp = (String)key.GetValue("DisplayName");
                            String uninstall = (String)key.GetValue("UninstallString");

                            if (tmp.ToLower().Contains(software))
                            {
                                name += key.GetValue("DisplayName") + "," + key.GetValue("UninstallString") + ",";
                            }
                        }
                    }
                }

                return name;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetSoftwareInfo32(String pc, String software)
        {
            String name = String.Empty;
            String ver = String.Empty;
            String installsrc = String.Empty;
            RegistryKey progPath;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                /* Microsoft Office Outlook 2003 */
                progPath = keys.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall", false);

                foreach (String str in progPath.GetSubKeyNames())
                {
                    RegistryKey key = progPath.OpenSubKey(str);

                    foreach (String val in key.GetValueNames())
                    {
                        if (val.Equals("DisplayName"))
                        {
                            String tmp = (String)key.GetValue("DisplayName");
                            String uninstall = (String)key.GetValue("UninstallString");

                            if (tmp.ToLower().Contains(software))
                            {
                                name += key.GetValue("DisplayName") + ",";
                                name += key.GetValue("DisplayName") + "," + key.GetValue("UninstallString") + ",";
                            }
                        }
                    }
                }

                return name;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetOfficeInfo(String pc)
        {
            String name = String.Empty;
            String ver = String.Empty;
            String installsrc = String.Empty;
            RegistryKey progPath;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                /* Microsoft Office XP Standard */
                progPath = keys.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{90120409-6000-11D3-8CFE-0050048383C9}", false);

                if (progPath != null)
                {
                    name = progPath.GetValue("DisplayName").ToString();
                    ver = progPath.GetValue("DisplayVersion").ToString();
                    installsrc = progPath.GetValue("InstallSource").ToString();

                    progPath.Close();

                    return name + "," + ver + "," + installsrc;
                }

                /* Microsoft Office XP Professional with FrontPage */
                progPath = keys.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{90280409-6000-11D3-8CFE-0050048383C9}", false);

                if (progPath != null)
                {
                    name = progPath.GetValue("DisplayName").ToString();
                    ver = progPath.GetValue("DisplayVersion").ToString();
                    installsrc = progPath.GetValue("InstallSource").ToString();

                    progPath.Close();

                    return name + "," + ver + "," + installsrc;
                }

                return "Unknown,Unknown,Unknown";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetOutlookInfo(String pc)
        {
            String name = String.Empty;
            String ver = String.Empty;
            String installsrc = String.Empty;
            RegistryKey progPath;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                /* Microsoft Office Outlook 2003 */
                progPath = keys.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{90E00409-6000-11D3-8CFE-0150048383C9}", false);

                if (progPath != null)
                {
                    name = progPath.GetValue("DisplayName").ToString();
                    ver = progPath.GetValue("DisplayVersion").ToString();
                    installsrc = progPath.GetValue("InstallSource").ToString();

                    progPath.Close();

                    return name + "," + ver + "," + installsrc;
                }

                return "Unknown,Unknown,Unknown";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String GetVisioInfo(String pc)
        {
            String name = String.Empty;
            String ver = String.Empty;
            String installsrc = String.Empty;
            RegistryKey progPath;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                /* Microsoft Office Visio Professional 2007 */
                progPath = keys.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{90120000-0051-0000-0000-0000000FF1CE}", false);

                if (progPath != null)
                {
                    name = progPath.GetValue("DisplayName").ToString();
                    ver = progPath.GetValue("DisplayVersion").ToString();
                    installsrc = progPath.GetValue("InstallSource").ToString();

                    progPath.Close();

                    return name + "," + ver + "," + installsrc;
                }

                return "Unknown,Unknown,Unknown";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /* Get IE proxy settings for given user SID out of registry. */
        public static Boolean GetTerminalService()
        {
            int value = 0;

            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey key = localMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", false);

                value = (int)key.GetValue("fDenyTSConnections", 0);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            return (value > 0);
        }
        /* Get IE proxy settings for given user SID out of registry. */
        public static Boolean GetIEProxy(String sid)
        {
            int value = 0;

            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey key = currentUser.OpenSubKey(sid + @"\Software\Microsoft\Windows\CurrentVersion\Internet Settings", false);

                value = (int)key.GetValue("ProxyEnable", 0);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            return (value > 0);
        }
        /* Get IE proxy settings for given user SID out of registry. */
        public static String GetIEProxyServer(String sid)
        {
            String value = String.Empty;

            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey key = currentUser.OpenSubKey(sid + @"\Software\Microsoft\Windows\CurrentVersion\Internet Settings", false);

                value = (String)key.GetValue("ProxyServer", String.Empty);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            return value;
        }
        /* Get IE proxy settings for given user SID out of registry. */
        public static Boolean GetIEProxyOverride(String sid)
        {
            String value = String.Empty;

            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey key = currentUser.OpenSubKey(sid + @"\Software\Microsoft\Windows\CurrentVersion\Internet Settings", false);

                value = (String)key.GetValue("ProxyOverride", String.Empty);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            /* If bypass proxy set, then it should contain <local>. */
            if (value.IndexOf("<local>") >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }        
    }
}
