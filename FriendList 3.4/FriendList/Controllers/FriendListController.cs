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
    public class FriendListController : Controller
    {
        private readonly IUserService _userService;
        public FriendListController(IUserService userService)
        {
            _userService = userService;
        }
        /*public IActionResult SearchFriends()
        {
            var user = this.HttpContext.Session.GetString("userId");
            return View(_userService.GetSuggestionList(user).ToList());
        }*/
        public IActionResult SearchFriends()
        {
            return View();
        }
        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var user = this.HttpContext.Session.GetString("userId");
            return Json(_userService.GetSuggestionList(user).ToList().ToDataSourceResult(request));
        }
        public ActionResult SendRequest(int uid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.AddFriend(user, uid);
            return RedirectToAction("SearchFriends","FriendList");
        }
        public ActionResult CancelRequest(int uid, string ReturnUrl, string status)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.CancelRequest(user, uid, status);
            return Redirect(ReturnUrl);
        }
        public ActionResult AcceptRequest(int uid, string ReturnUrl)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.AcceptRequest(user, uid);
            return Redirect(ReturnUrl);
        }
        public IActionResult MyFriends()
        {
            return View();
        }
        public IActionResult MyFriendsRead([DataSourceRequest] DataSourceRequest request)
        {
            var user = this.HttpContext.Session.GetString("userId");
            return Json(_userService.MyFriendList(user).ToList().ToDataSourceResult(request));
        }
        public IActionResult FriendRequest()
        {
            return View();
        }
        public IActionResult FriendRequestRead([DataSourceRequest] DataSourceRequest request)
        {
            var user = this.HttpContext.Session.GetString("userId");
            return Json(_userService.FriendRequestList(user).ToList().ToDataSourceResult(request));
        }
    }
}
