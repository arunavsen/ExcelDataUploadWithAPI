namespace ExcelUploader.Models
{
    public class ExcelFileDTO
    {
        public int Id { get; set; }
        public string? Month { get; set; }
        public int Year { get; set; }
        public string? CountryCode { get; set; }
        public string? BriefType { get; set; }
        public string? Title { get; set; }
        public string? ShortStory { get; set; }
        public string? LongStory { get; set; }
        public string? Link { get; set; }
        public DateTime PubDate { get; set; }
    }
}
