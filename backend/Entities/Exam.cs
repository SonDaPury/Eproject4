using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("time_limit")]
        public int TimeLimit { get; set; }

        [Column("max_question")]
        public int MaxQuestion { get; set; }

        /// <summary>
        /// bài ktra đã hoàn thành chưa
        /// </summary>

        [Column("status")]
        public bool Status { get; set; }

        [Column("chapter_id")]
        public int ChapterId { get; set; }
        public virtual Chapter? Chapter { get; set; }
        public ICollection<QuizQuestion>? QuizQuestions { get; set; }
        public ICollection<Answer>? Answers { get; set; }
        
    }
}
