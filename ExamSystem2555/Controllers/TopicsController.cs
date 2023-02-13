using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyDatabase.Models;
using WebApp.Data;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ITopicService _service;

        public TopicsController(ITopicService service)
        {
            _service = service;
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
              return View(await _service.GetAllTopicsAsync());
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _service.GetAllTopicsAsync() == null)
            {
                return NotFound();
            }

            var topic = await _service.GetTopicByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description")] Topic topic)
        {
            
                await _service.AddTopicAsync(topic);
                await _service.SaveAsync();
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _service.GetAllTopicsAsync() == null)
            {
                return NotFound();
            }

            var topic = await _service.GetTopicByIdAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,Title,Description")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateTopicAsync(topic);
                    await _service.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
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
            return View(topic);
        }


        private bool TopicExists(int id)
        {
          return _service.GetTopicByIdAsync(id) != null;
        }
    }
}
