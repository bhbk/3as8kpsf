using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Text;
namespace Bhbk.Lib.Msft.Win.Sys.Registry
{
    //http://stackoverflow.com/questions/1541053/adding-registry-key-in-c-shows-when-i-read-it-back-but-not-in-regedit
    //http://msdn.microsoft.com/en-us/library/aa965884%28VS.85%29.aspx
    public static class value
    {
        public static void clear(RegistryKey root, String key, String value, String type)
        {
            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey path = root.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                switch (type)
                {
                    case "binary":
                        path.SetValue(value, 0, RegistryValueKind.Binary);
                        break;
                    case "dword":
                        path.SetValue(value, 0, RegistryValueKind.DWord);
                        break;
                    case "qword":
                        path.SetValue(value, 0, RegistryValueKind.QWord);
                        break;
                    case "expand":
                        path.SetValue(value, String.Empty, RegistryValueKind.ExpandString);
                        break;
                    case "multi":
                        path.SetValue(value, String.Empty, RegistryValueKind.MultiString);
                        break;
                    case "string":
                        path.SetValue(value, String.Empty, RegistryValueKind.String);
                        break;
                    default:
                        break;
                };
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + Environment.NewLine
                    + "HIVE:" + root.ToString() + Environment.NewLine
                    + "KEY:" + key + Environment.NewLine
                    + "VALUE:" + value + Environment.NewLine
                    + "DATA:" + String.Empty + Environment.NewLine
                    + "TYPE:" + type);
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
        public static void get(RegistryKey root, String key, String value)
        {
            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey path = root.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                RegistryValueKind type = path.GetValueKind(value);

                if (type.Equals(RegistryValueKind.Binary)
                    || type.Equals(RegistryValueKind.String)
                    || type.Equals(RegistryValueKind.DWord)
                    || type.Equals(RegistryValueKind.ExpandString)
                    || type.Equals(RegistryValueKind.QWord)
                    || type.Equals(RegistryValueKind.Unknown))
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + Environment.NewLine
                        + "HIVE:" + root.ToString() + Environment.NewLine
                        + "KEY:" + key + Environment.NewLine
                        + "VALUE:" + value + Environment.NewLine
                        + "DATA:" + path.GetValue(value).ToString() + Environment.NewLine
                        + "TYPE:" + type);
                }
                else if (type.Equals(RegistryValueKind.MultiString))
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + Environment.NewLine
                        + "HIVE:" + root.ToString() + Environment.NewLine
                        + "KEY:" + key + Environment.NewLine
                        + "VALUE:" + value + Environment.NewLine
                        + "DATA:" + path.GetValue(value).ToString() + Environment.NewLine
                        + "TYPE:" + type);
                }
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
        public static void set(RegistryKey root, String key, String value, String data, String type)
        {
            try
            {
                /* Open the key where IE store's its proxy setting. */
                RegistryKey path = root.OpenSubKey(key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                switch (type)
                {
                    case "binary":
                        path.SetValue(value, Boolean.Parse(data), RegistryValueKind.Binary);
                        break;
                    case "dword":
                        path.SetValue(value, Int32.Parse(data), RegistryValueKind.DWord);
                        break;
                    case "qword":
                        path.SetValue(value, Int64.Parse(data), RegistryValueKind.QWord);
                        break;
                    case "expand":
                        path.SetValue(value, data, RegistryValueKind.ExpandString);
                        break;
                    case "multi":
                        path.SetValue(value, data, RegistryValueKind.MultiString);
                        break;
                    case "string":
                        path.SetValue(value, data, RegistryValueKind.String);
                        break;
                    case "ciphertext":
                        path.SetValue(value, data, RegistryValueKind.String);
                        break;
                    default:
                        break;
                };
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + Environment.NewLine
                    + "HIVE:" + root.ToString() + Environment.NewLine
                    + "KEY:" + key + Environment.NewLine
                    + "VALUE:" + value + Environment.NewLine
                    + "DATA:" + data + Environment.NewLine
                    + "TYPE:" + type);
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
    }
}
