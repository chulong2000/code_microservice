namespace DemoApi.Domain.ModelMetas
{
    /// <summary>
    /// Tham số phân trang, sắp xếp và tìm kiếm dùng chung cho các API danh sách (GetAll).
    /// Dùng làm mẫu [FromQuery] cho các entity khác.
    /// </summary>
    public class PagingRequestMeta
    {
        private int _pageIndex = 1;
        private int _pageSize = 20;

        /// <summary>Trang hiện tại, bắt đầu từ 1. Giá trị nhỏ hơn 1 sẽ tự về 1.</summary>
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value < 1 ? 1 : value;
        }

        /// <summary>Số bản ghi mỗi trang. Giới hạn 1-200, mặc định 20.</summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value switch
            {
                < 1 => 20,
                > 200 => 200,
                _ => value
            };
        }

        /// <summary>Từ khoá tìm kiếm (search/filter theo Name).</summary>
        public string? Keyword { get; set; }

        /// <summary>Tên cột cần sắp xếp (VD: Name, Order, CreatedAt). Không hợp lệ -> dùng sắp xếp mặc định.</summary>
        public string? SortColumn { get; set; }

        /// <summary>true = giảm dần, false (mặc định) = tăng dần.</summary>
        public bool SortDescending { get; set; }
    }
}
