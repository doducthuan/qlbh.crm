
        using System;
        using System.Collections.Generic;
        
        namespace QLBH_Dion.Util.Parameters
        {
            public class ActivityLogDTParameters: DTParameters
            {
                public List<int> AccountIds { get; set; } = new List<int>(); 

                public string SearchAll { get; set; } = "";
            }
        }
    