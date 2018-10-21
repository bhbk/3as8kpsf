using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Bhbk.Lib.Core.Validators
{
    public class IdentityAudience
    {
        public static bool Multiple(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            var audienceList = new List<string>();

            foreach (string first in audiences)
                foreach (string second in first.Split(','))
                    audienceList.Add(second.Trim());

            foreach (string entry in audienceList)
                if (validationParameters.ValidAudiences.Contains(entry))
                    return true;

            return false;
        }

        public static bool Single(string audience, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            throw new NotImplementedException();
        }
    }
}
