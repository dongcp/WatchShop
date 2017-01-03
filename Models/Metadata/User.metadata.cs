using System;
using System.ComponentModel.DataAnnotations;

namespace Models.EF
{

    [MetadataTypeAttribute(typeof(UserMetadata))]
    public partial class User
    {
        internal sealed class UserMetadata
        {
            [Display(Name="Tên đăng nhập")]
            public string Username { get; set; }
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }
            [Display(Name = "Họ tên")]
            public string Name { get; set; }
            [Display(Name = "Ngày sinh")]
            public Nullable<System.DateTime> Birthday { get; set; }
            [Display(Name = "Địa chỉ")]
            public string Address { get; set; }
            [Display(Name = "Số điện thoại")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Nhóm người dùng")]
            public string UserGroupId { get; set; }
        }
    }
}
