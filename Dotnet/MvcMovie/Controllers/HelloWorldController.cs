using Microsoft.AspNetCore.Mvc;

namespace MvcMovie.Controllers;

public class HelloWorldController : Controller
{
    
    public string Index()
    {
        return "To jest moja domyślna akcja (Index)...";
    }

    public string Welcome()
    {
        return "To jest metoda akcji Welcome!";
    }
}