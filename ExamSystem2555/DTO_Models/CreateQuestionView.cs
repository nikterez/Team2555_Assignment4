using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyDatabase.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.DTO_Models
{
    public class CreateQuestionView
    {
        [ValidateNever]
        public int QId { get; set; }
        public string QDisplay { get; set; }
        [ValidateNever]
        public QuestionDifficultyView Difficulty { get; set; }
        [ValidateNever]
        public AnswerView[] AnswerViews { get; set; }
        [ValidateNever]
        public TopicView TopicView { get; set; }
        [ValidateNever]
        public CertificatesView? CertificatesView { get; set; }

        public CreateQuestionView()
        {
            Difficulty = new QuestionDifficultyView();
            AnswerViews = new AnswerView[4];
            CertificatesView = new CertificatesView();
            TopicView = new TopicView();
        }
        public bool HasTopic { get; set; }


    }
}
