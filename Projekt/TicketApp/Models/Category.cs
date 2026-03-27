using System.ComponentModel.DataAnnotations;

namespace TicketApp.Models;

public class Category
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa kategorii jest wymagana.")]
    [StringLength(50, ErrorMessage = "Nazwa nie może być dłuższa niż 50 znaków.")]
    public string Name { get; set; } = string.Empty;

    public List<Event>? Events { get; set; }
}