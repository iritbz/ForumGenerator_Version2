﻿using ForumGenerator_Version2_Server.ForumData;
using ForumGenerator_Version2_Server.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ForumGenerator_Version2_Server.Sys
{
    public interface IForumGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forumId"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns>The User object of the loggedin user</returns>
        User login(int forumId, string userName, string password);

        bool logout(int forumId, int userId);

        SuperUser superUserLogin(string userName, string password);

        bool superUserLogout();

        User register(int forumId, string userName, string password, string email, string signature);

        List<Forum> getForums();

        List<SubForum> getSubForums(int forumId);

        List<Discussion> getDiscussions(int forumId, int subForumId);

        List<Comment> getComments(int forumId, int subForumId, int discussionId);

        List<User> getUsers(int forumId);

        Forum createNewForum(string userName, string password, string forumName, string adminUserName, string adminPassword);

        SubForum createNewSubForum(string userName, string password, int forumId, string subForumTitle);

        Discussion createNewDiscussion(string userName, string password, int forumId, int subForumId, string title, string content);

        Comment createNewComment(string userName, string password, int forumId, int subForumId, int discussionId, string content);

        User changeAdmin(string userName, string password, int forumId, int newAdminUserId);


    }
}