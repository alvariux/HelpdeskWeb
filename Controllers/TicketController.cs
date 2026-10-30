using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using HelpDeskWeb.Models;
using HelpDeskWeb.Data;
using HelpDeskWeb.ViewModels;

namespace HelpDeskWeb.Controllers
{
    public class TicketController : Controller
    {
        private AppDbContext _appDbContext;

        public TicketController(AppDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        private List<User> GetAdminUsers()
        {
            return (from user in _appDbContext.Users
                    join userRole in _appDbContext.UserRoles on user.Id equals userRole.UserId
                    join role in _appDbContext.Roles on userRole.RoleId equals role.Id
                    where role.Name == "Administrador"
                    orderby user.FullName
                    select user)
                    .Distinct()
                    .ToList();
        }

        [Authorize(Roles = "Usuario")]
        [HttpGet]
        public IActionResult MyTickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = _appDbContext.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Where(t => t.CreatedById == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return View(list);
        }

        [Authorize(Roles = "Usuario")]
        [HttpGet]
        public IActionResult CreateUserTicket()
        {
            return View(new CreateUserTicketViewModel());
        }

        [Authorize(Roles = "Usuario")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserTicket(CreateUserTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Forbid();
                }

                var ticket = new Ticket
                {
                    Title = model.Title,
                    Description = model.Description,
                    TicketNumber = $"T-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4)}",
                    CreatedById = userId,
                    CreatedAt = DateTime.Now
                };

                _appDbContext.Tickets.Add(ticket);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(MyTickets));
            }

            return View(model);
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Index()
        {
            var list = _appDbContext.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .ToList();
            return View(list);
        }

        [Authorize(Roles = "Administrador,Usuario")]
        [HttpGet]
        public IActionResult Details(int id)
        {
            var ticket = _appDbContext.Tickets
                .Include(t => t.Status)
                .Include(t => t.Priority)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.ResolvedBy)
                .Include(t => t.ClosedBy)
                .FirstOrDefault(t => t.Id == id);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Statuses"] = _appDbContext.TicketStatus.ToList();
            ViewData["Priorities"] = _appDbContext.TicketPriority.ToList();
            ViewData["Users"] = new SelectList(GetAdminUsers(), "Id", "FullName");
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(ticket.TicketNumber))
                {
                    ticket.TicketNumber = $"T-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 4)}";
                }
                ticket.CreatedAt = DateTime.Now;
                _appDbContext.Tickets.Add(ticket);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Statuses"] = _appDbContext.TicketStatus.ToList();
            ViewData["Priorities"] = _appDbContext.TicketPriority.ToList();
            ViewData["Users"] = new SelectList(GetAdminUsers(), "Id", "FullName");
            return View(ticket);
        }        

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ticket = _appDbContext.Tickets.Find(id);
            if (ticket == null) return NotFound();
            ViewData["Statuses"] = _appDbContext.TicketStatus.ToList();
            ViewData["Priorities"] = _appDbContext.TicketPriority.ToList();
            ViewData["Users"] = new SelectList(GetAdminUsers(), "Id", "FullName", ticket.AssignedToId);
            return View(ticket);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var existing = _appDbContext.Tickets.Find(ticket.Id);
                if (existing == null) return NotFound();

                existing.Title = ticket.Title;
                existing.Description = ticket.Description;
                existing.StatusId = ticket.StatusId;
                existing.PriorityId = ticket.PriorityId;
                existing.AssignedToId = ticket.AssignedToId;
                existing.DueDate = ticket.DueDate;
                existing.UpdatedAt = DateTime.Now;

                await _appDbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Statuses"] = _appDbContext.TicketStatus.ToList();
            ViewData["Priorities"] = _appDbContext.TicketPriority.ToList();
            ViewData["Users"] = new SelectList(GetAdminUsers(), "Id", "FullName", ticket.AssignedToId);
            return View(ticket);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = _appDbContext.Tickets.Find(id);
            if (ticket == null) return NotFound();
            _appDbContext.Tickets.Remove(ticket);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
