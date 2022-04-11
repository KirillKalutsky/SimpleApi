using System;
using System.Collections.Generic;
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
    public class ArticlesController:ControllerBase
    {
        private readonly ArticleRepository context;
        private readonly IMapper mapper;
        public ArticlesController(ArticleRepository context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public ActionResult<ArticleDto> GetArticleById([FromRoute] Guid id)
        {
            var article = context.FindById(id);

            if (article == null)
                return NotFound();

            return mapper.Map<ArticleDto>(article);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Guid>> GetArticlesId()
        {
            return Ok(context.GetAll());
        }

        [HttpGet("Creators/{creatorId}")]
        public ActionResult<IEnumerable<ArticleDto>> GetArticlesByIdCreator([FromRoute] Guid creatorId)
        {
            var articles = context.FindByCreatorId(creatorId);

            if (articles == null)
                return NotFound();

            var result = articles.Select(art => mapper.Map<ArticleDto>(art));

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Guid> Init([FromBody] ArticleToCreateDto articleDto)
        {
            if (articleDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var article = new Article();
            mapper.Map(articleDto, article);

            context.Insert(article);

            return article.Id;
        }

        [HttpPatch("{id}")]
        public ActionResult<Guid> Update([FromRoute] Guid id, 
            [FromBody] JsonPatchDocument<ArticleToUpdateDto> articleDto)
        {
            if (articleDto == null)
                return BadRequest();

            var article = context.FindById(id);

            if (article == null)
                return NotFound();

            var updateArticle = mapper.Map<ArticleToUpdateDto>(article);
            articleDto.ApplyTo(updateArticle,
                (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);
            if(!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var upA = mapper.Map(updateArticle, article);
            article = upA;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var article = context.FindById(id);

            if (article == null)
                return NotFound();

            context.Delete(article);
            return NoContent();
        }

    }
}
