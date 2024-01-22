
using System;
using System.Collections.Generic;

namespace QLBH_Dion.Util.Parameters
{
    public class OrdersDTParameters : DTParameters
    {
        public List<int> AuctionProductIds { get; set; } = new List<int>();
        public List<int> OrderStatusIds { get; set; } = new List<int>();
        public List<int> AccountIds { get; set; } = new List<int>();
        public string SearchAll { get; set; } = "";
    }
}
