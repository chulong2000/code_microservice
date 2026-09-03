namespace DemoApi.Domain.ViewModels
{
    /// <summary>
    /// Kết quả danh sách có phân trang dùng chung cho các API GetAll.
    /// </summary>
    public class PagedResultViewModel<T>
    {
        public List<T> Items { get; set; } = [];
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalRecords / (double)PageSize) : 0;
    }
}
