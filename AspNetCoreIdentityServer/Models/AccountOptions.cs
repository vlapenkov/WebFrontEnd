using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityServer.Models
{
    public class AccountOptions
    {
        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = false;
    }
}
