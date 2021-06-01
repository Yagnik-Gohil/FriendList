using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Repository
{
    public class Repository : IRepository
    {
        private readonly DatabaseContext dbObj;
        public Repository(DatabaseContext dbObj)
        {
            this.dbObj = dbObj;
        }
        public void Create(UserTable model)
        {
            try
            {
                dbObj.Add(model);
                dbObj.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public bool IsValidUser(UserTable model)
        {
            var user = dbObj.UserTable.SingleOrDefault(m => m.Email == model.Email && m.Password == model.Password);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public List<UserTable> GetSuggestionList(string email)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var allUser = dbObj.UserTable.Where(x => x.Email != email).ToList();

            var request = dbObj.StatusTable.Where(x => x.RequestBy == currentUser.UID || x.RequestTo == currentUser.UID);

            if(request == null)
            {
                var suggestionList = new List<UserTable>();
                foreach (var item in allUser)
                {
                    var user = dbObj.UserTable.Where(x => x.UID == item.UID).FirstOrDefault();
                    suggestionList.Add(new UserTable()
                    {
                        UID = item.UID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Status = "unknown"
                    });

                }

                return suggestionList;
            }
            else
            {
                var suggestionList = new List<UserTable>();
                foreach (var item in allUser)
                {
                    var user = dbObj.StatusTable.Where(x => x.RequestBy == currentUser.UID && x.RequestTo == item.UID).FirstOrDefault();
                    if(user != null)
                    {
                        if(user.Status == "friends")
                        {
                            continue;
                        }
                        suggestionList.Add(new UserTable()
                        {
                            UID = item.UID,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Status = "requestedByYou"
                        });
                        continue;
                    }
                    var user2 = dbObj.StatusTable.Where(x => x.RequestTo == currentUser.UID && x.RequestBy == item.UID).FirstOrDefault();
                    if (user2 != null)
                    {
                        if (user2.Status == "friends")
                        {
                            continue;
                        }
                        suggestionList.Add(new UserTable()
                        {
                            UID = item.UID,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Status = "requestedToYou"
                        });
                        continue;
                    }
                    suggestionList.Add(new UserTable()
                    {
                        UID = item.UID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Status = item.Status
                    });

                }
                return suggestionList;
            }
            
        }
        public void AddFriend(string email, int uid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();
            StatusTable request = new StatusTable();
            request.RequestBy = currentUser.UID;
            request.RequestTo = uid;
            request.Status = "requested";

            dbObj.Add(request);
            dbObj.SaveChanges();
        }
        public void CancelRequest(string email, int uid, string status)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();
            if(status == "reject")
            {
                StatusTable request = dbObj.StatusTable.Where(x => x.RequestBy == uid && x.RequestTo == currentUser.UID).FirstOrDefault();
                dbObj.StatusTable.Remove(request);
            }
            else
            {
                StatusTable request = dbObj.StatusTable.Where(x => x.RequestBy == currentUser.UID && x.RequestTo == uid).FirstOrDefault();
                dbObj.StatusTable.Remove(request);
            }

            dbObj.SaveChanges();
        }
        public void AcceptRequest(string email, int uid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();
            StatusTable request = dbObj.StatusTable.Where(x => x.RequestBy == uid && x.RequestTo == currentUser.UID).FirstOrDefault();
            request.Status = "friends";

            dbObj.Entry(request).State = EntityState.Modified;
            dbObj.SaveChanges();
        }
        public List<UserTable> MyFriendList(string email)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var friendsStatusList = dbObj.StatusTable.Where(x => (x.RequestBy == currentUser.UID || x.RequestTo == currentUser.UID) && x.Status == "friends").ToList();
            var myFriendList = new List<UserTable>();
            foreach (var item in friendsStatusList)
            {
                if(item.RequestBy == currentUser.UID)
                {
                    UserTable friend = dbObj.UserTable.Where(x => x.UID == item.RequestTo).FirstOrDefault();
                    myFriendList.Add(friend);
                }
                else
                {
                    UserTable friend = dbObj.UserTable.Where(x => x.UID == item.RequestBy).FirstOrDefault();
                    myFriendList.Add(friend);
                }
            }
            return myFriendList.ToList();
        }
        public List<UserTable> FriendRequestList(string email)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var friendsStatusList = dbObj.StatusTable.Where(x => x.RequestTo == currentUser.UID && x.Status == "requested").ToList();
            var myFriendRequestList = new List<UserTable>();
            foreach (var item in friendsStatusList)
            {
                var requestBy = dbObj.UserTable.Where(x => x.UID == item.RequestBy).FirstOrDefault();
                myFriendRequestList.Add(requestBy);
            }
            return myFriendRequestList.ToList();
        }
        public UserProfile MyProfile(string email)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();
            /*var postList = dbObj.PostTable.Where(x => x.PostedBy == currentUser.UID).ToList();*/

            UserProfile MyProfile = new UserProfile();

            MyProfile.UserTable = currentUser;

            MyProfile.PostModel = getMyPosts(currentUser.UID);

            MyProfile.TotalFriends = dbObj.StatusTable.Where(x => (x.RequestBy == currentUser.UID || x.RequestTo == currentUser.UID) && x.Status == "friends").Count();
            MyProfile.FriendRequests = dbObj.StatusTable.Where(x => x.RequestTo == currentUser.UID && x.Status == "requested").Count();
            MyProfile.ProfilePicture = currentUser.ProfilePicture == null ? "Default/User.jpg" : currentUser.ProfilePicture;
            
            return MyProfile;
        }
        List<PostModel> getMyPosts(int uid)
        {
            var myPosts = dbObj.PostTable.Where(x => x.PostedBy == uid).ToList();

            var PostList = new List<PostModel>();
            foreach (var item in myPosts)
            {
                PostModel model = new PostModel();
                model.PID = item.PID;
                model.UserName = dbObj.UserTable.Where(x => x.UID == item.PostedBy).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                model.Description = item.Description;
                model.CreatedDate = item.CreatedDate;
                model.ImagePath = item.ImagePath;
                model.CommentModel = getCommentList(item.PID);
                PostList.Add(model);
            }
            return PostList.OrderByDescending(x => x.CreatedDate).ToList();
        }
        public void CreatePost(string email, IFormFile photo, PostTable model)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();
            /*string path = Path.Combine(Directory.GetCurrentDirectory(),"Members", currentUser.UID.ToString());*/
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Members", currentUser.UID.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (model.PID == 0) // New Post
            {
                PostTable Post = new PostTable();
                Post.PostedBy = currentUser.UID;
                Post.Description = model.Description;
                Post.CreatedDate = DateTime.Now;

                dbObj.Add(Post);
                dbObj.SaveChanges();

                if(photo != null)
                {
                    var PostID = Post.PID;
                    string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Members", currentUser.UID.ToString(), PostID.ToString() + Path.GetExtension(photo.FileName));
                    var stream = new FileStream(imagepath, FileMode.Create);
                    photo.CopyTo(stream);

                    Post.ImagePath = Path.Combine("Members/" + currentUser.UID + "/" + PostID + Path.GetExtension(photo.FileName));
                    dbObj.SaveChanges();
                }
            }
            else // Edit Post
            {
                PostTable Post = dbObj.PostTable.Where(x=>x.PID == model.PID).FirstOrDefault();
                Post.Description = model.Description;

                dbObj.Entry(Post).State = EntityState.Modified;
                dbObj.SaveChanges();
            }
            
        }
        /*public void CreatePost(string email, string post)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            PostTable Post = new PostTable();
            Post.PostedBy = currentUser.UID;
            Post.Description = post;
            Post.CreatedDate = DateTime.Now;

            dbObj.Add(Post);
            dbObj.SaveChanges();
        }*/
        public List<PostModel> MyFeedPosts(string email)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var allPosts = dbObj.PostTable.Where(x => x.PostedBy != currentUser.UID).ToList();

            var PostList = new List<PostModel>();
            foreach (var item in allPosts)
            {
                if (IsFriend(currentUser.UID,item.PostedBy))
                {
                    /*var commentList = dbObj.CommentTable.Where(x => x.PID == item.PID).ToList();*/

                    PostModel model = new PostModel();
                    model.PID = item.PID;
                    model.UserName = dbObj.UserTable.Where(x => x.UID == item.PostedBy).Select(x => x.FirstName + " " + x.LastName).FirstOrDefault();
                    model.Description = item.Description;
                    model.CreatedDate = item.CreatedDate;
                    model.ImagePath = item.ImagePath;
                    model.CommentModel = getCommentList(item.PID);
                    PostList.Add(model);
                }
            }
            return PostList.OrderByDescending(x => x.CreatedDate).ToList();
        }
        List<CommentModel> getCommentList(int pid)
        {
            var allComments = dbObj.CommentTable.ToList();
            List<CommentModel> commentList = new List<CommentModel>();
            foreach (var item in allComments)
            {
                if (item.PID == pid)
                {
                    var commentByUser = dbObj.UserTable.Where(x => x.UID == item.CommentBy).FirstOrDefault();

                    CommentModel model = new CommentModel();
                    model.CID = item.CID;
                    model.UserName = commentByUser.FirstName + " " + commentByUser.LastName;
                    model.Comment = item.Comment;
                    commentList.Add(model);
                }
            }
            return commentList;
        }
        bool IsFriend(int user1,int user2)
        {
            var status = dbObj.StatusTable.Where(x => x.RequestBy == user1 && x.RequestTo == user2 && x.Status == "friends").FirstOrDefault();
            if(status != null)
            {
                return true;
            }
            else
            {
                status = dbObj.StatusTable.Where(x => x.RequestBy == user2 && x.RequestTo == user1 && x.Status == "friends").FirstOrDefault();
                if(status != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public void AddComment(string email, int pid, string Comments)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            CommentTable comment = new CommentTable();
            comment.PID = pid;
            comment.Comment = Comments;
            comment.CommentBy = currentUser.UID;

            dbObj.Add(comment);
            dbObj.SaveChanges();
        }
        public void CreateGroup(string email, string Title)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupTable group = new GroupTable();
            group.GroupName = Title;
            group.GroupAdmin = currentUser.UID;
            group.CreatedDate = DateTime.Now;

            dbObj.Add(group);
            dbObj.SaveChanges();

            GroupManagerTable gm = new GroupManagerTable();
            gm.UserID = currentUser.UID;
            gm.GroupID = group.GID;
            gm.IsAdmin = true;
            gm.IsMember = true;
            gm.InvitedBy = currentUser.UID;

            dbObj.Add(gm);
            dbObj.SaveChanges();
        }
        /*public List<GroupTable> GetMyGroups(string email)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var myGroups = dbObj.GroupManagerTable.Where(x => x.UserID == currentUser.UID && x.IsMember == true).ToList();
            var GroupList = new List<GroupTable>();

            foreach (var item in myGroups)
            {
                GroupTable group = dbObj.GroupTable.Where(x => x.GID == item.GroupID).FirstOrDefault();
                GroupList.Add(group);
            }
            return GroupList.ToList();
        }*/
        public MyGroupsModel GetMyGroups(string email)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var myGroups = dbObj.GroupManagerTable.Where(x => x.UserID == currentUser.UID && x.IsMember == true).ToList();
            var GroupList = new List<GroupTable>();

            foreach (var item in myGroups)
            {
                GroupTable group = dbObj.GroupTable.Where(x => x.GID == item.GroupID).FirstOrDefault();
                GroupList.Add(group);
            }
            var myInvites = dbObj.GroupManagerTable.Where(x => x.UserID == currentUser.UID && x.InvitedBy != currentUser.UID && x.IsMember == false).ToList();
            var InviteList = new List<GroupTable>();

            foreach (var item in myInvites)
            {
                GroupTable group = dbObj.GroupTable.Where(x => x.GID == item.GroupID).FirstOrDefault();
                InviteList.Add(group);
            }

            MyGroupsModel model = new MyGroupsModel();
            model.MyGroups = GroupList.ToList();
            model.GroupInvites = InviteList.ToList();

            return model;

        }
        public GroupModel OpenGroup(string email, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();
            GroupTable group = dbObj.GroupTable.Where(x => x.GID == gid).FirstOrDefault();
            GroupManagerTable myDetails = dbObj.GroupManagerTable.Where(x => x.GroupID == gid && x.UserID == currentUser.UID).FirstOrDefault();
            var allMemberDetails = dbObj.GroupManagerTable.Where(x => x.GroupID == gid && x.IsMember).ToList();

            GroupModel model = new GroupModel();
            model.GID = gid;
            model.AdminID = group.GroupAdmin;
            model.GroupName = group.GroupName;
            model.IsAdmin = myDetails.IsAdmin ? true : false;

            var GroupMembers = new List<UserTable>();
            foreach (var item in allMemberDetails)
            {
                UserTable member = dbObj.UserTable.Where(x => x.UID == item.UserID).FirstOrDefault();
                GroupMembers.Add(member);
            }

            var allPost = dbObj.GroupPostTable.Where(x => x.PostedIn == gid).ToList();
            var GroupPosts = new List<GroupPostModel>();
            foreach (var item in allPost)
            {
                UserTable postedBy = dbObj.UserTable.Where(x => x.UID == item.PostedBy).FirstOrDefault();
                GroupPostModel post = new GroupPostModel();
                post.GPID = item.GPID;
                post.UserName = postedBy.FirstName + " " + postedBy.LastName;
                post.Description = item.Description;
                post.ImagePath = item.ImagePath;
                post.CreatedDate = item.CreatedDate;

                GroupPosts.Add(post);
            }

            model.UserTable = GroupMembers.ToList();
            model.GroupPostModel = GroupPosts.ToList();

            return model;
        }
        public void GroupPost(string email, int gid, IFormFile photo, string Comments)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Groups", gid.ToString(), currentUser.UID.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            GroupPostTable GPost = new GroupPostTable();
            GPost.PostedIn = gid;
            GPost.PostedBy = currentUser.UID;
            GPost.Description = Comments;
            GPost.CreatedDate = DateTime.Now;

            dbObj.Add(GPost);
            dbObj.SaveChanges();

            if (photo != null)
            {
                var PostID = GPost.GPID;
                string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Groups", gid.ToString(), currentUser.UID.ToString(), PostID.ToString() + Path.GetExtension(photo.FileName));
                var stream = new FileStream(imagepath, FileMode.Create);
                photo.CopyTo(stream);

                GPost.ImagePath = Path.Combine("Groups/" + gid + "/" + currentUser.UID + "/" + PostID + Path.GetExtension(photo.FileName));
                dbObj.SaveChanges();
            }
        }
        /*public List<UserTable> InviteMembers(string email, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var allUser = dbObj.UserTable.Where(x => x.Email != email).ToList();

            var invites = dbObj.GroupManagerTable.Where(x => x.UserID != currentUser.UID && x.InvitedBy == currentUser.UID).ToList().Count();

            if (invites == 0)
            {
                var membersList = new List<UserTable>();
                foreach (var item in allUser)
                {
                    if (IsFriend(currentUser.UID,item.UID))
                    {
                        var user = dbObj.UserTable.Where(x => x.UID == item.UID).FirstOrDefault();
                        membersList.Add(new UserTable()
                        {
                            UID = item.UID,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Status = "unknown"
                        });
                    }

                }

                return membersList.ToList();
            }
            else
            {
                var membersList = new List<UserTable>();
                foreach (var item in allUser)
                {
                    var user = dbObj.GroupManagerTable.Where(x => x.InvitedBy == currentUser.UID && x.UserID == item.UID).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.IsMember)
                        {
                            continue;
                        }
                        membersList.Add(new UserTable()
                        {
                            UID = item.UID,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Status = "Invited"
                        });
                        continue;
                    }
                    membersList.Add(new UserTable()
                    {
                        UID = item.UID,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Status = "unknown"
                    });

                }
                return membersList.ToList();
            }
        }*/
        public List<UserTable> InviteMembers(string email, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var allUser = dbObj.UserTable.Where(x => x.Email != email).ToList();

            var membersList = new List<UserTable>();
            foreach (var item in allUser)
            {
                var user = dbObj.GroupManagerTable.Where(x => x.UserID == item.UID && x.GroupID == gid).FirstOrDefault();
                if (user != null)
                {
                    if (user.InvitedBy == currentUser.UID)
                    {
                        if (user.IsMember)
                        {
                            continue;
                        }
                        membersList.Add(new UserTable()
                        {
                            UID = item.UID,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Status = "Invited"
                        });
                        continue;
                    }
                    else if (user.InvitedBy == item.UID)
                    {
                        if (user.IsMember)
                        {
                            continue;
                        }
                        membersList.Add(new UserTable()
                        {
                            UID = item.UID,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Status = "wantedtojoin"
                        });
                        continue;
                    }

                }
                membersList.Add(new UserTable()
                {
                    UID = item.UID,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Status = "unknown"
                });

            }
            return membersList.ToList();
        }
        public void SendInvite(string email, int uid, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupManagerTable gm = new GroupManagerTable();
            gm.UserID = uid;
            gm.GroupID = gid;
            gm.IsAdmin = false;
            gm.IsMember = false;
            gm.InvitedBy = currentUser.UID;

            dbObj.Add(gm);
            dbObj.SaveChanges();
        }
        public void CancelInvite(string email, int uid, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupManagerTable gm = dbObj.GroupManagerTable.Where(x => x.UserID == uid && x.GroupID == gid && x.InvitedBy == currentUser.UID).FirstOrDefault();

            dbObj.Remove(gm);
            dbObj.SaveChanges();
        }
        public void AcceptInvite(string email, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupManagerTable gm = dbObj.GroupManagerTable.Where(x => x.UserID == currentUser.UID && x.GroupID == gid).FirstOrDefault();
            gm.IsMember = true;

            dbObj.Entry(gm).State = EntityState.Modified;
            dbObj.SaveChanges();
        }
        public void RejectInvite(string email, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupManagerTable gm = dbObj.GroupManagerTable.Where(x => x.UserID == currentUser.UID && x.GroupID == gid).FirstOrDefault();

            dbObj.Remove(gm);
            dbObj.SaveChanges();
        }
        public List<SearchGroupModel> SearchGroup(string email)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            var allGroups = dbObj.GroupTable.ToList();
            var SearchList = new List<SearchGroupModel>();

            foreach (var item in allGroups)
            {
                GroupManagerTable gm = dbObj.GroupManagerTable.Where(x => x.UserID == currentUser.UID && x.GroupID == item.GID).FirstOrDefault();
                if (gm == null)
                {
                    SearchGroupModel sgm = new SearchGroupModel();
                    sgm.GID = item.GID;
                    sgm.GroupName = item.GroupName;
                    sgm.Status = "unknown";

                    SearchList.Add(sgm);
                }
                else if(gm.InvitedBy == currentUser.UID && gm.IsMember == false)
                {
                    SearchGroupModel sgm = new SearchGroupModel();
                    sgm.GID = item.GID;
                    sgm.GroupName = item.GroupName;
                    sgm.Status = "requested";

                    SearchList.Add(sgm);
                }
            }

            return SearchList.ToList();
        }
        public void SendJoinRequest(string email, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupManagerTable gm = new GroupManagerTable();
            gm.UserID = currentUser.UID;
            gm.GroupID = gid;
            gm.IsAdmin = false;
            gm.IsMember = false;
            gm.InvitedBy = currentUser.UID;

            dbObj.Add(gm);
            dbObj.SaveChanges();
        }
        public void CancelJoinRequest(string email, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupManagerTable gm = dbObj.GroupManagerTable.Where(x => x.UserID == currentUser.UID && x.GroupID == gid).FirstOrDefault();

            dbObj.Remove(gm);
            dbObj.SaveChanges();
        }
        public void AcceptJoinRequest(string email, int uid, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupManagerTable gm = dbObj.GroupManagerTable.Where(x => x.UserID == uid && x.GroupID == gid).FirstOrDefault();
            gm.IsMember = true;

            dbObj.Entry(gm).State = EntityState.Modified;
            dbObj.SaveChanges();
        }
        public void RejectJoinRequest(string email, int uid, int gid)
        {
            UserTable currentUser = dbObj.UserTable.Where(x => x.Email == email).FirstOrDefault();

            GroupManagerTable gm = dbObj.GroupManagerTable.Where(x => x.UserID == uid && x.GroupID == gid).FirstOrDefault();

            dbObj.Remove(gm);
            dbObj.SaveChanges();
        }
    }
}
