namespace UnCRM.Api.Contract
{
    public class ModelErrorContract
    {
        public int Status { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}