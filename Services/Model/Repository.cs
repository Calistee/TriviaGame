using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Model
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext db;


        public Repository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
        public async Task<User> GetUser(int id)
        {
            User user = await db.User.FindAsync(id);

            return user;
        }

        public async Task<Question> GetRandomQuestion(int userId)
        {
            var userQuestionAnswers = await db.UserQuestionAnswer.Where(x => x.UserId == userId).Select(x => x.QuestionId).ToListAsync<int>();

            var question = await db.Question.Where(x => !userQuestionAnswers.Contains(x.Id))
            .Include(x => x.Answers).OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefaultAsync();


            return question;
        }
        public async Task<Answer> GetAnswer(int id)
        {
            return await db.Answer.FindAsync(id);
        }

        public async Task AddUserQuestionAnswer(UserQuestionAnswer userQuestionAnswer)
        {
            await db.UserQuestionAnswer.AddAsync(userQuestionAnswer);
        }

        public int GetUserPlayedCount(int userId)
        {
            return db.UserQuestionAnswer.Where(x => x.UserId == userId).Count();
        }
    }
}
