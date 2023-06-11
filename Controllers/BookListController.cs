using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkProj.Entity;
using WorkProj.Entity.Entity;
using WorkProj.Models;
using WorkProj.Services;

namespace WorkProj.Controllers
{
    public class BookListController : Controller
    {
        private readonly ILogger<BookListController> _logger;

        private readonly DBContext _dbContext;

        private readonly IDRetriever _idRetriever;

        public BookListController(ILogger<BookListController> logger, DBContext dbContext, IDRetriever idRetriever)
        {
            _dbContext = dbContext;
            _logger = logger;
            _idRetriever = idRetriever;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SortByAlphabet()
        {
            UserViewModel.Books = UserViewModel.Books.OrderBy(b => b.Title).ToList();
            return View("~/Views/BookList/Index.cshtml");
        }

        public IActionResult SortByPrice(string param1)
        {
            if (param1.Equals("cheap"))
            {
                UserViewModel.Books = UserViewModel.Books.OrderBy(b => b.Price).ToList();
            }
            else
            {
                UserViewModel.Books = UserViewModel.Books.OrderByDescending(b => b.Price).ToList();
            }
            return View("~/Views/BookList/Index.cshtml");
        }

        public IActionResult SortByRating()
        {
            UserViewModel.Books = UserViewModel.Books.OrderByDescending(b => b.OpinionsNumber).ToList();
            return View("~/Views/BookList/Index.cshtml");
        }


        [HttpGet]
        public IActionResult Favorite(int bookId)
        {
            int userId = _idRetriever.GetLoggedInUserId();
            if (userId == 0)
            {
                return View("~/Views/BookList/Index.cshtml");
            }
            Book book = _dbContext.Books.FirstOrDefault(u => u.Id == bookId);
            User user = _dbContext.Users
                .Include(u => u.FavoriteBooks)
                .Include(u => u.CartBooks)
                .FirstOrDefault(u => u.Id == userId);

            if (user.FavoriteBooks.FirstOrDefault(Fb => Fb.Id == book.Id) is null)
            {
                user.FavoriteBooks.Add(book);
            }
            else
            {
                user.FavoriteBooks.Remove(book);
            }

            UserViewModel.User = user;

            _dbContext.SaveChanges();

            return View("~/Views/BookList/Index.cshtml");
        }

        [HttpGet]
        public IActionResult Buy(int bookId)
        {
            int userId = _idRetriever.GetLoggedInUserId();
            if (userId == 0)
            {
                return View("~/Views/BookList/Index.cshtml");
            }
            Book book = _dbContext.Books.FirstOrDefault(u => u.Id == bookId);
            User user = _dbContext.Users
                .Include(u => u.CartBooks)
                .Include(u => u.FavoriteBooks)
                .FirstOrDefault(u => u.Id == userId);

            if (user.CartBooks.FirstOrDefault(Fb => Fb.Id == book.Id) is not null)
            {
                return View("~/Views/BookList/Index.cshtml");
            }

            user.CartBooks.Add(book);

            UserViewModel.User = user;

            _dbContext.SaveChanges();

            return View("~/Views/BookList/Index.cshtml");
        }

        [HttpGet]
        public IActionResult deleteBuy(int bookId)
        {
            int userId = _idRetriever.GetLoggedInUserId();

            if (userId == 0)
            {
                return View("~/Views/BookList/Index.cshtml");
            }

            Book book = _dbContext.Books.FirstOrDefault(u => u.Id == bookId);
            User user = _dbContext.Users
                .Include(u => u.CartBooks)
                .Include(u => u.FavoriteBooks)
                .FirstOrDefault(u => u.Id == userId);

            user.CartBooks.Remove(book);

            UserViewModel.User = user;

            _dbContext.SaveChanges();

            return View("~/Views/BookList/Index.cshtml");
        }
    }
}
