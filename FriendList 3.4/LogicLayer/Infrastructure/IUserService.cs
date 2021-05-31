using DataAccessLayer.Entities;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer.Infrastructure
{
    public interface IUserService
    {
        void CreateUser(UserTable model);
        bool IsValidUser(UserTable model);
        List<UserTable> GetSuggestionList(string user);
        void AddFriend(string user, int uid);
        void CancelRequest(string user, int uid, string status);
        void AcceptRequest(string user, int uid);
        List<UserTable> MyFriendList(string user); 
        List<UserTable> FriendRequestList(string user);
        UserProfile MyProfile(string user);
        void CreatePost(string user, IFormFile photo, PostTable model);
        List<PostModel> MyFeedPosts(string user);
        void AddComment(string user, int pid, string Comments);
        void CreateGroup(string user, string Title);
        MyGroupsModel GetMyGroups(string user);
        GroupModel OpenGroup(string user, int gid);
        void GroupPost(string user, int gid, IFormFile photo, string Comments);
        List<UserTable> InviteMembers(string user, int gid);
        void SendInvite(string user, int uid, int gid);
        void CancelInvite(string user, int uid, int gid);
        void AcceptInvite(string user, int gid);
        void RejectInvite(string user, int gid);
        List<SearchGroupModel> SearchGroup(string user);
        void SendJoinRequest(string user, int gid);
        void CancelJoinRequest(string user, int gid);
        void AcceptJoinRequest(string user, int uid, int gid);
        void RejectJoinRequest(string user, int uid, int gid);
    }
}
