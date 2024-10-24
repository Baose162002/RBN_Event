namespace BusinessObject.DTO.ResponseDto
{
    public class ListSubscriptionPackageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int DurationInDays { get; set; }
    }
}
