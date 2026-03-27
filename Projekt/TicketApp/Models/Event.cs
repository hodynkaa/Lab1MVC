using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketApp.Models;

public class Event
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Wprowadź nazwę wydarzenia.")]
    [StringLength(100, ErrorMessage = "Nazwa nie może przekraczać 100 znaków.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Dodaj opis wydarzenia.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj datę i czas.")]
    public DateTime EventDate { get; set; }

    [Required(ErrorMessage = "Podaj miejsce wydarzenia.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj cenę biletu.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TicketPrice { get; set; }

    [Required(ErrorMessage = "Podaj całkowitą liczbę miejsc.")]
    public int TotalSeats { get; set; }

    public int AvailableSeats { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}