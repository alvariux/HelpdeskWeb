using Microsoft.AspNetCore.Mvc;
using HelpDeskWeb.Models;
using HelpDeskWeb.Data;

namespace HelpDeskWeb.Controllers
{
    public class TicketPriorityController : Controller
    {
        private AppDbContext _appDbContext;

        public TicketPriorityController(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<TicketPriority> list = _appDbContext.TicketPriority.ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketPriority ticketPriority)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.TicketPriority.Add(ticketPriority);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketPriority);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _appDbContext.TicketPriority.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TicketPriority ticketPriority)
        {
            if (ModelState.IsValid)
            {
                // Cargar existente y actualizar solo campos permitidos
                var existing = _appDbContext.TicketPriority.Find(ticketPriority.Id);
                if (existing == null) return NotFound();

                existing.Name = ticketPriority.Name;
                existing.UpdatedAt = DateTime.Now;

                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticketPriority);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = _appDbContext.TicketPriority.Find(id);
            if (item == null) return NotFound();
            _appDbContext.TicketPriority.Remove(item);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
