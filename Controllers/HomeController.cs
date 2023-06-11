using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WorkProj.Entity;
using WorkProj.Entity.Entity;
using WorkProj.Models;
using WorkProj.Services;

namespace WorkProj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DBContext _dbContext;

        private readonly IDRetriever _idRetriever;

        public HomeController(DBContext dbContext, ILogger<HomeController> logger, IDRetriever idRetriever)
        {
            _dbContext = dbContext;
            _logger = logger;
            _idRetriever = idRetriever;
        }

        [HttpGet]
        public IActionResult Index()
        {
            UserViewModel.Books = _dbContext.Books.ToList();
            int id = _idRetriever.GetLoggedInUserId();
            if (id != 0)
            {
                UserViewModel.User = _dbContext.Users
                    .Include(u => u.FavoriteBooks)
                    .Include(u => u.CartBooks)
                    .FirstOrDefault(u => u.Id == id);
            }
            else
            {
                UserViewModel.User = null;
            }

            return View();
        }

        [HttpPost]
        public IActionResult OfferPropositions(UserViewModel model)
        {
            string email = model.Email;
            string subject = "Персональні пропозиції";
            string body = "Шановний користувач,\r\n\r\n" +
                          "Ми раді повідомити вас про нашу нову можливість - отримання персональних пропозицій! Тепер ви зможете отримувати унікальні пропозиції, створені саме для вас.\r\n\r\n" +
                          "Ми розуміємо, наскільки важливо мати доступ до персоналізованих послуг та пропозицій, тому ми зробили все можливе, щоб надати вам найкращий досвід.\r\n\r\n" +
                          "Ми будемо надсилати вам ексклюзивні пропозиції, спеціальні знижки та інші цікаві пропозиції, які вам точно сподобаються.\r\n\r\n" +
                          "Не пропустіть свою унікальну можливість отримати особливі персональні пропозиції!\r\n\r\nЗ повагою,\r\nКоманда Cozy Library";

            EmailSender.SendEmail(email, subject, body);

            return View("~/Views/Propositions/Index.cshtml");
        }

        [HttpPost]
        public IActionResult SignIn(UserViewModel model)
        {
            if ((!ModelState.TryGetValue("Email", out var email) || !email.Errors.Any()) && (!ModelState.TryGetValue("Password", out var password) || !password.Errors.Any()))
            {
                User user = model.ValidateUser(_dbContext);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties).Wait();

                    _logger.LogInformation("Користувач успішно ввійшов в обліковий запис");

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.isOpened = "True";
            return View("~/Views/Home/Index.cshtml", model);
        }

        [HttpPost]
        public IActionResult SignUp(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _dbContext.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user != null)
                {
                    ModelState.AddModelError("Email", "User with this email already exists.");
                    _logger.LogInformation("Користувач з такою ж елекронною поштою існує");
                    ViewBag.isOpened = "True";
                    return View("~/Views/Home/Index.cshtml", model);
                }
                user = model.ToUser();

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.isOpened = "True";
            return View("~/Views/Home/Index.cshtml", model);
        }

        [HttpGet]
        public IActionResult Favorite(int bookId)
        {
            int userId = _idRetriever.GetLoggedInUserId();
            if (userId == 0)
            {
                return View("~/Views/Home/Index.cshtml");
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

            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public IActionResult Buy(int bookId)
        {
            int userId = _idRetriever.GetLoggedInUserId();
            if (userId == 0)
            {
                return View("~/Views/Home/Index.cshtml");
            }
            Book book = _dbContext.Books.FirstOrDefault(u => u.Id == bookId);
            User user = _dbContext.Users
                .Include(u => u.CartBooks)
                .Include(u => u.FavoriteBooks)
                .FirstOrDefault(u => u.Id == userId);

            if (user.CartBooks.FirstOrDefault(Fb => Fb.Id == book.Id) is not null)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            user.CartBooks.Add(book);

            UserViewModel.User = user;

            _dbContext.SaveChanges();

            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public IActionResult deleteBuy(int bookId)
        {
            int userId = _idRetriever.GetLoggedInUserId();

            if (userId == 0)
            {
                return View("~/Views/Home/Index.cshtml");
            }

            Book book = _dbContext.Books.FirstOrDefault(u => u.Id == bookId);
            User user = _dbContext.Users
                .Include(u => u.CartBooks)
                .Include(u => u.FavoriteBooks)
                .FirstOrDefault(u => u.Id == userId);

            user.CartBooks.Remove(book);

            UserViewModel.User = user;

            _dbContext.SaveChanges();

            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public IActionResult BookList()
        {
            return View("~/Views/BookList/Index.cshtml");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            _logger.LogInformation("Виконування виходу з облікового запису");
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            _logger.LogInformation("Перехід на головну сторінку");
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}