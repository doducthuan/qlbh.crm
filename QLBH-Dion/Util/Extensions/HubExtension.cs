using QLBH_Dion.Constants;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace QLBH_Dion.Util.Extensions
{
    public static class HubExtension
    {
        public static string GetLoggedInUserInfo(this Hub hub, string key)
        {
            try
            {
                if (hub.Context?.User?.Identity is ClaimsIdentity identity)
                {
                    return identity.FindFirst(key)?.Value;
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static int GetLoggedInUserId(this Hub hub)
        {
            return Convert.ToInt32(GetLoggedInUserInfo(hub, ClaimNames.ID));
        }
    }
}
