using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketApp.Data;
using TicketApp.Models;

namespace TicketApp.Controllers;

public class EventsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Events.Include(e => e.Category);
        return View(await applicationDbContext.ToListAsync());
    }

    
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return View();
    }

   
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,EventDate,Location,TicketPrice,TotalSeats,AvailableSeats,CategoryId")] Event ticketEvent)
    {
        if (ModelState.IsValid)
        {
            _context.Add(ticketEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", ticketEvent.CategoryId);
        return View(ticketEvent);
    }

    
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var ticketEvent = await _context.Events.FindAsync(id);
        if (ticketEvent == null) return NotFound();
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", ticketEvent.CategoryId);
        return View(ticketEvent);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,EventDate,Location,TicketPrice,TotalSeats,AvailableSeats,CategoryId")] Event ticketEvent)
    {
        if (id != ticketEvent.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ticketEvent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(ticketEvent.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", ticketEvent.CategoryId);
        return View(ticketEvent);
    }

    
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var ticketEvent = await _context.Events
            .Include(e => e.Category)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (ticketEvent == null) return NotFound();

        return View(ticketEvent);
    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var ev = await _context.Events
            .Include(e => e.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (ev == null) return NotFound();

        return View(ev);
    }

    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ConfirmPurchase(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        var userId = _userManager.GetUserId(User);
        if (ev != null && ev.AvailableSeats > 0 && userId != null)
        {
            ev.AvailableSeats--;
            _context.Update(ev);

            var reservation = new Reservation
            {
                UserId = userId,
                EventId = id,
                ReservationDate = DateTime.Now
            };
            _context.Reservations.Add(reservation);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Płatność zakończona sukcesem! Bilet został zarezerwowany.";
        
            return RedirectToAction("Index", "Reservations");           
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ticketEvent = await _context.Events.FindAsync(id);
        if (ticketEvent != null) _context.Events.Remove(ticketEvent);
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.Id == id);
    }
}