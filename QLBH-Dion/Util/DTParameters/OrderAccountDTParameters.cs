
        using System;
        using System.Collections.Generic;
        
        namespace QLBH_Dion.Util.Parameters
        {
            public class OrderAccountDTParameters: DTParameters
            {
                public List<int> AccountBuyIds { get; set; } = new List<int>(); 
public List<int> OrderAccountStatusIds { get; set; } = new List<int>(); 

                public string SearchAll { get; set; } = "";
            }
        }
    