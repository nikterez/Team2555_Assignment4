using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MyDatabase.Models;
using System.Data;
using WebApp.DTO_Models.CandidateExaminations;
using WebApp.DTO_Models.Final;
using WebApp.MainServices;
using WebApp.MainServices.Interfaces;

namespace WebApp.Controllers
{
    public class CandidateExaminationsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICandidateExaminationManagerService _service;

        public CandidateExaminationsController(IMapper mapper, ICandidateExaminationManagerService service)
        {
            _mapper = mapper;
            _service = service;
        }

        public async Task<ActionResult> CandidateExaminationsIndex()
        {
            var candidateExaminations = await _service.CandidateExamService.GetAllCandidateExamAsync();
            await _service.CandidateExaminationLoad(candidateExaminations);
            var model = _mapper.Map<List<CandidateExaminationsDTO>>(candidateExaminations);
            return View(model);
        }

        public async Task<ActionResult> GetCandidateExaminationsForCandidate(int? id)
        {
            var candidate = await _service.CandidateService.GetCandidateByIdAsync(id);
            var candidateExaminations = (await _service.CandidateExamService.GetAllCandidateExamAsync()).Where(ce => ce.Candidate == candidate).ToList();

            foreach (var cExam in candidateExaminations)
            {
                await _service.CandidateExaminationLoad(cExam);

            }

            var completedExams = candidateExaminations.Where(ce=>ce.CandidateExamResults!=null).ToList();

            var model = _mapper.Map<List<CandidateExaminationsDTO>>(completedExams);

            return View(model);

        }


    }
}
