namespace backend.Dtos
{
    public class QuestionForExamDto
    {
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
        public string? Options { get; set; }
    }
}
