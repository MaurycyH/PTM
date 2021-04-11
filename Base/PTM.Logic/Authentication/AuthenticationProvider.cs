using PTM.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PTM.Logic.Authentication
{
    public enum AuthenticationProvider
    {
        [EnumDescription("PTM.Oauth.Google")]
        Google,
        [EnumDescription("PTM.Oauth.Microsoft")]
        Microsoft
    }
}
