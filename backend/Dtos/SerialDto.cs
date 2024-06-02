namespace backend.Dtos
{
    public class SerialDto
    {
        public int? Index { get; set; }
        public int Lesson_ID { get; set; }
        public int? Exam_ID { get; set; }
    }
    public class UpdateSerialDto
    {
        public int? Index { get; set; }
        public int? Lesson_ID { get; set; }
        public int? Exam_ID { get; set; }
    }
}
