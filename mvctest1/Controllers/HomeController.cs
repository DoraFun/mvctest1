using mvctest1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            IEnumerable<Book> books = db.Books;

            ViewBag.Books = books;

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

        //Редактирование книг
        [HttpGet]
        public ActionResult Edit(int id)
        {
            // Get the book from the database by id
            Book book = db.Books.Find(id);

            // Check if book exists
            if (book == null)
            {
                return HttpNotFound(); // Return 404 error
            }

            // Pass all fields of the book to the view using ViewBag
            ViewBag.Id = book.Id;
            ViewBag.Title = book.Name;
            ViewBag.Author = book.Author;
            ViewBag.Price = book.Price;

            return View();
        }

        [HttpPost]
        public ActionResult Edit(Book model)
        {
            if (ModelState.IsValid)
            {
                // eddit entity
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            // If there were validation errors, display the form again
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            // Get the book from the database by id
            Book book = db.Books.Find(id);

            // Check if book exists
            if (book == null)
            {
                return HttpNotFound(); // Return 404 error
            }

            // Pass all fields of the book to the view using ViewBag

            return View();
        }

        [HttpPost]
        public ActionResult Delete(Book model)
        {
            if (ModelState.IsValid)
            {
                // delete entry

                db.Entry(model).State = EntityState.Deleted;
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            // If there were validation errors, display the form again
            return View(model);
        }
    }
}