namespace GHM.Infrastructure.Models
{
    public class BriefUser
    {
        /// <summary>
        /// Mã trùng với mã của tài khoản.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Mã khách hàng sử dụng hệ thống.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Tên đăng nhập.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Họ tên.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Ảnh đại diện.
        /// </summary>
        public string Avatar { get; set; }

    }
}
