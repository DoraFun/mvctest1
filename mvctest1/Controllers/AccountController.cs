using mvctest1.Models;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace mvctest1.Controllers
{
    public class AccountController : Controller
    {
            public ActionResult Login()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Login(LoginModel model)
            {
                if (ModelState.IsValid)
                {
                    // поиск пользователя в бд
                    User user = null;
                    using (BookContext db = new BookContext())
                    {
                        user = db.Users.FirstOrDefault(u => u.Name == model.Name && u.Password == model.Password);

                }
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                    }
                }

                return View(model);
            }

            public ActionResult Register()
            {
                return View();
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Register(RegisterModel model)
            {
                if (ModelState.IsValid)
                {
                    User user = null;
                    using (BookContext db = new BookContext())
                    {
                        user = db.Users.FirstOrDefault(u => u.Name == model.Name);
                    }
                    if (user == null)
                    {
                        // создаем нового пользователя
                        using (BookContext db = new BookContext())
                        {
                            db.Users.Add(new User { Name = model.Name, Password = model.Password});
                            db.SaveChanges();

                            user = db.Users.Where(u => u.Name == model.Name && u.Password == model.Password).FirstOrDefault();
                        }
                        // если пользователь удачно добавлен в бд
                        if (user != null)
                        {
                            FormsAuthentication.SetAuthCookie(model.Name, true);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                    }
                }

                return View(model);
            }
            public ActionResult Logoff()
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
        
    }
}