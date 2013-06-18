﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ForumGenerator_Version2_Server.ForumData;
using ForumGenerator_Version2_Server.Users;
using ForumGenerator_Version2_Server.Sys;
using ForumGenerator_Version2_Server.Sys.Exceptions;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using ForumGenerator_Version2_Server.DataLayer;

namespace ForumGenerator_Version2_Server.ForumData
{
    [DataContract(IsReference = true)]
    public class SubForum
    {
        [DataMember]
        [Key]
        public int subForumId { get; private set; }
        [DataMember]
        public string subForumTitle { get; private set; }
        [IgnoreDataMember]
        public virtual List<User> moderators { get; private set; }
        [IgnoreDataMember]
        public virtual List<Discussion> discussions { get; private set; }
        [IgnoreDataMember]
        public virtual Forum parentForum { get; private set; }


        public SubForum(string subForumTitle, Forum parentForun)
        {
            this.subForumTitle = subForumTitle;
            this.moderators = new List<User>();
            this.parentForum = parentForun;
            User u = this.parentForum.admin;
            this.moderators.Add(parentForum.admin);
            this.discussions = new List<Discussion>();
        }

        public SubForum() { }

        public SubForum(int subForumId, string subForumTitle)
        {
            this.subForumId = subForumId;
            this.subForumTitle = subForumTitle;
        }

        internal Discussion createNewDiscussion(string title, string content, User user, ForumGeneratorContext db)
        {
            Discussion newDiscussion = new Discussion(title, content, user, this);
            this.discussions.Add(newDiscussion);
            db.Discussions.Add(newDiscussion);
            db.SaveChanges();
            return newDiscussion;
        }

        internal Discussion getDiscussion(int discussionId)
        {
            try
            {
                return discussions.Find(delegate (Discussion d) { return d.discussionId == discussionId; });
            }
            catch (ArgumentNullException)
            {
                throw new DiscussionNotFoundException(ForumGeneratorDefs.DISCUSSION_NF);
            }
        }


        internal Discussion removeDiscussion(int discussionId, ForumGeneratorContext db)
        {
            Discussion d = this.getDiscussion(discussionId);
            this.discussions.Remove(d);
            db.Discussions.Remove(d);
            db.SaveChanges();
            return d;
        }


        public User getModerator(string userName)
        {
            try
            {
                return this.moderators.Find(
                    delegate(User mem) { return mem.userName == userName; });
            }
            catch (ArgumentNullException)
            {
                throw new UserNotFoundException(ForumGeneratorDefs.USER_NF);
            }
        }


        public Boolean addModerator(string modUserName, ForumGeneratorContext db)
        {
            // check if user is registered to the forum
            User newModerator = parentForum.getUser(modUserName);
   
            try
            {
                // check if user is already a moderator of this subforum
                this.getModerator(modUserName);
                throw new UnauthorizedOperationException(ForumGeneratorDefs.EXIST_MODERATOR);
            }
            catch (UserNotFoundException)
            {
                this.moderators.Add(newModerator);
                db.Entry(db.SubForums.Find(this)).CurrentValues.SetValues(this);
                db.SaveChanges();
                return true;
            }
        }


        internal Boolean removeModerator(string modUserName, ForumGeneratorContext db)
        {
            if (parentForum.admin.userName == modUserName)     // not allowed
            {
                throw new UnauthorizedOperationException(ForumGeneratorDefs.F_ADMIN_S_MOD);
            }

            User moderator = parentForum.getUser(modUserName);
            bool ans = moderators.Remove(moderator);
            db.Entry(db.SubForums.Find(this)).CurrentValues.SetValues(this);
            db.SaveChanges();
            return ans;
        }


        internal int getNumOfCommentsSingleUser(User user)
        {
            int result = 0;
            foreach (Discussion d in discussions)
            {
                result += d.getNumOfCommentsSingleUser(user);
            }
            return result;
        }


        internal List<User> getResponsersForSingleUser(User user)
        {
            List<User> responsers = new List<User>();
            foreach (Discussion d in discussions)
            {
                responsers.AddRange(d.getResponsersForSingleUser(user));
            }
            return responsers;
        }


        internal int getNumOfComments()
        {
            int result = 0;

            foreach (Discussion d in discussions)
            {
                // a discussion's content is considered as a comment
                result += d.getNumOfComments() + 1;
            }
            return result;
        }


        /**
         * This method only checks if the user is a moderator. 
         * If not - it returns ForumGenerator.MEMBER
         */ 
        public int getUserType(string userName)
        {
            try
            {
                User user = getModerator(userName);
                return (int)ForumGenerator.userTypes.MODERATOR;
            }
            catch (UserNotFoundException)
            {
                return (int)ForumGenerator.userTypes.MEMBER;
            }              
        }

    }
}
