using DataAccessLayer.Entities;
using DataAccessLayer.Model;
using FriendList.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using LogicLayer.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FriendList.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserTable model)
        {
            _userService.CreateUser(model);
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult Login(UserTable model)
        {
            bool userdetails = _userService.IsValidUser(model);
            if (userdetails == false)
            {
                ModelState.AddModelError("Password", "Invalid login attempt.");
                return View(model);
            }
            HttpContext.Session.SetString("userId", model.Email);

            return RedirectToAction("FeedPosts");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult MyProfile()
        {
            var user = this.HttpContext.Session.GetString("userId");
            return View(_userService.MyProfile(user));
        }
        public ActionResult MyProfileRead([DataSourceRequest] DataSourceRequest request)
        {
            var user = this.HttpContext.Session.GetString("userId");
            return Json(_userService.MyProfile(user).PostModel.ToList().ToDataSourceResult(request));
        }
        [HttpGet]
        public ActionResult CreatePost()
        {
            return View();
        }
        public ActionResult CreatePost([DataSourceRequest] DataSourceRequest request, IFormFile photo, PostTable model)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.CreatePost(user, photo, model);
            return RedirectToAction("MyProfile");
        }
        /*public ActionResult CreatePost(string post)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.CreatePost(user, post);
            return RedirectToAction("MyProfile");
        }*/
        public ActionResult FeedPosts()
        {
            return View();
        }
        public ActionResult FeedPostsRead([DataSourceRequest] DataSourceRequest request)
        {
            var user = this.HttpContext.Session.GetString("userId");
            return Json(_userService.MyFeedPosts(user).ToList().ToDataSourceResult(request));
        }
        public ActionResult AddComment(int pid, string Comments)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.AddComment(user, pid, Comments);
            return RedirectToAction("FeedPosts");
        }
    }
}
