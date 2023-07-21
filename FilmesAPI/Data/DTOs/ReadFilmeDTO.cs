namespace FilmesAPI.Data.DTOs;

public class ReadFilmeDTO
{
    public string Titulo { get; set; }
    public string Genero { get; set; }
    public int Duracao { get; set; }
    public DateTime HoraConsulta { get; set; } = DateTime.Now;
    public List<ReadSessaoDTO> Sessoes { get; set; }
}
