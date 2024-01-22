namespace QLBH_Dion.Models
{
    public class FCMToken
    {
        public string DeviceToken { get; set; }
        public int Device { get; set; } // 1 la web, 0 la app
    }
}
