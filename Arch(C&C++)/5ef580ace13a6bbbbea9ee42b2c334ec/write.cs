using Microsoft.Win32;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace Bhbk.Lib.Msft.Win.Sys.Registry
{
    // http://books.google.com/books?id=81OLVXxb-qcC&pg=PA1058&lpg=PA1058&dq=c%23+opensubkey+create+key&source=bl&ots=XH4zHscVRf&sig=So-DLzsmAq5F_UGjk3ad5NWzJGs&hl=en&ei=FjBJSuSREoyqswOt98go&sa=X&oi=book_result&ct=result&resnum=1
    public class Writer
    {
        private static RegistryKey keys;
        private static readonly RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
        private static readonly RegistryKey currentUser = Microsoft.Win32.Registry.Users;

        public static String SetHDS(String pc, String val, String str)
        {
            String rslt = String.Empty;
            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                RegistryKey key = keys.OpenSubKey(@"SOFTWARE\HDS", true);

                key = keys.CreateSubKey(@"SOFTWARE\HDS\GM");

                key.SetValue(val, str, RegistryValueKind.String);
                key.Close();

                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static String SetHDSCleanup(String pc)
        {
            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, pc);

                RegistryKey key = keys.OpenSubKey(@"SOFTWARE\HDS", true);

                String[] subs = key.GetSubKeyNames();

                foreach (String s in subs)
                {
                    if (s.ToLower().Equals("gm") || s.ToLower().Equals("wolverine"))
                    {
                        key.DeleteSubKeyTree(s);
                    }
                }

                key.Close();

                return String.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static Int32 SetPowerPoint8(String pc, String val, Int32 str)
        {
            String rslt = String.Empty;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.ClassesRoot, pc);

                RegistryKey key = keys.OpenSubKey(@"PowerPoint.Show.8", true);

                key.SetValue(val, str, RegistryValueKind.DWord);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            return str;
        }
        public static Int32 SetPowerPoint12(String pc, String val, Int32 str)
        {
            String rslt = String.Empty;

            try
            {
                keys = RegistryKey.OpenRemoteBaseKey(RegistryHive.ClassesRoot, pc);

                RegistryKey key = keys.OpenSubKey(@"PowerPoint.Show.12", true);

                key.SetValue(val, str, RegistryValueKind.DWord);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            return str;
        }
        /* Set a service startup state. */
        public static void SetTerminalService(Boolean value)
        {
            try
            {
                // Reverse boolean value because of the way it's represented in the registry.
                value = !value;

                // Open the key where IE store's its proxy setting.
                RegistryKey key = localMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server");

                int setValue = (value ? 1 : 0);
                key.SetValue("fDenyTSConnections", setValue);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        /* Get IE proxy settings for given user SID out of registry. */
        public static void SetIEProxy(String sid, Boolean value)
        {
            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey key = currentUser.CreateSubKey(sid + @"\Software\Microsoft\Windows\CurrentVersion\Internet Settings");

                int setValue = (value ? 1 : 0);
                key.SetValue("ProxyEnable", setValue);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        /* Get IE proxy settings for given user SID out of registry. */
        public static void SetIEProxyServer(String sid, String value)
        {
            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey key = currentUser.CreateSubKey(sid + @"\Software\Microsoft\Windows\CurrentVersion\Internet Settings");

                key.SetValue("ProxyServer", value);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        /* Get IE proxy settings for given user SID out of registry. */
        public static void SetIEProxyOverrideAddresses(String sid, String addresses)
        {
            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey key = currentUser.CreateSubKey(sid + @"\Software\Microsoft\Windows\CurrentVersion\Internet Settings");

                if (!addresses.Contains("<local>"))
                {
                    addresses += ";<local>";
                }

                key.SetValue("ProxyOverride", addresses);
                key.Close();
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
