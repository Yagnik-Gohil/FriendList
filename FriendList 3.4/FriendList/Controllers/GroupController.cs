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
    public class GroupController : Controller
    {
        private readonly IUserService _userService;

        public GroupController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult MyGroups()
        {
            var user = this.HttpContext.Session.GetString("userId");
            return View(_userService.GetMyGroups(user));
        }
        public ActionResult CreateGroup(string Title)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.CreateGroup(user, Title);
            return RedirectToAction("MyGroups");
        }
        [Route("/Group/OpenGroup/{gid}")]
        public ActionResult OpenGroup(int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            return View(_userService.OpenGroup(user, gid));
        }
        public ActionResult GroupPost(int gid, IFormFile photo, string Comments) // Create Group Post
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.GroupPost(user, gid, photo, Comments);
            return RedirectToAction("OpenGroup", "Group", new { gid = gid });
        }
        public ActionResult InviteMembers(int gid) // Invite Popup
        {
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewBag.GID = gid;
            var user = this.HttpContext.Session.GetString("userId");
            return PartialView("InvitePopup", _userService.InviteMembers(user, gid));
        }
        public ActionResult SendInvite(int uid, string ReturnUrl, int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.SendInvite(user, uid, gid);
            return Redirect(ReturnUrl);
        }
        public ActionResult CancelInvite(int uid, string ReturnUrl, int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.CancelInvite(user, uid, gid);
            return Redirect(ReturnUrl);
        }
        public ActionResult AcceptInvite(int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.AcceptInvite(user, gid);
            return RedirectToAction("MyGroups");
        }
        public ActionResult RejectInvite(int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.RejectInvite(user, gid);
            return RedirectToAction("MyGroups");
        }
        public ActionResult SearchGroup()
        {
            var user = this.HttpContext.Session.GetString("userId");
            return View(_userService.SearchGroup(user));
        }
        public ActionResult SendJoinRequest(int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.SendJoinRequest(user, gid);
            return RedirectToAction("SearchGroup");
        }
        public ActionResult CancelJoinRequest(int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.CancelJoinRequest(user, gid);
            return RedirectToAction("SearchGroup");
        }
        public ActionResult AcceptJoinRequest(int uid, int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.AcceptJoinRequest(user, uid, gid);
            return RedirectToAction("SearchGroup");
        }
        public ActionResult RejectJoinRequest(int uid, int gid)
        {
            var user = this.HttpContext.Session.GetString("userId");
            _userService.RejectJoinRequest(user, uid, gid);
            return RedirectToAction("SearchGroup");
        }
    }
}
