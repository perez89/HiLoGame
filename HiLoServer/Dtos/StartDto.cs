namespace HiLoServer.Dtos;

public class StartDto
{
    [Required]
    public string GameId { get; set; }

    [Required]
    public string PlayerId { get; set; }

    [Required]
    public string Response { get; set; }
}