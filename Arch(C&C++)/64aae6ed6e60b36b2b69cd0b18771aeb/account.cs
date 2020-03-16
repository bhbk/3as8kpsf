using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Management;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Bhbk.Lib.Msft.Win.Sys.WMI
{
    public class Account
    {
        /* READ THIS CHRIS!!!
         http://www.megasolutions.net/cSharp/Getting-local-admin-groups-and-users-on-a-windows-server-using-ADSI-27079.aspx
         */
        /* Returns the list of *VALID* user accounts on computer. */
        public static ArrayList GetLocalSystemAccounts(String domain)
        {
            ArrayList rslt = new ArrayList();
            try
            {
                /* There are easier ways to get username/SID information, but if the computer is on a
                 * domain, the entire domain user list must be interated through, which could take far
                 * to long. */
                ManagementObjectSearcher moc = new ManagementObjectSearcher("select * from Win32_UserAccount where domain=\'" + domain + "\'");
                foreach (ManagementObject mo in moc.Get())
                {
                    rslt.Add(mo["name"].ToString().ToLower());
                }
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
            return rslt;
        }
        /* Returns the list of *VALID* user accounts on computer. */
        public static ArrayList GetLocalSystemAccountsOld(String domain)
        {
            ArrayList rslt = new ArrayList();
            try
            {
                /* There are easier ways to get username/SID information, but if the computer is on a
                 * domain, the entire domain user list must be interated through, which could take far
                 * to long. */
                ManagementObjectSearcher moc = new ManagementObjectSearcher("select * from Win32_SystemUsers");
                foreach (ManagementObject mo in moc.Get())
                {
                    String dRtrn = String.Empty;
                    String uRtrn = String.Empty;
                    Regex domainMatch = new Regex("Domain=\"[0-9|a-z|A-Z|-|_| ]*\"", RegexOptions.IgnoreCase);
                    Regex userMatch = new Regex("Name=\"[0-9|a-z|A-Z|-|_| ]*\"", RegexOptions.IgnoreCase);
                    if (userMatch.IsMatch(mo["PartComponent"].ToString()))
                    {
                        MatchCollection dMatches = domainMatch.Matches(mo["PartComponent"].ToString());
                        foreach (Match m in dMatches)
                        {
                            dRtrn += m.Value.ToString();
                        }
                        MatchCollection uMatches = userMatch.Matches(mo["PartComponent"].ToString());
                        foreach (Match m in uMatches)
                        {
                            uRtrn += m.Value.ToString();
                        }
                        /* Program execution was giving me invalid array element errors because strings being returned
                         * by WMI were smaller than the index given in the .Substring() calls. */
                        if (dRtrn.Length > 7 && uRtrn.Length > 5)
                        {
                            dRtrn = dRtrn.Substring(7);
                            dRtrn = dRtrn.Trim('\"');
                            uRtrn = uRtrn.Substring(5);
                            uRtrn = uRtrn.Trim('\"');
                            /* Sometimes we want to query for local accounts & other times we want to query for domain accounts. */
                            if (dRtrn.Equals(domain))
                            {
                                rslt.Add(uRtrn);
                            }
                        }
                    }
                }
            }
            catch (ManagementException ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                    System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }
            return rslt;
        }
        public static ArrayList IdentifyCurrentSystemUsers()
        {
            ArrayList currentSystemUsers = new ArrayList();
            ManagementObjectSearcher moc = new ManagementObjectSearcher("select * from Win32_SystemUsers");

            try
            {

                foreach (ManagementObject mo in moc.Get())
                {
                    String dRtrn = String.Empty;
                    String uRtrn = String.Empty;
                    Regex domainMatch = new Regex("Domain=\"[0-9|a-z|A-Z|-|_| ]*\"", RegexOptions.IgnoreCase);
                    Regex userMatch = new Regex("Name=\"[0-9|a-z|A-Z|-|_| ]*\"", RegexOptions.IgnoreCase);

                    if (userMatch.IsMatch(mo["PartComponent"].ToString()))
                    {
                        MatchCollection dMatches = domainMatch.Matches(mo["PartComponent"].ToString());

                        foreach (Match m in dMatches)
                        {
                            dRtrn += m.Value.ToString();
                        }

                        MatchCollection uMatches = userMatch.Matches(mo["PartComponent"].ToString());

                        foreach (Match m in uMatches)
                        {
                            uRtrn += m.Value.ToString();
                        }

                        /* Program execution was giving me invalid array element errors because strings being returned
                         * by WMI were smaller than the index given in the .Substring() calls. */
                        if (dRtrn.Length > 7 && uRtrn.Length > 5)
                        {
                            dRtrn = dRtrn.Substring(7);
                            dRtrn = dRtrn.Trim('\"');
                            uRtrn = uRtrn.Substring(5);
                            uRtrn = uRtrn.Trim('\"');

                            if (IsUserAccountValid(dRtrn + "\\" + uRtrn) && !dRtrn.Equals(Environment.MachineName))
                            {
                                currentSystemUsers.Add(uRtrn);

                                System.Console.WriteLine("PROFILE EXISTS FOR USER: " + uRtrn);
                            }
                        }
                    }
                }
            }
            catch (ManagementException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return currentSystemUsers;
        }
        public static bool IsUserAccountValid(String user)
        {
            bool rtrn = false;

            try
            {
                ManagementObjectSearcher moc = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");

                foreach (ManagementObject mo in moc.Get())
                {
                    /* Match the SID against all entries in collection. */
                    if (mo["Caption"].ToString().Equals(user))
                    {
                        if (mo["Status"].ToString().Equals("Degraded"))
                        {
                            rtrn = false;
                            break;
                        }
                        else
                        {
                            rtrn = true;
                            break;
                        }
                    }
                }
            }
            catch (ManagementException ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return rtrn;
        }

    }
}
