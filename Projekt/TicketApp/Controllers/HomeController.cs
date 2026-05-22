using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using TicketApp.Data;



namespace TicketApp.Controllers;



public class HomeController : Controller

{

    private readonly ApplicationDbContext _context;



    public HomeController(ApplicationDbContext context)

    {

        _context = context;

    }





    public async Task<IActionResult> Index(int? categoryId)

    {

        ViewBag.Categories = await _context.Categories.ToListAsync();



        var events = _context.Events.Include(e => e.Category).AsQueryable();



        if (categoryId.HasValue)

        {

            events = events.Where(e => e.CategoryId == categoryId);

        }



        return View(await events.ToListAsync());

    }

} 

