using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webshop.Models;
using Webshop.Models.BusinessLayers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;

namespace Webshop.Controllers
{
    [Produces("application/json")]
    [Route("api/ApiArticles")]
    public class ApiArticlesController : Controller
    {
        private readonly WebShopRepository _context; //crate a service for context
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IFormCollection _form;
        public ApiArticlesController(IFormCollection form, IHostingEnvironment hostEnvironment, WebShopRepository context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
            _form = form;
        }

        // GET: api/ApiArticles
        [HttpGet]
        public IEnumerable<Articles> GetArticles()
        {
            return _context.Articles;
        }

        // GET: api/ApiArticles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Articles articles = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleId == id);

            if (articles == null)
            {
                return NotFound();
            }

            return Ok(articles);
        }

        // PUT: api/ApiArticles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticles([FromRoute] int id, [FromBody] Articles articles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != articles.ArticleId)
            {
                return BadRequest();
            }

            _context.Entry(articles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticlesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApiArticles
        [HttpPost]
        public async Task<IActionResult> PostArticles([FromBody] Articles articles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Articles.Add(articles);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ArticlesExists(articles.ArticleId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetArticles", new { id = articles.ArticleId }, articles);
        }

        // DELETE: api/ApiArticles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Articles articles = await _context.Articles.SingleOrDefaultAsync(m => m.ArticleId == id);
            if (articles == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(articles);
            await _context.SaveChangesAsync();

            return Ok(articles);
        }

        private bool ArticlesExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }

        // DELETE: api/ApiArticles

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ArticleBusinessLayer article, Articles art, ArticleTranslation artTrans)
        {
            if (article == null)
            {
                return BadRequest();
            }
            await article.AddArticle(art, artTrans, _context, _form);
                return CreatedAtRoute("GetArticles", new { id = art.ArticleId }, article);
        }
    }
}