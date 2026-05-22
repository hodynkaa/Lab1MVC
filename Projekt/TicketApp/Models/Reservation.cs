using System.ComponentModel.DataAnnotations;

namespace TicketApp.Models;

public class Reservation
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Wybór wydarzenia jest wymagany.")]
    public int EventId { get; set; }
    public Event? Event { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;


    public ApplicationUser? User { get; set; }
    [Required(ErrorMessage = "Podaj liczbę biletów.")]
    [Range(1, 10, ErrorMessage = "Możesz kupić od 1 do 10 biletów jednorazowo.")]
    public int TicketsCount { get; set; } =1;

    [Required]
    public DateTime ReservationDate { get; set; } = DateTime.Now;
}