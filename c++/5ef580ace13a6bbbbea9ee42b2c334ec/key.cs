using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Text;

namespace Bhbk.Lib.Msft.Win.Sys.Registry
{
    public static class key
    {
        public static void add(RegistryKey root, String key, String subkey, Boolean recurse)
        {
            try
            {
                RegistryKey path = null;
                if (recurse)
                {
                    path = root.CreateSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                else
                {
                    path = root.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                }

                path.CreateSubKey(subkey, RegistryKeyPermissionCheck.ReadWriteSubTree);
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + Environment.NewLine
                    + "HIVE:" + root.ToString() + Environment.NewLine
                    + "KEY:" + key + Environment.NewLine
                    + "SUBKEY:" + subkey + Environment.NewLine
                    + "RECURSIVE:" + recurse);
                path.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " "
                    + System.Reflection.MethodBase.GetCurrentMethod().ToString());
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
            }
        }
        public static void remove(RegistryKey root, String key, String subkey, Boolean recurse)
        {
            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey path = root.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + Environment.NewLine
                    + "HIVE:" + root.ToString() + Environment.NewLine
                    + "KEY:" + key + Environment.NewLine
                    + "SUBKEY:" + subkey + Environment.NewLine
                    + "RECURSIVE:" + recurse);
                if (recurse)
                {
                    path.DeleteSubKeyTree(subkey);
                }
                else
                {
                    path.DeleteSubKey(subkey, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " "
                    + System.Reflection.MethodBase.GetCurrentMethod().ToString());
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
            }
        }
    }
}
