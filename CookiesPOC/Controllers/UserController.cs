using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CookiesPOC.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        // Handling Login POST request
        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            if (username == "admin" && password == "password")
            {
                // Set session for the logged-in user
                Session["Username"] = username; //so basically session is server side and once you close the browser, it disposes off everything

                // Set cookie if "Remember Me" option is checked
                if (Request["RememberMe"] != null)
                {
                    HttpCookie cookie = new HttpCookie("UserLogin");
                    cookie.Value = username;
                    cookie.Expires = DateTime.Now.AddDays(7);
                    Response.Cookies.Add(cookie);
                }

                return RedirectToAction("Dashboard");
            }

            ViewBag.Message = "Invalid credentials!";
            return View();
        }

        // Dashboard - Only accessible if logged in
        public ActionResult Dashboard()
        {
            if (Session["Username"] != null)
            {
                ViewBag.Username = Session["Username"].ToString();
                return View();
            }

            return RedirectToAction("Login");
        }

        // Logout Action
        public ActionResult Logout()
        {
            Session.Abandon();

            if (Request.Cookies["UserLogin"] != null)
            {
                var cookie = new HttpCookie("UserLogin") //here cookie is CLient side which means, when i will check REMEMBER ME
                                                         //a cookie will be created and, later we can see in the cookie tab in inspect
                                                          
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");
        }
    }

}