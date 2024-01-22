namespace QLBH_Dion.Constants
{
    public class OrderStatusShipId
    {
        public static List<int> DELIVERING = new List<int>() { 1017, 1018, 1023, 1024, 1030, 1034, 1035 };
        public static List<int> RECEIVED = new List<int>() { 1019, 1020, 1033 };
        public static List<int> CANCEL = new List<int>() { 1065 };
        public static List<int> RETURN = new List<int>() { 1027, 1028, 1029 };
        public static List<int> WAITFORCONFIRM = new List<int>() { 1015 };
        public static List<int> CONFIRM = new List<int>() { 1016, 1022, 1026 };

    }
}
