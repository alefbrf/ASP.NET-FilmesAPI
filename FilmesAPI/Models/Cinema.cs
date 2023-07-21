using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Cinema
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "Campo de nome obrigatório.")]
    public string Nome { get; set; }
    public int EnderecoId { get; set; }
    public virtual Endereco Endereco { get; set; }
    public virtual ICollection<Sessao> Sessoes { get; set; }
}
