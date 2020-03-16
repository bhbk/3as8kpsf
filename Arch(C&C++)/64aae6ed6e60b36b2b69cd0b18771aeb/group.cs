using System;
using System.Collections;
using System.DirectoryServices;
using System.Text;
using System.Security;
using System.Security.Principal;

namespace Bhbk.Lib.Msft.Win.Sys.WMI
{
    public class Group
    {
        public static ArrayList GetLocalGroupUsers(String groupName)
        {
            ArrayList accounts = new ArrayList();

            /* http://www.codeproject.com/csharp/getusersid.asp */

            /*
                UserFlags
                MaxStorage
                PasswordAge
                PasswordExpired
                LoginHours
                FullName 
                Description
                BadPasswordAttempts
                HomeDirectory 
                LoginScript 
                Profile 
                HomeDirDrive 
                Parameters 
                PrimaryGroupID
                Name 
                MinPasswordLength
                MaxPasswordAge
                MinPasswordAge
                PasswordHistoryLength
                AutoUnlockInterval
                LockoutObservationInterval
                MaxBadPasswordsAllowed
                RasPermissions
                objectSid
             */

            try
            {
                DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
                DirectoryEntry searchGroup = localMachine.Children.Find(groupName, "group");
                object members = searchGroup.Invoke("members", null);

                foreach (object groupMember in (IEnumerable)members)
                {
                    DirectoryEntry member = new DirectoryEntry(groupMember);
                    String username = String.Empty;
/*
                    if (!member.Properties["FullName"].Value.ToString().Equals(String.Empty))
                    {
                        username = member.Properties["FullName"].Value.ToString();
                    }
                    else
                    {
                        username = member.Properties["Name"].Value.ToString();
                    }
*/
                    username = member.Properties["Name"].Value.ToString();

                    accounts.Add(username);
                }
            }
            catch (Exception ex)
            {
                Bhbk.Lib.Msft.Win.Sys.Log.application.write(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.MethodBase.GetCurrentMethod().ToString(), ex);
            }

            return accounts;
        }
    }
}
