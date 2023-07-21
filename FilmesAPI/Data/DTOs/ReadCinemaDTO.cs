namespace FilmesAPI.Data.DTOs;

public class ReadCinemaDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public ReadEnderecoDTO Endereco { get; set; }
    public List<ReadSessaoDTO> Sessoes { get; set; }
}
