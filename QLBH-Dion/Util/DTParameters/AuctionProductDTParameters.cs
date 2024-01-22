
        using System;
        using System.Collections.Generic;
        
        namespace QLBH_Dion.Util.Parameters
        {
            public class AuctionProductDTParameters: DTParameters
            {
                public List<int> ProductIds { get; set; } = new List<int>(); 
public List<int> AuctionProductIds { get; set; } = new List<int>(); 

                public string SearchAll { get; set; } = "";
            }
        }
    