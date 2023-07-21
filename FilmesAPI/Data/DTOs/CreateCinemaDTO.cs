using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs;

public class CreateCinemaDTO
{
    [Required(ErrorMessage = "Campo de nome obrigatório.")]
    public string Nome { get; set; }
    public int EnderecoId { get; set; }
}
