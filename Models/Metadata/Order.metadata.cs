using System;
using System.ComponentModel.DataAnnotations;

namespace Models.EF
{
    [MetadataTypeAttribute(typeof(OrderMetadata))]
    public partial class Order
    {
        internal sealed class OrderMetadata
        {
            [Display(Name = "Ngày tạo")]
            public Nullable<System.DateTime> CreatedDate { get; set; }
            [Display(Name = "Tổng tiền")]
            public Nullable<double> TotalCost { get; set; }
            [Display(Name = "Trạng thái")]
            public Nullable<int> Status { get; set; }
            [Display(Name = "Tên khách hàng")]
            public string CustomerName { get; set; }
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }
        }
    }
}
