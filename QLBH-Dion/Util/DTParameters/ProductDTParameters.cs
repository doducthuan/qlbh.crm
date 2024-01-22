
        using System;
        using System.Collections.Generic;
        
        namespace QLBH_Dion.Util.Parameters
        {
            public class ProductDTParameters: DTParameters
            {
                public List<int> ProductCategoryIds { get; set; } = new List<int>(); 
public List<int> ProductBrandIds { get; set; } = new List<int>(); 
public List<int> ProvinceIds { get; set; } = new List<int>(); 

                public string SearchAll { get; set; } = "";
            }
        }
    