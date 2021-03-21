using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class GameServices : IGameServices
    {
        private readonly IRepository repo;
        private readonly GameSettings gameSettings;

        public GameServices(IRepository repo, IOptions<GameSettings> gameSettings)
        {
            this.repo = repo;
            this.gameSettings = gameSettings.Value;
        }



        public async Task<GameData> GetGameData(int userId)
        {
            GameData gameData = new GameData();
            try
            {
                gameData.User = await repo.GetUser(userId);
                if (gameData.User == null)
                {
                    gameData.Message = "User not exists!";
                    return gameData;
                }
                gameData.Question = await repo.GetRandomQuestion(userId);

                gameData.PlayedCount = repo.GetUserPlayedCount(userId);
                gameData.MaxPlayCount = gameSettings.MaxPlayCount;

                gameData.Message = ((gameData.PlayedCount >= gameData.MaxPlayCount) || gameData.Question == null) ? "There are no more questions" : "";

            }
            catch (Exception ex)
            {

                gameData.Message = "Something went wrong!";
            }

            return gameData;
        }

        public async Task<GameData> AnswerQuestion(int userId, int questionId, int answerId)
        {
            GameData gameData = new GameData();
            try
            {
                Answer answer = await repo.GetAnswer(answerId);

                User user = await repo.GetUser(userId);
                if (answer.IsCorrect)
                {
                    user.Score += answer.Point;
                }
                gameData.User = user;
                gameData.AnswerIsCorrect = answer.IsCorrect;

                UserQuestionAnswer userQuestionAnswer = new UserQuestionAnswer()
                {
                    UserId = userId,
                    QuestionId = questionId,
                    AnswerId = answer.Id
                };
                await repo.AddUserQuestionAnswer(userQuestionAnswer);
                await repo.Save();
            }
            catch (Exception ex)
            {
                gameData.Message = "Something went wrong!";
            }


            return gameData;
        }

    }
}
