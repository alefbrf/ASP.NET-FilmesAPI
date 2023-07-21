using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs;

public class UpdateCinemaDTO
{
    [Required(ErrorMessage = "Campo de nome obrigatório.")]
    public string Nome { get; set; }
}
