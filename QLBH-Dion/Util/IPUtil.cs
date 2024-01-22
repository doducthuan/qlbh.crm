namespace QLBH_Dion.Util
{
    public static class IPUtil
    {
        public static string GetIPAddress(this HttpRequest Request)
        {
            try
            {
                if (Request.Headers["CF-CONNECTING-IP"].ToString() != null)
                {
                    return Request.Headers["CF-CONNECTING-IP"].ToString();
                }
            }
            catch (Exception e)
            {
                //throw;
            }
            return "";
        }
        public static string IPAddress(HttpRequest Request)
        {
            try
            {
                var remoteIpAddress = GetIPAddress(Request);
                if (remoteIpAddress == "")
                {
                    remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(); //Chỉ lấy dc IP của CloudFlare
                }
                return remoteIpAddress;
            }
            catch (Exception e)
            {
                //throw;
            }
            return "";
        }
    }
}
