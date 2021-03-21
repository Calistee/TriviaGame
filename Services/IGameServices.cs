using Services.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IGameServices
    {
        Task<GameData> AnswerQuestion(int userId, int questionId, int answerId);
        Task<GameData> GetGameData(int userId);
    }
}