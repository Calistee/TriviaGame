using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Model;

namespace TriviaGame.Pages
{
    [BindProperties(SupportsGet = true)]
    public class GameModel : PageModel
    {
        private readonly IGameServices service;

        public GameModel(IGameServices service)
        {
            this.service = service;
            GameData = new GameData();
        }

        public string PopupMessage { get; set; }
        //public User CurrentUser { get; set; }

        //public Question Question { get; set; }
        public GameData GameData { get; set; }
        public int User_Id { get; set; }
        public async Task OnGetAsync(int user_id)
        {
            GameData = await service.GetGameData(user_id);

        }
        public async Task<IActionResult> OnPostAnswerQuestionAsync(int userId, int questionId, int answerId)
        {
            GameData = await service.AnswerQuestion(userId, questionId, answerId);

            //if(isCorrectAnswer)
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Checking');", true);
            //PopupMessage = isCorrectAnswer ? "Correct Answer" : "Wrong Answer!!";
            //return RedirectToPage(new { user_id = userId });

            return RedirectToAction("OnGetAsync", new { user_id = userId });

        }
    }
}
