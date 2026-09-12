namespace DemoApi.Domain.ViewModels
{
    public class WorkScheduleBulkCreateResultViewModel
    {
        // Số bản ghi (nhân viên x ngày làm việc) dự kiến tạo
        public int TotalRequested { get; set; }

        // Số bản ghi thực sự được tạo mới
        public int TotalCreated { get; set; }

        // Số bản ghi bị bỏ qua vì nhân viên đã có lịch vào ngày đó
        public int TotalSkipped { get; set; }
    }
}
