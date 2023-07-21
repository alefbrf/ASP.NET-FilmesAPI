using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.DTOs;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CinemaController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public CinemaController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    [HttpPost]
    public IActionResult AdicionaCinema([FromBody] CreateCinemaDTO cinemaDto)
    {
        Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
        _context.Cinemas.Add(cinema);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaCinemasPorId), new { Id = cinema.Id }, cinemaDto);
    }

    [HttpGet]
    public List<ReadCinemaDTO> RecuperaCinemas()
    {
        var cinemas = (
            from objCinema in _context.Cinemas
            join objEndereco in _context.Enderecos on objCinema.EnderecoId equals objEndereco.Id
            select new
            {
                id = objCinema.Id,
                nome = objCinema.Nome,
                endereco = new ReadEnderecoDTO {
                    Id = objEndereco.Id,
                    Numero = objEndereco.Numero,
                    Logradouro = objEndereco.Logradouro
                }
            }
        ).Select(p => new ReadCinemaDTO
        {
            Id = p.id,
            Nome = p.nome,
            Endereco = p.endereco
        }).ToList();

        var lstCinemaId = cinemas.Select(c => c.Id).ToList();

        var sessoes = (
            from objSessoes in _context.Sessoes.Where(p => lstCinemaId.Contains(p.CinemaId))
            select new Sessao
            {
                Id = objSessoes.Id,
                CinemaId = objSessoes.CinemaId
            }        
        ).ToList();

        foreach (ReadCinemaDTO cinema in cinemas)
        {
            cinema.Sessoes = sessoes.Where(p => cinema.Id == p.CinemaId).Select(x => new ReadSessaoDTO
            {
                Id = x.Id,
                FilmeId = x.FilmeId
            }).ToList();
        }

        return cinemas;
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaCinemasPorId(int id)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema != null)
        {
            ReadCinemaDTO cinemaDto = _mapper.Map<ReadCinemaDTO>(cinema);
            return Ok(cinemaDto);
        }
        return NotFound();
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaCinema(int id, [FromBody] UpdateCinemaDTO cinemaDto)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema == null)
        {
            return NotFound();
        }
        _mapper.Map(cinemaDto, cinema);
        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeletaCinema(int id)
    {
        Cinema cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema == null)
        {
            return NotFound();
        }
        _context.Remove(cinema);
        _context.SaveChanges();
        return NoContent();
    }

}
