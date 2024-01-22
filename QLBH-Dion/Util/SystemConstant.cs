using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLBH_Dion.Util
{
    public class SystemConstant
    {
        public static string ACTIVATION_PAGE_URL = "activation.html";
        public static string DEFAULT_URL = "https://localhost:44350/";
        public static int ROLE_ANONYMOUS_USER = 0;
        public static int ROLE_SYSTEM_ADMIN = 1000001;
        public static int AUCTION_PRODUCT_STATUS_ID_WAITING = 1001;
        public static int AUCTION_PRODUCT_STATUS_ID_GOING = 1002;
        public static int AUCTION_PRODUCT_STATUS_ID_ENDED = 1003;

        //Property Category
        public static int TU_QUY = 3;
        public static int TAM_HOA = 4;
        public static int NGU_QUY = 2;
        public static int LOC_PHAT = 6;
        public static int ONG_DIA = 7;
        public static int THAN_TAI = 1;
        public static int SANH_TIEN = 9;
        public static int PHONG_THUY = 5;
        //Property Brand
        public static int XE_CON = 1;
        public static int XE_KHACH = 2;
        public static int XE_TAI = 3;
        public static int XE_TAI_VAN = 4;
        // Auction property status
        public static int CHUA_DAU_GIA = 1001;
        public static int DANG_DAU_GIA = 1002;
        public static int DA_DAU_GIA = 1003;
    }
}
