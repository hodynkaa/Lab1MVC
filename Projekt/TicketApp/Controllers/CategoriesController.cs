using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketApp.Data;
using TicketApp.Models;

namespace TicketApp.Controllers;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }

    
    public IActionResult Create()
    {
        if (User.Identity?.Name != "katyahour@gmail.com") return RedirectToAction("Index", "Home");
        return View();
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (User.Identity?.Name != "katyahour@gmail.com") return RedirectToAction("Index", "Home");

        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }
        return View(category); 
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (User.Identity?.Name != "katyahour@gmail.com") return RedirectToAction("Index", "Home");
        
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

}