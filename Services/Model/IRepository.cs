using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Model
{
    public interface IRepository
    {
        Task Save();
        Task<User> GetUser(int id);

        Task<Question> GetRandomQuestion(int userId);
        Task<Answer> GetAnswer(int id);

        Task AddUserQuestionAnswer(UserQuestionAnswer userQuestionAnswer);
        int GetUserPlayedCount(int userId);
    }
}
