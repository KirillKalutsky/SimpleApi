using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.Core;
using SimpleApi.Repositories;

namespace SimpleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorRepository context;
        private readonly IMapper mapper;
        public AuthorsController(AuthorRepository context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        public ActionResult<Guid> Init([FromBody] AuthorToCreateDto authorDto)
        {
            if (authorDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity();

            var author = mapper.Map<Author>(authorDto);

            context.Insert(author);

            return author.Id;
        }

        [HttpGet("{authorId}")]
        public ActionResult<Author> GetAuthorById([FromRoute] Guid authorId)
        {
            var author = context.FindById(authorId);

            if (author == null)
                return NotFound();

            return author;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Guid>> GetAuthorsId()
        {
            return Ok(context.GetAll());
        }

        [HttpPut("{authorId}")]
        public IActionResult AddArticleToAuthorById([FromRoute] Guid authorId, [FromBody] Guid articleId)
        {
            if (articleId == Guid.Empty)
                return BadRequest();

            var author = context.FindById(authorId);
            if (author == null)
                return NotFound($"{nameof(authorId)}");

            var isSuccess = context.AddArticleById(author, articleId);
            if (isSuccess)
                return NoContent();
            return NotFound($"{articleId}");
        }

        [HttpPatch("{authorId}")]
        public IActionResult UpdateAuthor([FromRoute] Guid authorId,
            [FromBody] JsonPatchDocument<AuthorToUpdateDto> authorUpdate)
        {
            if (authorUpdate == null)
                return BadRequest();

            var author = context.FindById(authorId);
            if (author == null)
                return NotFound();

            var authorDto = mapper.Map<AuthorToUpdateDto>(author);
            authorUpdate.ApplyTo(authorDto, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            if (!ModelState.IsValid)
                return UnprocessableEntity();

            var updateAuthor = mapper.Map(authorDto, author);

            author = updateAuthor;
            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{authorId}")]
        public IActionResult Remove([FromRoute] Guid authorId)
        {
            var author = context.FindById(authorId);
            if (author == null)
                return NotFound();

            context.Remove(author);

            return NoContent();
        }
    }
}
