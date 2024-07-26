using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Data.DTOs;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers
{
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
        /// <param name="filmeDTO">Objeto com os campos necesssários para a criação de um filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDTO filmeDTO)
        {
            Filme filme = _mapper.Map<Filme>(filmeDTO);
            _context.Filmes.Add(filme);
            _context.SaveChanges(); 
            return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
        }

        /// <summary>
        /// Busca todos os filmes cadastrados no banco
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery]int skip, [FromQuery] int take = 10)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
        }

        /// <summary>
        /// Busca o filme conforme o id informado
        /// </summary>
        /// <param name="id">Informação de id numerico que deve ser procurado</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult RecuperaFilmePorId(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();
            var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
            return Ok(filmeDto);
        }

        /// <summary>
        /// Alteração de um ou mais campos colocando todas as informações cadastradas
        /// </summary>
        /// <param name="id">Objeto numerico para identificação do filme</param>
        /// <param name="filmeDto">Objeto com os campos do filme</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();
            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Alteração de apenas um campo passando o seu caminho
        /// </summary>
        /// <param name="id">Objeto numerico para identifação do filme</param>
        /// <param name="patch">Campo a ser alterado. ex: "path": "/titulo"</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();

            var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

            patch.ApplyTo(filmeParaAtualizar, ModelState);

            if (!TryValidateModel(filmeParaAtualizar)){
                return ValidationProblem(ModelState);
            }

            _mapper.Map(filmeParaAtualizar, filme);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Deleção de filmes conforme o id informado
        /// </summary>
        /// <param name="id">Objeto numerico para identifação do filme</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeletaFilme(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();

            _context.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
