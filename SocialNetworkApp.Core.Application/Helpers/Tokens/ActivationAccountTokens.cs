using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Helpers.Tokens
{
    public class ActivationAccountTokens
    {
        public static string GetToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
