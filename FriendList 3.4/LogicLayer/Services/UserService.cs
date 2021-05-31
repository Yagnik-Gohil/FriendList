using DataAccessLayer.Entities;
using DataAccessLayer.Model;
using DataAccessLayer.Repository;
using LogicLayer.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repo;

        public UserService(IRepository repo)
        {
            _repo = repo;
        }
        public void CreateUser(UserTable model)
        {
            _repo.Create(model);
        }
        public bool IsValidUser(UserTable model)
        {
            return _repo.IsValidUser(model);
        }
        public List<UserTable> GetSuggestionList(string user)
        {
            return _repo.GetSuggestionList(user);
        }
        public void AddFriend(string user, int uid)
        {
            _repo.AddFriend(user, uid);
        }
        public void CancelRequest(string user, int uid, string status)
        {
            _repo.CancelRequest(user, uid, status);
        }
        public void AcceptRequest(string user, int uid)
        {
            _repo.AcceptRequest(user, uid);
        }
        public List<UserTable> MyFriendList(string user)
        {
            return _repo.MyFriendList(user);
        }
        public List<UserTable> FriendRequestList(string user)
        {
            return _repo.FriendRequestList(user);
        }
        public UserProfile MyProfile(string user)
        {
            return _repo.MyProfile(user);
        }
        public void CreatePost(string user, IFormFile photo, PostTable model)
        {
            _repo.CreatePost(user, photo, model);
        }
        public List<PostModel> MyFeedPosts(string user)
        {
            return _repo.MyFeedPosts(user);
        }
        public void AddComment(string user, int pid, string Comments)
        {
            _repo.AddComment(user, pid, Comments);
        }
        public void CreateGroup(string user, string Title)
        {
            _repo.CreateGroup(user, Title);
        }
        public MyGroupsModel GetMyGroups(string user)
        {
            return _repo.GetMyGroups(user);
        }
        public GroupModel OpenGroup(string user, int gid)
        {
            return _repo.OpenGroup(user, gid);
        }
        public void GroupPost(string user, int gid, IFormFile photo, string Comments)
        {
            _repo.GroupPost(user, gid, photo, Comments);
        }
        public List<UserTable> InviteMembers(string user, int gid)
        {
            return _repo.InviteMembers(user, gid);
        }
        public void SendInvite(string user, int uid, int gid)
        {
            _repo.SendInvite(user, uid, gid);
        }
        public void CancelInvite(string user, int uid, int gid)
        {
            _repo.CancelInvite(user, uid, gid);
        }
        public void AcceptInvite(string user, int gid)
        {
            _repo.AcceptInvite(user, gid);
        }
        public void RejectInvite(string user, int gid)
        {
            _repo.RejectInvite(user, gid);
        }
        public List<SearchGroupModel> SearchGroup(string user)
        {
            return _repo.SearchGroup(user);
        }
        public void SendJoinRequest(string user, int gid)
        {
            _repo.SendJoinRequest(user, gid);
        }
        public void CancelJoinRequest(string user, int gid)
        {
            _repo.CancelJoinRequest(user, gid);
        }
        public void AcceptJoinRequest(string user, int uid, int gid)
        {
            _repo.AcceptJoinRequest(user, uid, gid);
        }
        public void RejectJoinRequest(string user, int uid, int gid)
        {
            _repo.RejectJoinRequest(user, uid, gid);
        }
    }
}
