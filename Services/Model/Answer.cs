using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Services.Model
{
    public class Answer
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public int Point { get; set; }
        public bool IsCorrect { get; set; }
    }
}
