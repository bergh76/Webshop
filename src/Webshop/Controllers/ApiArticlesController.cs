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
using Webshop.ViewModels;
using System.Globalization;

namespace Webshop.Controllers
{
    [Produces("application/json")]
    [Route("api/ApiArticles")]
    public class ApiArticlesController : Controller
    {
        private readonly WebShopRepository _context; //crate a service for context
        private readonly ArticleBusinessLayer _artBLL;
        //private readonly IHostingEnvironment _hostEnvironment;
        public ApiArticlesController(WebShopRepository context, ArticleBusinessLayer artBLL) //IFormCollection form,,
        {
            _artBLL = artBLL;
            _context = context;
            //_form = form;
        }

        // GET: api/ApiArticles
        [HttpGet]
        public IEnumerable<ArticlesViewModel> GetArticles()
        {
            var artList = from p in _context.Articles
                          join i in _context.Images on p.ArticleId equals i.ArtikelId
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice,
                              ArticleStock = p.ArticleStock,
                              CategoryID = p.CategoryId,
                              VendorID = p.VendorId,
                              ProductID = p.ProductId,
                              SubCategoryID = p.SubCategoryId,
                              ArticleName = pt.ArticleName,
                              ArticleShortText = pt.ArticleShortText,
                              ArticleFeaturesOne = pt.ArticleFeaturesOne,
                              ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                              ArticleFeaturesThree = pt.ArticleFeaturesThree,
                              ArticleFeaturesFour = pt.ArticleFeaturesFour,
                              ArticleImgPath = i.ImagePath + i.ImageName,
                              LangCode = pt.LangCode,
                              ISTranslated = pt.ISTranslated,
                              ISActive = p.ISActive,
                              ISCampaign = p.ISCampaign
                          };
            IEnumerable<ArticlesViewModel> vModel = artList.ToList();
            return vModel;
        }

        // GET: api/ApiArticles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticles([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var artList = from p in _context.Articles
                          where p.ArticleId == id
                          join i in _context.Images on p.ArticleId equals i.ArtikelId
                          join pt in _context.ArticleTranslations on
                                           new { p.ArticleId, Second = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName }
                                           equals new { pt.ArticleId, Second = pt.LangCode }
                          select new ArticlesViewModel
                          {
                              ArticleId = p.ArticleId,
                              ArticleNumber = p.ArticleNumber,
                              ArticlePrice = p.ArticlePrice,
                              ArticleStock = p.ArticleStock,
                              CategoryID = p.CategoryId,
                              VendorID = p.VendorId,
                              ProductID = p.ProductId,
                              SubCategoryID = p.SubCategoryId,
                              ArticleName = pt.ArticleName,
                              ArticleShortText = pt.ArticleShortText,
                              ArticleFeaturesOne = pt.ArticleFeaturesOne,
                              ArticleFeaturesTwo = pt.ArticleFeaturesTwo,
                              ArticleFeaturesThree = pt.ArticleFeaturesThree,
                              ArticleFeaturesFour = pt.ArticleFeaturesFour,
                              ArticleImgPath = i.ImagePath + i.ImageName,
                              LangCode = pt.LangCode,
                              ISTranslated = pt.ISTranslated,
                              ISActive = p.ISActive,
                              ISCampaign = p.ISCampaign
                          };

            if (artList == null)
            {
                return NotFound();
            }

            return Ok(await artList.SingleOrDefaultAsync(m => m.ArticleId == id));
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

            //_context.Entry(articles).State = EntityState.Modified;
            _context.Update(articles);


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

            return CreatedAtAction("GetArticles", new { id = articles.ArticleId }, articles);
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
                ImageModel img = new ImageModel()
                {
                    ArtikelId = articles.ArticleId,
                    ImageDate = DateTime.Now,
                    ImageName = "NO_product_fount.png",
                    ImagePath = "images/imageupload/NoImage/"
                };
                _context.Images.Add(img);
                await _context.SaveChangesAsync();
                articles.ArticleAddDate = DateTime.Now;
                articles.ImageId = img.ImageId;
                _context.Entry(articles).State = EntityState.Modified;
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
            ArticleTranslation artT = await _context.ArticleTranslations.SingleOrDefaultAsync(m => m.ArticleId == id);
            if (articles == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(articles);
            _context.ArticleTranslations.Remove(artT);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticles", articles);
            //return CreatedAtAction("GetArticles", new { id = articles.ArticleId }, articles);
        }

        private bool ArticlesExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }

    }
}