using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Entities
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("image")]
        public string? Image { get; set; }
        public ICollection<QuizQuestion>? QuizQuestions { get; set; }
        public ICollection<Option>? Options { get; set; }
    }
}
