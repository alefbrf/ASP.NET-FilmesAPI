using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.DTOs;

public class CreateFilmeDTO
{
    [Required(ErrorMessage = "Título obrigatório")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "Gênero obrigatório")]
    [StringLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required(ErrorMessage = "Duração obrigatório")]
    [Range(70, 600, ErrorMessage = "Duração deve ter entre 70 e 600 minutos")]
    public int Duracao { get; set; }
}
