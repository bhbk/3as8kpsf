using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Management;

namespace Bhbk.Lib.Msft.Win.Sys.WMI
{
    public static class domain
    {
        private const int NETSETUP_UNJOIN_DOMAIN = 0;
        private const int NETSETUP_JOIN_DOMAIN = 1;
        private const int NETSETUP_ACCT_CREATE = 2;
        private const int NETSETUP_ACCT_DELETE = 4;
        private const int NETSETUP_WIN9X_UPGRADE = 16;
        private const int NETSETUP_DOMAIN_JOIN_IF_JOINED = 32;
        private const int NETSETUP_JOIN_UNSECURE = 64;
        private const int NETSETUP_MACHINE_PWD_PASSED = 128;
        private const int NETSETUP_DEFER_SPN_SET = 256;
        private const int NETSETUP_JOIN_WITH_NEW_NAME = 1024;
        private const int NETSETUP_JOIN_READONLY = 2048;
        private const int NETSETUP_ERROR_5 = 5;
        private const int NETSETUP_ERROR_87 = 87;
        private const int NETSETUP_ERROR_110 = 110;
        private const int NETSETUP_ERROR_1323 = 1323;
        private const int NETSETUP_ERROR_1326 = 1326;
        private const int NETSETUP_ERROR_1355 = 1355;
        private const int NETSETUP_ERROR_2224 = 2224;
        private const int NETSETUP_ERROR_2691 = 2691;
        private const int NETSETUP_ERROR_2692 = 2692;
        private const String NETSETUP_ERROR_5_MSG = "Access is denied.";
        private const String NETSETUP_ERROR_87_MSG = "The parameter is incorrect.";
        private const String NETSETUP_ERROR_110_MSG = "The system cannot open the specified object.";
        private const String NETSETUP_ERROR_1323_MSG = "Unable to update the password.";
        private const String NETSETUP_ERROR_1326_MSG = "Logon failure: unknown username or bad password.";
        private const String NETSETUP_ERROR_1355_MSG = "The specified domain either does not exist or could not be contacted.";
        private const String NETSETUP_ERROR_2224_MSG = "The account already exists.";
        private const String NETSETUP_ERROR_2691_MSG = "The machine is already joined to the domain.";
        private const String NETSETUP_ERROR_2692_MSG = "The machine is not currently joined to a domain.";

        public static Boolean DomainAdd(String domain, String ou, String user, String pass)
        {
            bool rtrn = false;
            int rslt = 0;
            ManagementBaseObject query = null, join = null;
            ManagementObject mo = new ManagementObject("Win32_ComputerSystem.Name='" + WMI.Computer.Name() + "'");
            mo.Scope.Options.EnablePrivileges = true;
            mo.Scope.Options.Authentication = AuthenticationLevel.PacketPrivacy;
            mo.Scope.Options.Impersonation = ImpersonationLevel.Impersonate;

            try
            {
                query = mo.GetMethodParameters("JoinDomainOrWorkgroup");
                query["Name"] = domain;
                query["UserName"] = user;
                query["Password"] = pass;
                //                query["AccountOU"] = ou;
                //                query["FJoinOptions"] = NETSETUP_JOIN_DOMAIN + NETSETUP_ACCT_CREATE;
                query["FJoinOptions"] = NETSETUP_JOIN_DOMAIN;

                join = mo.InvokeMethod("JoinDomainOrWorkgroup", query, null);
                rslt = Int32.Parse(join["ReturnValue"].ToString());

                switch (rslt)
                {
                    case 0:
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " executed successfully.");
                        rtrn = true;
                        break;
                    default:
                        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. ERROR:" + rslt);
                        Console.WriteLine("http://www.google.com/search?q=wmi+joindomainorworkgroup+error+" + rslt);
                        rtrn = false;
                        break;
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. INVALIDOPERATIONEXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "INVALIDOPERATIONEXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
                rtrn = false;
            }
            catch (ManagementException ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. MANAGEMENTEXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "MANAGEMENTEXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
                rtrn = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. EXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "STACKTRACE: " + ex.StackTrace);
                rtrn = false;
            }
            return rtrn;
        }
        public static Boolean DomainAddRename(String domain, String ou, String name, String user, String pass)
        {
            int rslt = 0;
            ManagementBaseObject query = null, join = null, rename = null;
            ManagementObject mo = new ManagementObject("Win32_ComputerSystem.Name='" + WMI.Computer.Name() + "'");
            mo.Scope.Options.EnablePrivileges = true;
            mo.Scope.Options.Authentication = AuthenticationLevel.PacketPrivacy;
            mo.Scope.Options.Impersonation = ImpersonationLevel.Impersonate;

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
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " RENAME failed. ERROR:" + rslt);
                    Console.WriteLine("http://www.google.com/search?q=wmi+rename+error+" + rslt);
                    return false;
                }
                else
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " RENAME executed successfully.");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " RENAME failed. INVALIDOPERATIONEXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "RENAME INVALIDOPERATIONEXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "RENAME STACKTRACE: " + ex.StackTrace);
                return false;
            }
            catch (ManagementException ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " RENAME failed. MANAGEMENTEXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "RENAME MANAGEMENTEXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "RENAME STACKTRACE: " + ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " RENAME failed. EXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "RENAME EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "RENAME STACKTRACE: " + ex.StackTrace);
                return false;
            }

            try
            {
                query = mo.GetMethodParameters("JoinDomainOrWorkgroup");
                query["Name"] = domain;
                query["Password"] = pass;
                query["UserName"] = user;
                //                    query["AccountOU"] = ou;
                query["FJoinOptions"] = NETSETUP_JOIN_DOMAIN + NETSETUP_JOIN_WITH_NEW_NAME + NETSETUP_ACCT_CREATE;

                join = mo.InvokeMethod("JoinDomainOrWorkgroup", query, null);
                rslt = Int32.Parse(join["ReturnValue"].ToString());

                if (!rslt.Equals(0))
                {
                    Console.Write(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " JOIN failed. ERROR:" + rslt);
                    Console.WriteLine("http://www.google.com/search?q=wmi+joindomainorworkgroup+error+" + rslt);
                    return false;
                }
                else
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " JOIN executed successfully.");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " JOIN failed. INVALIDOPERATIONEXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "JOIN INVALIDOPERATIONEXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "JOIN STACKTRACE: " + ex.StackTrace);
                return false;
            }
            catch (ManagementException ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " JOIN failed. MANAGEMENTEXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "JOIN MANAGEMENTEXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "JOIN STACKTRACE: " + ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " JOIN failed. EXCEPTION:" + rslt);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "JOIN EXCEPTION: " + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "JOIN STACKTRACE: " + ex.StackTrace);
                return false;
            }

            return true;
        }
        public static Boolean DomainRemove(String user, String pass)
        {
            int rslt = 0;
            ManagementBaseObject query = null, join = null;
            ManagementObject mo = new ManagementObject("Win32_ComputerSystem.Name='" + WMI.Computer.Name() + "'");

            try
            {
                query = mo.GetMethodParameters("UnJoinDomainOrWorkgroup");
                query["UserName"] = user;
                query["Password"] = pass;
                query["FUnJoinOptions"] = NETSETUP_UNJOIN_DOMAIN + NETSETUP_ACCT_DELETE;

                join = mo.InvokeMethod("UnJoinDomainOrWorkgroup", query, null);
                rslt = Int32.Parse(join["ReturnValue"].ToString());

                if (!rslt.Equals(0))
                {
                    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().ToString() + " failed. ERROR:" + rslt);
                    Console.WriteLine("http://www.google.com/search?q=wmi+unjoindomainorworkgroup+error+" + rslt);
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
    }
}
