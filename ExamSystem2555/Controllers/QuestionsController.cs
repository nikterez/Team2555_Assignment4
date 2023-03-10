using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MyDatabase.Models;
using NuGet.Packaging;
using System.Runtime.InteropServices;
using WebApp.DTO_Models;
using WebApp.DTO_Models.Certificates;
using WebApp.DTO_Models.Questions;
using WebApp.MainServices.Interfaces;

namespace WebApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionManagerService _service;
        private readonly IMapper _mapper;


        public QuestionsController(IQuestionManagerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [Authorize(Roles = "Quality Controller,Administrator")]
        public async Task<IActionResult> Index()
        {
            var questions = (await _service.QuestionService.GetAllQuestionsAsync()).Where(q => q.Status != "Unavailable");
            return View(questions);
        }

        // GET: Questions/Details
        [Authorize(Roles = "Quality Controller,Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _service.QuestionService == null)
            {
                return NotFound();
            }

            var question = await _service.QuestionService.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return NotFound();
            }
            await _service.QuestionLoad(question);
            var details = new QuestionDetailsView
            {
                QuestionDetailsViewId = question.QuestionId,
                QuestionDisplay = question.Display,
                QuestionDifficulty = question.QuestionDifficulty.Difficulty,
                Answers = _service.AnswerService.GetAllAnswersAsync().Result.Where(an => an.Question == question).ToList(),
                Topics = new List<Topic>()
            };
            var topicQuestion = _service.TopicQuestionService.GetAllTopicQuestionsAsync().Result.Where(tq => tq.Question == question).ToList();
            // topiQuestion where question with id 1 exists

            foreach (var item in topicQuestion)
            {
                await _service.TopicQuestionLoad(item);
                if (item.Topic != null)
                {
                    details.Topics.Add(item.Topic);
                }

            }

            // from certificatetopicquestions 

            details.Certificates = new List<Certificate>();
            var certificateTopicQuestion = _service.CertificateTopicQuestionService.GetAllCertificateTopicQuestionsAsync().Result.ToList();
            foreach (var item in topicQuestion)
            {
                var certTopicQuestList = (await _service.CertificateTopicQuestionService.GetAllCertificateTopicQuestionsAsync()).Where(ctq => ctq.TopicQuestion == item);
                foreach (var cert in certTopicQuestList)
                {
                    await _service.CertificateTopicsLoad(cert);

                    details.Certificates.Add(cert.CertificateTopic.Certificate);

                }
            }
            var x = details.Certificates.GroupBy(c => c.CertificateId).Select(x => x.First()).ToList();
            details.Certificates = x;

            return View(details);
        }

        // GET: Questions/Create
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Create()
        {
            var topicsList = await _service.TopicService.GetAllTopicsAsync();

            var certificateList = await _service.CertificateService.GetAllCertificatesAsync();
            await _service.CertificateLevelLoad(certificateList);

            var certificateListDTO = _mapper.Map<List<CertificateDTO>>(certificateList);

            var difficultiesList = await _service.QuestionDifficultyService.GetAllDifficultiesAsync();
            var difficultiesListDTO = _mapper.Map<List<QuestionDifficultyDTO>>(difficultiesList);

            var newQuestion = new NewCreateQuestionView();
            newQuestion.DifficultiesList = new SelectList(difficultiesListDTO, "QuestionDifficultyId", "Difficulty");
            newQuestion.CertificatesList = new MultiSelectList(certificateListDTO, "CertificateId", "FullTitle");
            newQuestion.TopicsList = new MultiSelectList(topicsList, "TopicId", "Title");

            return View(newQuestion);
        }


        // POST: Questions/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Create([FromForm] NewCreateQuestionView createdQuestion)
        {
            if (ModelState.IsValid)
            {
                var questionDiffculty = await _service.QuestionDifficultyService.GetDifficultyByIdAsync(createdQuestion.SelectedDifficultyId);
                var topicIds = createdQuestion.SelectedTopicIds;
                var certificatesId = createdQuestion.SelectedCertificateIds;

                var newQuestion = _mapper.Map<Question>(createdQuestion.Question);
                newQuestion.QuestionPossibleAnswers = _mapper.Map<List<QuestionPossibleAnswer>>(createdQuestion.Question.PossibleAnswers);
                newQuestion.QuestionDifficulty = questionDiffculty;

                if (topicIds == null)
                {
                    var newTopicQuestion = new TopicQuestion { Question = newQuestion, Topic = null };

                    var selectedCertificates = (await _service.CertificateService.SortCertificatesById(certificatesId)).ToList();
                    var ct = (await _service.CertificateTopicService.GetAllCertificateTopicsAsync()).ToList();




                    selectedCertificates.ForEach(sc => _service.CertificateTopicQuestionService.AddCertificateTopicQuestionAsync(new CertificateTopic { Certificate = sc, Topic = null }, newTopicQuestion));
                }
                else
                {
                    var selectedTopics = await _service.TopicService.SortTopicsById(topicIds);
                    var newTopicQuestionList = selectedTopics.Select(t => new TopicQuestion { Topic = t, Question = newQuestion });


                    foreach (var tq in newTopicQuestionList)
                    {
                        foreach (var ct in (await _service.CertificateTopicService.GetAllCertificateTopicsAsync()).ToList())
                        {
                            if (ct.Topic == tq.Topic)
                            {
                                await _service.CertificateTopicQuestionService.AddCertificateTopicQuestionAsync(ct, tq);
                            }
                        }
                    }
                }
                await _service.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            return View("Error", ModelState);
        }


        // GET: Questions/Edit
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int? id)
        {


            if (id == null || _service.QuestionService == null)
            {
                return NotFound();
            }

            var question = await _service.QuestionService.GetQuestionByIdAsync(id);
            var difficulties = await _service.QuestionDifficultyService.GetAllDifficultiesAsync();

            await _service.QuestionLoad(question);

            EditQuestionView editModel = new EditQuestionView()
            {
                EditQuestionId = question.QuestionId,
                EditQuestionDisplay = question.Display,
                EditDifficultyLevel = question.QuestionDifficulty.Difficulty

            };
            editModel.EditDifficulty.Difficulties = new SelectList(difficulties, "QuestionDifficultyId", "Difficulty");

            if (question == null)
            {
                return NotFound();
            }
            return View(editModel);
        }

        // POST: Questions/Edit/5
        [Authorize(Roles = "Administrator")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] EditQuestionView question)
        {
            //var newQuestion =await _service.QuestionService.GetQuestionByIdAsync(question.EditQuestionId);
            var newQuestion = _mapper.Map<Question>(question);
            newQuestion.QuestionDifficulty = await _service.QuestionDifficultyService.GetDifficultyByIdAsync(question.EditDifficulty.SelectedId);


            if (id != newQuestion.QuestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.QuestionService.UpdateQuestionAsync(newQuestion);

                    await _service.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(newQuestion.QuestionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));

            }
            return View(question);
        }



        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditAnswersIndex(int? id)
        {
            var question = await _service.QuestionService.GetQuestionByIdAsync(id);
            var answers = (await _service.AnswerService.GetAllAnswersAsync()).ToList();
            //answers.ForEach(async a => await _service.QuestionAnswerLoad(a));

            foreach (var item in answers)
            {
                await _service.QuestionAnswerLoad(item);
            }
            var myAnswers = answers.Where(x => x.Question.QuestionId == id);

            return View(myAnswers);
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditSelectedAnswer(int? id)
        {
            if (id == null || _service.QuestionService == null)
            {
                return NotFound();
            }

            var answer = await _service.AnswerService.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSelectedAnswer(int id, [FromForm] QuestionPossibleAnswer questionPossibleAnswer)
        {
            if (id != questionPossibleAnswer.QuestionPossibleAnswerId)
            {
                return NotFound();
            }

            var qAnswer = await _service.AnswerService.GetAnswerByIdAsync(id);
            qAnswer.IsCorrect = questionPossibleAnswer.IsCorrect;
            qAnswer.PossibleAnswer = questionPossibleAnswer.PossibleAnswer;
            await _service.QuestionAnswerLoad(qAnswer);

            await _service.AnswerService.UpdateAnswerAsync(qAnswer);
            await _service.SaveChanges();

            return RedirectToAction("EditAnswersIndex", new { id = qAnswer.Question.QuestionId });

        }

        public async Task<IActionResult> ConfirmChangeQuestionStatus(int? id)
        {
            if (id == null || await _service.QuestionService.GetAllQuestionsAsync() == null)
            {
                return NotFound();
            }

            var question = await _service.QuestionService.GetQuestionByIdAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<QuestionDTO>(question));
        }

        // POST: Questions/Delete
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeQuestionStatus(int QuestionId)
        {

            if (_service.QuestionService == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Questions'  is null.");
            }
            var question = await _service.QuestionService.GetQuestionByIdAsync(QuestionId);
            if (question != null)
            {
                question.Status = "Unavailable";
                await _service.QuestionService.UpdateQuestionAsync(question);
            }

            await _service.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _service.QuestionService.GetQuestionByIdAsync(id) != null;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditQuestionTopicsIndex(int? id)
        {
            EditQuestionTopicView model = new EditQuestionTopicView();
            List<Topic> topics = new List<Topic>();

            var allTopicQuestions = (await _service.TopicQuestionService.GetAllTopicQuestionsAsync()).ToList();
            foreach (var tq in allTopicQuestions)
            {
                await _service.TopicQuestionLoad(tq);
            }
            var x = allTopicQuestions[0].Question;
            var z = allTopicQuestions;
            var topicQuestions = allTopicQuestions.Where(tq => tq.Question.QuestionId == id);

            foreach (var topicQuestion in topicQuestions)
            {
                topics.Add(topicQuestion.Topic);
            }
            model.Topics = topics;
            model.QuestionId = id;

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddQuestionTopic(int? id, EditQuestionTopicView model)
        {

            var myQuestion = await _service.QuestionService.GetQuestionByIdAsync(id);

            var topicQuestions = (await _service.TopicQuestionService.GetAllTopicQuestionsAsync()).Where(x => x.Question == myQuestion);

            List<Topic> sortedTopics = new List<Topic>();
            foreach (var item in topicQuestions)
            {
                await _service.TopicQuestionLoad(item);
                if (item.Topic != null)
                {
                    sortedTopics.Add(item.Topic);
                }
            }

            List<Topic> final = new List<Topic>();


            var x = await _service.TopicService.GetAllTopicsAsync();

            foreach (var topic in sortedTopics)
            {
                foreach (var item in x)
                {
                    if (topic != item)
                        final.Add(item);
                }
            }

            model.TopicsList = new MultiSelectList(final, "TopicId", "Title");
            model.QuestionId = id;
            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestionTopicPost(int? id, [FromForm] EditQuestionTopicView model)
        {

            var question = await _service.QuestionService.GetQuestionByIdAsync(model.QuestionId);
            var selectedTopics = model.SelectedTopicIds.ToList();
            List<Topic> sortedTopics = new List<Topic>();

            var allTopics = await _service.TopicService.GetAllTopicsAsync();
            foreach (var item in allTopics)
            {

                foreach (var topic in selectedTopics)
                {
                    if (topic == item.TopicId)
                    {
                        sortedTopics.Add(item);
                    }
                }
            }
            var certificateTopics = await _service.CertificateTopicService.GetAllCertificateTopicsAsync();
            foreach (var cert in certificateTopics)
            {

                foreach (var topic in sortedTopics)
                {
                    if (topic == cert.Topic)
                    {
                        var certTopic = await _service.CertificateTopicService.GetCertificateTopicByIdAsync(cert.CertificateTopicId);
                        await _service.CertificateTopicQuestionService.AddCertificateTopicQuestionAsync(certTopic, new TopicQuestion { Topic = topic, Question = question });
                    }

                }
            }

            await _service.SaveChanges();

            return RedirectToAction("EditQuestionTopicsIndex", "Questions", new { id = model.QuestionId });



        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditQuestionCertificatesIndex(int? id)
        {
            EditQuestionCertificateView model = new EditQuestionCertificateView();
            List<Certificate> certificates = new List<Certificate>();

            var allCertificateTopicQuestions = await _service.CertificateTopicQuestionService.GetAllCertificateTopicQuestionsAsync();


            var allTopicQuestions = await _service.TopicQuestionService.GetAllTopicQuestionsAsync();
            foreach (var tq in allTopicQuestions)
            {
                await _service.TopicQuestionLoad(tq);
            }
            var topicQuestions = allTopicQuestions.Where(tq => tq.Question.QuestionId == id);

            foreach (var certTopicQuest in allCertificateTopicQuestions)
            {
                foreach (var topicQuestion in topicQuestions)
                {
                    if (certTopicQuest.TopicQuestion == topicQuestion)
                    {
                        await _service.CertificateTopicsLoad(certTopicQuest);
                        await _service.CertificateLevelLoad(certTopicQuest.CertificateTopic.Certificate);
                        certificates.Add(certTopicQuest.CertificateTopic.Certificate);
                    }
                }
            }

            model.Certificates = _mapper.Map<List<CertificateDTO>>(certificates);
            model.QuestionId = id;

            return View(model);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddQuestionCertificate(int? id, EditQuestionCertificateView model)
        {
            var myQuestion = await _service.QuestionService.GetQuestionByIdAsync(id);

            var topicQuestion = (await _service.TopicQuestionService.GetAllTopicQuestionsAsync()).First(x => x.Question == myQuestion);

            var ctq = await _service.CertificateTopicQuestionService.GetAllCertificateTopicQuestionsAsync();

            var myCt = new List<Certificate>();


            foreach (var item in ctq)
            {
                await _service.CertificateTopicsLoad(item);
                if (item.TopicQuestion == topicQuestion)
                {
                    myCt.Add(item.CertificateTopic.Certificate);
                }
            }

            List<Certificate> sortedCertificates = new List<Certificate>();
            var ct = await _service.CertificateService.GetAllCertificatesAsync();

            foreach (var certificate in ct)
            {
                foreach (var item in myCt)
                {
                    if (certificate != item)
                    {
                        sortedCertificates.Add(certificate);
                    }

                }
            }

            model.CertificatesList = new MultiSelectList(sortedCertificates, "CertificateId", "Title");
            model.QuestionId = id;

            return View(model);
        }


        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestionCertificatePost(int? id, [FromForm] EditQuestionCertificateView model)
        {

            var question = await _service.QuestionService.GetQuestionByIdAsync(model.QuestionId);
            var selectedCertificates = model.SelectedCertificateIds.ToList();
            List<Certificate> sortedCertificates = new List<Certificate>();

            var allCertificates = await _service.CertificateService.GetAllCertificatesAsync();

            selectedCertificates.ForEach(async x => sortedCertificates.Add(await _service.CertificateService.GetCertificateByIdAsync(x)));


            var allCertificateTopics = await _service.CertificateTopicService.GetAllCertificateTopicsAsync();

            foreach(var ct in allCertificateTopics) 
            {
                await _service.CertificateTopicLoad(ct);
            }

            List<CertificateTopic> certificateTopicsList = new List<CertificateTopic>();
            foreach (var certificate in sortedCertificates)
            {

                foreach (var certTopic in allCertificateTopics)
                {

                    if (certTopic.Certificate == certificate && certTopic.Topic == null)
                    {
                        certificateTopicsList.Add(certTopic);
                    }
                }

                if(certificateTopicsList.Count == 0 || certificateTopicsList.Any(x => x.Certificate != certificate))
                {
                    certificateTopicsList.Add(new CertificateTopic { Certificate = certificate, Topic = null });
                }

                //foreach (var item in certificateTopicsList)
                //{
                //    if(item.Certificate!=certificate)
                //    {
                //        certificateTopicsList.Add(new CertificateTopic { Certificate = certificate, Topic = null });
                //    }
                //}
            }

            var allTopicQuestions = await _service.TopicQuestionService.GetAllTopicQuestionsAsync();
            foreach (var topicQuestion in allTopicQuestions)
            {
                await _service.TopicQuestionLoad(topicQuestion);
            }

            TopicQuestion myNewTopicQuestion = new TopicQuestion();


            foreach (var topicQuestion in allTopicQuestions)
            {
                if (topicQuestion.Topic == null && topicQuestion.Question == question)
                {
                    myNewTopicQuestion = topicQuestion;
                }
                else
                {
                    myNewTopicQuestion = new TopicQuestion
                    {
                        Topic = null,
                        Question = topicQuestion.Question,
                    };
                }
            }

            if(myNewTopicQuestion.Topic != null) 
            {
                myNewTopicQuestion = new TopicQuestion { Topic = null, Question = question };
            }

            foreach (var ctl in certificateTopicsList)
            {
                await _service.CertificateTopicQuestionService.AddCertificateTopicQuestionAsync(ctl, myNewTopicQuestion);
            }

            //foreach (var sortedCertificate in sortedCertificates)
            //{

            //    if (certificateTopicsList.Count == 0)
            //    {

            //        await _service.CertificateTopicQuestionService.AddCertificateTopicQuestionAsync(new CertificateTopic { Certificate = sortedCertificate, Topic = null }, myNewTopicQuestion);

            //    }
            //    else
            //    {
            //        foreach (var certificateTopic in certificateTopicsList)
            //        {
            //            await _service.CertificateTopicQuestionService.AddCertificateTopicQuestionAsync(certificateTopic, myNewTopicQuestion);

            //        }
            //    }
            //}



            await _service.SaveChanges();

            return RedirectToAction("EditQuestionCertificatesIndex", "Questions", new { id = model.QuestionId });



        }

        // GET: Questions/Delete/5
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _service.QuestionService == null)
        //    {
        //        return NotFound();
        //    }

        //    var question = await _service.QuestionService.GetQuestionByIdAsync(id);
        //    if (question == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(question);
        //}



    }
}
