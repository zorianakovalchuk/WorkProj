using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkProj.Entity;
using WorkProj.Entity.Entity;

namespace WorkProj.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;

        private readonly DBContext _dbContext;

        public BookController(ILogger<BookController> logger, DBContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Home()
        {
            return View("~/Views/Book/Index.cshtml");
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            int bookId = int.Parse(id);
            Book book = _dbContext.Books.FirstOrDefault(b => b.Id == bookId);
            return View(book);
        }

    }
}
