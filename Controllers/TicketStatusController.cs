using Microsoft.AspNetCore.Mvc;
using HelpDeskWeb.Models;
using HelpDeskWeb.Data;

namespace HelpDeskWeb.Controllers
{
    public class TicketStatusController : Controller
    {
        private AppDbContext _appDbContext;

        public TicketStatusController(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<TicketStatus> ticketStatusList = _appDbContext.TicketStatus.ToList();
            return View(ticketStatusList);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketStatus ticketStatus)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.TicketStatus.Add(ticketStatus);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketStatus);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ticketStatus = _appDbContext.TicketStatus.Find(id);
            if (ticketStatus == null)
            {
                return NotFound();
            }
            return View(ticketStatus);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TicketStatus ticketStatus)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.TicketStatus.Update(ticketStatus);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketStatus);
        }
    }
}
