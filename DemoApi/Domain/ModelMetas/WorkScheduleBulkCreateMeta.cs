namespace DemoApi.Domain.ModelMetas
{
    public class WorkScheduleBulkCreateMeta
    {
        public Guid? FacilityID { get; set; }
        public List<WorkScheduleEntryRequest> Entries { get; set; } = null!;
    }
}
