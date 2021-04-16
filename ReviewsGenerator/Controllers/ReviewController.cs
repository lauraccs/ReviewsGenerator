using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReviewsGenerator.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReviewsGenerator.Controllers
{
    [Route("api/reviews")]
    public class ReviewController : Controller
    {


        public ReviewController()
        {
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpGet]
        [Route("generate")]
        public IActionResult GenerateReview()
        {
            var markov = new MarkovBase.MarkovBase();
            var review = markov.GenerateReview();
            if (review != null)
            {
                return Ok(review);
            }
            else
            {
                return NoContent();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
