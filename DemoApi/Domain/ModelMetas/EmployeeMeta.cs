namespace DemoApi.Domain.ModelMetas
{
    public class EmployeeMeta
    {
        public string EmployeeCode { get; set; } = string.Empty;
        public Guid? JobApplicationId { get; set; }
        public Guid JobPositionId { get; set; }
        public Guid PrimaryFacilityId { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public DateTime HireDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
