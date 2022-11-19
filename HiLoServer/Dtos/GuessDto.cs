namespace HiLoServer.Dtos;

public class GuessDto
{
    [Required]
    public string GameId { get; set; }

    [Required]
    public string PlayerId { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
    public int Guess { get; set; }
}