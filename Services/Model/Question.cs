using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Model
{
    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public IList<Answer> Answers { get; set; }

    }
}
