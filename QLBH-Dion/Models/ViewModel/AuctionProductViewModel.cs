
        namespace QLBH_Dion.Models.ViewModels
        {
            public class AuctionProductViewModel : AuctionProduct
            {
                
                public string? ProductName {get; set;}
                public string ProductProvince { get; set;}
                public string AuctionProductBrand { get; set;}
                public int AuctionProductCategoryId {  get; set;}
                public string AuctionProductCategory {  get; set;}     
                public int AuctionProductBrandId { get; set; }
                public string AuctionProductStatusName {  get; set;}
                public int flagOrdered {  get; set;}
                public int orderId { get; set;}
		        public string? ProductCategoryName { get; set; }
                public int? ProvinveId { get; set;}
		        public string? ProvinceName { get; set; }
		        public string? ProductBrandName { get; set; }
                public string? LinkWebView { get; set; }

    }
}
