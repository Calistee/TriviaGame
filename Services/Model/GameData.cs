using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Model
{
    public class GameData
    {
        public User User { get; set; }
        public Question Question { get; set; }
        public int MaxPlayCount { get; set; }
        public int PlayedCount { get; set; }
        public string Message { get; set; }
        public bool AnswerIsCorrect { get; set; }
    }
}
