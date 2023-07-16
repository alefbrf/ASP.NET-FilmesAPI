using AutoMapper;
using Azure;
using FilmesAPI.Data;
using FilmesAPI.Data.DTOs;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;
    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionarFilme([FromBody]CreateFilmeDTO filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperarFilmeID), new { id = filme.Id }, filme);
    }

    /// <summary>
    /// Recupera uma lista de filmes do banco de dados
    /// </summary>
    /// <param name="skip">Inteiro que representa quantos filmes deve pular na consulta</param>
    /// <param name="take">Inteiro que representa quantos filmes deve pegar na consulta</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso a consulta seja feita com sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<ReadFilmeDTO> RecuperaFilmes([FromQuery]int skip = 0, [FromQuery]int take = 20)
    {
        return _mapper.Map<List<ReadFilmeDTO>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Recupera um filme do banco de dados
    /// </summary>
    /// <param name="id">Inteiro que representa o Id do filme que deve buscar</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso a consulta seja feita com sucesso</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult? RecuperarFilmeID(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDTO>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Atualiza um filme do banco de dados
    /// </summary>
    /// <param name="id">Inteiro que representa o Id do filme que deve atualizar</param>
    /// <param name="filmeDto">Objeto com as informações que deve atualizar no filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a consulta seja feita com sucesso</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaFilme(int id, [FromBody]UpdateFilmeDTO filmeDto)
    {
        var filme = (
            from objFilme in _context.Filmes
            where
                objFilme.Id == id
            select
                objFilme
            ).FirstOrDefault();

        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Atualiza parcialmente um filme do banco de dados
    /// </summary>
    /// <param name="id">Inteiro que representa o Id do filme que deve atualizar</param>
    /// <param name="patch">Objeto com as informações que deve atualizar no filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a consulta seja feita com sucesso</response>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDTO> patch)
    {
        var filme = (
            from objFilme in _context.Filmes
            where
                objFilme.Id == id
            select
                objFilme
        ).FirstOrDefault();

        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDTO>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if(!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta um filme do banco de dados
    /// </summary>
    /// <param name="id">Inteiro que representa o Id do filme que deve deletar</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a consulta seja feita com sucesso</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeletarFilme(int id)
    {
        var filme = (
            from objFilme in _context.Filmes
            where
                objFilme.Id == id
            select
                objFilme
        ).FirstOrDefault();

        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
