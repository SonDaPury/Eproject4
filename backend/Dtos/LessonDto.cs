namespace backend.Dtos
{
    public class LessonDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? VideoDuration { get; set; }
        public string? Thumbnail { get; set; }
        public string? Video { get; set; }
        public int Index { get; set; }
        public int View { get; set; }
        public bool Status { get; set; }
        public int ChapterId { get; set; }
    }
}
