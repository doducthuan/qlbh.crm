using Microsoft.AspNetCore.Mvc;

namespace QLBH_Dion.Models
{
    public class QLBH_Response : IActionResult
    {
        public static string STATUS_SUCCESS = "200";
        public string status { get; set; }
        public string message { get; set; }
        public string value { get; set; }
        public IList<Object> data { get; set; }
        public object resources { get; set; }




        public QLBH_Response(string status, string message, IList<Object> data)
        {
            this.status = status;
            this.message = message;
            this.data = data;
        }
        public QLBH_Response(string status, string message, object data)
        {
            this.status = status;
            this.message = message;
            this.resources = data;
        }
        public QLBH_Response(string status, string message)
        {
            this.status = status;
            this.message = message;
        }

        public QLBH_Response()
        {
        }

        public static QLBH_Response Success<T>(T? data, string? message = "SUCCESS") where T : class
        {
            return new QLBH_Response()
            {
                status = "200",
                message = message,
                resources = data
            };
        }
        public static QLBH_Response SUCCESS(IList<Object> data)
        {
            return new QLBH_Response("200", "SUCCESS", data);
        }

        public static QLBH_Response SUCCESS(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("200", "SUCCESS", returnData);
        }
        public static QLBH_Response BAD_REQUEST(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("400", "BAD_REQUEST", returnData);
        }

        public static QLBH_Response BAD_REQUEST()
        {
            List<Object> returnData = new List<Object>();
            var obj = new { Code = 400, Message = "BAD_REQUEST" };
            returnData.Add(obj);
            return new QLBH_Response("400", "BAD_REQUEST", returnData);
        }

        public static QLBH_Response SUCCESS()
        {
            return new QLBH_Response("200", "SUCCESS");
        }
        //trả về SUCCESSNOTBIDDING trong hoàn tiền đặt cọc
        //public static QLBH_Response SUCCESSNOTBIDDING(Object data)
        //{
        //    List<Object> returnData = new List<Object>();
        //    returnData.Add(data);
        //    return new QLBH_Response("205", "SUCCESSNOTBIDDING", returnData);
        //}
        public static QLBH_Response SUCCESSNOTBIDDING(IList<Object> data)
        {
            return new QLBH_Response("205", "SUCCESSNOTBIDDING", data);
        }
        //trả về SUCCESSHAVEBIDDING trong hoàn tiền đặt cọc
        //public static QLBH_Response SUCCESSHAVEBIDDING(Object data)
        //{
        //    List<Object> returnData = new List<Object>();
        //    returnData.Add(data);
        //    return new QLBH_Response("206", "SUCCESSHAVEBIDDING", returnData);
        //}
        public static QLBH_Response SUCCESSHAVEBIDDING(IList<Object> data)
        {
            return new QLBH_Response("206", "SUCCESSHAVEBIDDING", data);
        }

        public static QLBH_Response CREATED(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("201", "CREATED", returnData);
        }

        public static QLBH_Response Faild()
        {
            return new QLBH_Response("099", "FAILD");
        }
        public static QLBH_Response BiddingFaildEnded(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("096", "BIDDINGFAILD", returnData);
        }

        public static QLBH_Response EmailExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("202", "EMAILEXIST", data);
        }
        public static QLBH_Response EmailNotValid(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("204", "EMAILNOTVALID");
        }
        public static QLBH_Response UsernameExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("203", "USENAMEEXIST");
        }
        public static QLBH_Response IdCardNumberExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("205", "IDCARNUMBEREXIST");
        }
        public static QLBH_Response BiddingRequestExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("203", "BIDDINGREQUESTEXIST", returnData);
        }
        public static QLBH_Response PhoneExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("204", "PHONEEXIST");
        }
        public static QLBH_Response CompanyIdExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("206", "COMPANYIDEXIST");
        }
        public static QLBH_Response ItemExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("203", "ITEMEXIST", returnData);
        }
        public static QLBH_Response NotFoundBiddingMax()
        {
            return new QLBH_Response("999", "FAILD");
        }
        public static QLBH_Response BlockedAccount()
        {
            return new QLBH_Response("201", "BLOCKEDACCOUNT");
        }
        public static QLBH_Response WrongAccount()
        {
            return new QLBH_Response("202", "WRONGACCOUNT");
        }
        public static QLBH_Response NotFoundBiddingSecond()
        {
            return new QLBH_Response("998", "FAILD");
        }
        public static QLBH_Response PasswordExist(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("202", "PASSWORDEXIST", returnData);
        }
        public static QLBH_Response PasswordIsNotFormat(Object data)
        {
            List<Object> returnData = new List<Object>();
            returnData.Add(data);
            return new QLBH_Response("205", "PASSWORDISNOTINCORRECTFORMAT", returnData);
        }

        public static QLBH_Response OTP_REQUIRED(IList<Object> data)
        {
            return new QLBH_Response("200", "OTP_REQUIRED", data);
        }
        public static QLBH_Response OTP_OVER_LIMIT(IList<Object> data)
        {
            return new QLBH_Response("099", "OTP_OVER_LIMIT", data);
        }
        public static QLBH_Response OTP_INVALID_DATA(IList<Object> data)
        {
            return new QLBH_Response("098", "INVALID_DATA", data);
        }
        public static QLBH_Response OTP_EXIST()
        {
            return new QLBH_Response("204", "OTP_EXIST");
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
