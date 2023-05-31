using mvctest1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvctest1.Controllers
{
    public class HomeController : Controller
    {
        // создаем контекст данных
        BookContext db = new BookContext();

        public ActionResult Index()
        {
            // получаем из бд все объекты Book
            IEnumerable<Book> books = db.Books;
            // передаем все объекты в динамическое свойство Books в ViewBag
            ViewBag.Books = books;
            // возвращаем представление
            return View();
        }

        public ActionResult AdminPanel()
        {
            IEnumerable<Purchase> purchases = db.Purchases;

            ViewBag.Purchases = purchases;
            return View();
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            ViewBag.BookId = id;
            return View();
        }
        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!";
        }

        //добавление книг
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book model)
        {
            if (ModelState.IsValid)
            {
                // Add the new entity to the database
                db.Books.Add(model);
                db.SaveChanges();

                // Redirect to the "index" action (list of all books)
                return RedirectToAction("Index");
            }

            // If there were validation errors, display the form again
            return View(model);
        }

        
    }
}