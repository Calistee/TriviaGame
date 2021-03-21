using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Services;
using Services.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Triviagame.Test
{
    [TestFixture]
    public class GameServicesTests
    {
        private Mock<IRepository> repo;
        ServiceProvider _provider;
        private IGameServices service;
        private GameSettings gameSettings;

        [OneTimeSetUp]
        public void GlobalPrepare()
        {
            var services = new ServiceCollection();
            services.AddTransient<IOptions<GameSettings>>(
                provider => Options.Create<GameSettings>(new GameSettings
                {
                    MaxPlayCount = 5
                }));
            _provider = services.BuildServiceProvider();
        }

        [SetUp]
        public void Setup()
        {
            repo = new Mock<IRepository>();
            IOptions<GameSettings> options = _provider.GetService<IOptions<GameSettings>>();
            service = new GameServices(repo.Object, options);
            gameSettings = options.Value;
        }
        [Test]
        public async Task GetGameData_WhenExceptionThrows_ReturnMessage()
        {
            int userId = 1;
            User user = new User { Id = 1 };
            repo.Setup(r => r.GetUser(userId)).ReturnsAsync(user);
            repo.Setup(r => r.GetRandomQuestion(userId)).ThrowsAsync(new Exception());

            var gameData = await service.GetGameData(userId);

            Assert.That(gameData.Message, Is.Not.Empty);
            Assert.That(gameData.Message, Is.Not.Null);
        }
        [Test]
        public async Task GetGameData_WhenUserNotExists_ReturnMessage()
        {
            User user = null;
            int userId = 1;
            repo.Setup(r => r.GetUser(userId)).ReturnsAsync(user);

            var gameData = await service.GetGameData(userId);

            Assert.That(gameData.Message, Is.Not.Empty);
            Assert.That(gameData.Message, Is.Not.Null);
            Assert.That(gameData.Question, Is.Null);
        }
        [Test]
        public async Task GetGameData_WhenUserAlreadyAnsweredAll_ReturnMessage()
        {
            int userId = 1;
            User user = new User { Id = 1 };
            repo.Setup(r => r.GetUser(userId)).ReturnsAsync(user);
            repo.Setup(r => r.GetRandomQuestion(userId)).ReturnsAsync(new Question());

            repo.Setup(r => r.GetUserPlayedCount(userId)).Returns(gameSettings.MaxPlayCount);

            var gameData = await service.GetGameData(userId);

            Assert.That(gameData.Message, Is.Not.Empty);
            Assert.That(gameData.Message, Is.Not.Null);
        }
        [Test]
        public async Task GetGameData_WhenUserNotAnsweredAll_ReturnQuestion()
        {
            int userId = 1;
            User user = new User { Id = 1 };
            repo.Setup(r => r.GetUser(userId)).ReturnsAsync(user);
            repo.Setup(r => r.GetRandomQuestion(userId)).ReturnsAsync(new Question());

            repo.Setup(r => r.GetUserPlayedCount(userId)).Returns(gameSettings.MaxPlayCount - 1);

            var gameData = await service.GetGameData(userId);

            Assert.That(gameData.Message, Is.Empty);
            Assert.That(gameData.Question, Is.Not.Null);
        }
        [Test]
        public async Task AnswerQuestion_WhenAnswerIsCorrect_AddScore()
        {
            int userId = 1;
            User user = new User { Id = 1, Score = 0 };
            int answerId = 2;
            Answer answer = new Answer { Id = answerId, IsCorrect = true, Point = 5 };
            repo.Setup(r => r.GetAnswer(It.IsAny<int>())).ReturnsAsync(answer);
            repo.Setup(r => r.GetUser(userId)).ReturnsAsync(user);


            var gameData = await service.AnswerQuestion(userId, 1, answerId);

            Assert.That(gameData.User.Score.Equals(5));
            Assert.That(gameData.AnswerIsCorrect, Is.True);
        }
        [Test]
        public async Task AnswerQuestion_WhenAnswerIsNotCorrect_NoScore()
        {
            int userId = 1;
            User user = new User { Id = 1, Score = 10 };
            int answerId = 2;
            Answer answer = new Answer { Id = answerId, IsCorrect = false, Point = 0 };
            repo.Setup(r => r.GetAnswer(It.IsAny<int>())).ReturnsAsync(answer);
            repo.Setup(r => r.GetUser(userId)).ReturnsAsync(user);


            var gameData = await service.AnswerQuestion(userId, 1, answerId);

            Assert.That(gameData.User.Score.Equals(10));
            Assert.That(gameData.AnswerIsCorrect, Is.False);
        }
        [Test]
        public async Task AnswerQuestion_WhenExceptionThrows_ReturnMessage()
        {
            int userId = 1;
            User user = new User { Id = 1, Score = 0 };
            int answerId = 2;
            Answer answer = new Answer { Id = answerId, IsCorrect = true, Point = 5 };
            repo.Setup(r => r.GetAnswer(It.IsAny<int>())).ReturnsAsync(answer);
            repo.Setup(r => r.GetUser(userId)).ReturnsAsync(user);
            repo.Setup(r => r.AddUserQuestionAnswer(It.IsAny<UserQuestionAnswer>())).ThrowsAsync(new Exception());

            var gameData = await service.AnswerQuestion(userId, 1, answerId);

            Assert.That(gameData.Message, Is.Not.Null);
            Assert.That(gameData.Message, Is.Not.Empty);
        }
    }
}