using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Models.EF
{
    [MetadataTypeAttribute(typeof(ProductMetadata))]
    public partial class Product
    {
        internal sealed class ProductMetadata
        {
            public string Id { get; set; }
            [Display(Name = "Tên sản phẩm")]
            public string Name { get; set; }
            [Display(Name = "Loại sản phẩm")]
            public Nullable<int> Type { get; set; }
            [Display(Name = "Thương hiệu")]
            public string BranchId { get; set; }
            [Display(Name = "Giá bán")]
            public Nullable<double> Price { get; set; }
            [Display(Name = "Giảm giá")]
            public Nullable<int> Discount { get; set; }
            [Display(Name = "Mô tả")]
            public string Description { get; set; }
            [Display(Name = "Ảnh")]
            public string Image { get; set; }
            [Display(Name = "Số lượt xem")]
            public Nullable<int> View { get; set; }
        }
    }
}
