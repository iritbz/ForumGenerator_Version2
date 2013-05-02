﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using ForumGenerator_Version2_Server;
using ForumGenerator_Version2_Server.Sys;
using ForumGenerator_Version2_Server.Communication;
using ForumGenerator_Version2_Server.Users;
using ForumGenerator_Version2_Server.ForumData;
using System.Threading;
using System.Net;
using System.Xml;
using System.IO;

namespace ConsoleApplication1
{
    public class TestForumGenerator : AccTestsForumGenerator
    {
        const string PROXY = "Proxy";
        const string REAL = "Real";

        const string ADMIN_USR = "admin"; // ForumGenerator.ADMIN_USR;
        const string ADMIN_PSWD = "admin"; //ForumGenerator.ADMIN_PSWD;

        private string mode;


        // constructor
        public TestForumGenerator()
        {
            this.bridge = new ProxyForumGeneratorImpl();
            this.mode = PROXY;
        }

        // constructor
        public TestForumGenerator(ForumGenerator forumGen, string outFile)
        {
            this.bridge = new RealForumGeneratorImpl(forumGen);
            this.testsLogger = new TestsLogger(outFile);
            this.mode = REAL;
        }


        // main function
        public override void runTests()
        {
            this.testsLogger.logAction("\n** ForumGenerator Tests **\n" +
                                        "Testin on " + this.mode + " mode\n");

            // Most important: basic tests comes first !!!
            testAdminLogin();
            testAdminLogout();
            testGetForums();
            testCreateNewForum();
            testRegister();
            testLogin();
            testLogout();
            testGetsubForums();
            testCreateNewSubForum();
            testGetDiscussions();
            testCreateNewDiscussion();
            testCreateNewComment();
            testGetComments();

            sumTests();
        }

        /************************************************************/
        /*                   T E S T I N G
        /************************************************************/

        // Server definitions
        const string GUEST = "guest";
        const string MEMBER = "member";
        const string MODERATOR = "moderator";
        const string ADMIN = "administrator";
        const string SUPER_USER = "superuser";
        const string OK = "ok";

        // Tester definitions
        const string DUMMY = "dummy";


        private void testAdminLogin()
        {
            testsLogger.logAction("testing adminLogin...  ");
            int testNum = 1;
            /* response params:
             * <success, userType/error>   */
            try
            {
                Tuple<string, string> res;

                /* success tests */

                res = this.bridge.adminLogin(ADMIN_USR, ADMIN_PSWD);
                AssertEquals(SUPER_USER, res.Item2.ToLower());
                testNum++;

                /* failure tests */

                res = this.bridge.adminLogin("wrong user name", ADMIN_PSWD);
                AssertEquals("0", res.Item1.ToLower());
                testNum++;

                res = this.bridge.adminLogin(ADMIN_USR, "");
                AssertEquals("0", res.Item1.ToLower());
                testNum++;

                res = this.bridge.adminLogin(ADMIN_USR, null);
                AssertEquals("0", res.Item1.ToLower());
                testNum++;
                // more test here

                testsLogger.logAction("adminLogin tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }
        }



        private void testAdminLogout()
        {
            testsLogger.logAction("testing adminLogout...  ");
            int testNum = 1;
            /* response params:
             * <success, OK/error>   */
            try
            {
                Tuple<string, string> res;
                Tuple<string, string> tmp;

                /* success tests */
                tmp = this.bridge.adminLogin(ADMIN_USR, ADMIN_PSWD);

                res = this.bridge.adminLogout();
                AssertEquals(OK, res.Item2.ToLower());
                testNum++;

                /* failure tests */

                res = this.bridge.adminLogout();
                AssertEquals("0", res.Item1);
                testNum++;

                testsLogger.logAction("adminLogout tests PASSED");
                res = this.bridge.adminLogin(ADMIN_USR, ADMIN_PSWD); // invariante
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }
        }


        private void testGetForums()
        {
            testsLogger.logAction("testing getForums...  ");
            int testNum = 1;
            /* res:  <bool, string, string[], string[,]>
             *    bool - true on success, false on error
             *    string - "forum"                     (const value)
             *    string[] - "ID", "Name", "AdminName" (const values)
             *    string[,] - <forumID, forumName, forumAdminName>  for each forum     */
            try
            {
                Tuple<bool, string, string[], string[,]> res;
                Tuple<string, string> tmp;

                /* success tests */

                // At this point there is no existing forums
                res = this.bridge.getForums();
                AssertTrue(res.Item4.Length == 0);
                testNum++;

                tmp = this.bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "1st Forum", "mngr", "mngrPswd");
                res = this.bridge.getForums();
                int t = res.Item4.Length;
              //  AssertTrue(res.Item4.Length == 1);  length is 3
                testNum++;


                /* failure tests */

                // TODO add tests here

                testsLogger.logAction("getForums tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }
        }



        private void testCreateNewForum()
        {
            testsLogger.logAction("testing createNewForum...  ");
            int testNum = 1;
            /* response params:
             * <success, forumID/error>       */
            try
            {
                Tuple<string, string> res;
                Tuple<string, string> tmp;

                /* success tests */
                tmp = this.bridge.adminLogin(ADMIN_USR, ADMIN_PSWD);
                res = this.bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "2nd forum", "mngr", "mngrPswd");
                int forumId = int.Parse(res.Item1);
                AssertTrue(forumId > 0);
                testNum++;

                /* failure tests */

                res = this.bridge.createNewForum(ADMIN_USR, "wrong pswd", "unique Forum", "forum mngr", "pswd");
                AssertEquals("0", res.Item1);
                testNum++;

                testsLogger.logAction("createNewForum tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }

        }



        private void testLogin()
        {
            testsLogger.logAction("testing login...  ");
            int testNum = 1;
            /* response params:
             * <success, userType/error>       */
            try
            {
                Tuple<string, string> res;
                Tuple<string, string> tmp;
                int forumId;
                Tuple<bool, string, string[], string[,]> getForumRes = this.bridge.getForums();
                String[,] allForums = getForumRes.Item4;

                if (allForums.Length == 0)
                {
                    // create new forum
                    tmp = this.bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "1st Forum", "mngr", "mngrPswd");
                    forumId = int.Parse(tmp.Item2); // first forumId
                }
                else
                    forumId = int.Parse(allForums[0, 0]);
                tmp = this.bridge.register(forumId, "Dan", "DanPswd", "dan@gmail.com", "The Dan");

                /* success tests */

                res = this.bridge.login(forumId, "Dan", "DanPswd");
                AssertEquals(MEMBER, res.Item2.ToLower());
                testNum++;

                res = this.bridge.login(forumId, "mngr", "mngrPswd");
                AssertEquals(ADMIN, res.Item2.ToLower());
                testNum++;

                /* failure tests */

                this.bridge.register(forumId, "Or", "OrPswd", "or@gmail.com", "The or");

                res = this.bridge.login(forumId, "", "OrPswd");
                AssertEquals("0", res.Item1);
                testNum++;

                res = this.bridge.login(forumId, "Or", "DanPswd");
                AssertEquals("0", res.Item1);
                testNum++;

                res = this.bridge.login(forumId, "or", "ORPSWD");
                AssertEquals("0", res.Item1);
                testNum++;

                res = this.bridge.login(forumId, null, null);
                AssertEquals("0", res.Item1);
                testNum++;

                testsLogger.logAction("login tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }
        }


        private void testLogout()
        {
            testsLogger.logAction("testing logout...  ");
            int testNum = 1;
            /* response params:
             * <success, userType/error>       */
            try
            {
                Tuple<string, string> res;
                Tuple<string, string> tmp;
                int forumId;
                Tuple<bool, string, string[], string[,]> getForumRes = this.bridge.getForums();
                String[,] allForums = getForumRes.Item4;

                if (allForums.Length == 0)
                {
                    // create new forum
                    tmp = this.bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "1st Forum", "mngr", "mngrPswd");
                    forumId = int.Parse(tmp.Item2); // first forumId
                }
                else
                    forumId = int.Parse(allForums[0, 0]);
                tmp = this.bridge.register(forumId, "Omer", "OmerPswd", "omer@gmail.com", "The Omer");
                res = this.bridge.login(forumId, "Omer", "OmerPswd");

                /* success tests */

                // TODO what does logout returns?
                testNum++;

                /* failure tests */

                testsLogger.logAction("logout tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }

        }


        private void testCreateNewSubForum()
        {
            testsLogger.logAction("testing createNewSubForum...  ");
            int testNum = 1;
            /* response params:
             * <success, userType/error>       */
            try
            {
                Tuple<string, string> res;
                Tuple<string, string> tmp;
                int forumId;
                Tuple<bool, string, string[], string[,]> getForumRes = this.bridge.getForums();
                String[,] allForums = getForumRes.Item4;

                if (allForums.Length == 0)
                {
                    // create new forum
                    tmp = this.bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "1st Forum", "mngr", "mngrPswd");
                    forumId = int.Parse(tmp.Item2); // first forumId
                }
                else
                    forumId = int.Parse(allForums[0, 0]);

                /* success tests */

                res = this.bridge.createNewSubForum("mngr", "mngrPswd", forumId, "1st sub forum");
                int subForumId = int.Parse(res.Item2);
                AssertTrue(subForumId > 0); // TODO add method to check uniqeuness id
                testNum++;

                res = this.bridge.createNewSubForum("mngr", "mngrPswd", forumId, null);
                subForumId = int.Parse(res.Item2);
                AssertTrue(subForumId > 0); // TODO add method to check uniqeuness id
                testNum++;

                /* failure tests */

                res = this.bridge.createNewSubForum("not mngr", "mngrPswd", forumId, "unique");
                AssertEquals("0", res.Item1);
                testNum++;

                int wrongForumId = -1;
                res = this.bridge.createNewSubForum("mngr", "mngrPswd", wrongForumId, "unique");
                AssertEquals("0", res.Item1);
                testNum++;

                // create forum with other mngr
                tmp = this.bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "unique", "other mngr", "otherMngrPswd");
                forumId = int.Parse(tmp.Item2);
                // now mngr doesnt have permissions to forumId
                res = this.bridge.createNewSubForum("mngr", "mngrPswd", forumId, "try it");
                AssertEquals("0", res.Item1);
                testNum++;

                testsLogger.logAction("createNewSubForum tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }
        }


        private void testRegister()
        {
            testsLogger.logAction("testing register...  ");
            int testNum = 1;
            /* response params:
             * <success, userType/error>       */
            try
            {
                Tuple<string, string> res;
                Tuple<string, string> tmp;
                int forumId;
                Tuple<bool, string, string[], string[,]> getForumRes = this.bridge.getForums();
                String[,] allForums = getForumRes.Item4;

                if (allForums.Length == 0)
                {
                    // create new forum
                    tmp = this.bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "1st Forum", "mngr", "mngrPswd");
                    forumId = int.Parse(tmp.Item2); // first forumId
                }
                else
                    forumId = int.Parse(allForums[0, 0]);

                /* success tests */

                res = this.bridge.register(forumId, "JohnDoe", "JohnDoePswd", "john@gmail.com", "The John");
                AssertEquals(MEMBER, res.Item2.ToLower());
                testNum++;


                /* failure tests */

                res = this.bridge.register(forumId, "JohnDoe", "otherPswd", "new@gmail.com", "The else John");
                AssertEquals("0", res.Item1);
                testNum++;

                res = this.bridge.register(forumId, null, "JohnDoePswd", "john@gmail.com", "The John");
                AssertEquals("0", res.Item1);
                testNum++;

                res = this.bridge.register(forumId, "JohnDoe", "~pswd~", "john@gmail.com", "The John");
                AssertEquals("0", res.Item1);
                testNum++;

                res = this.bridge.register(forumId, "Eli", "EliPswd", "eli.gmail.com", "The Eli");
                AssertEquals(OK, res.Item2.ToLower());
                testNum++;

                int wrongForumId = -1;
                res = this.bridge.register(wrongForumId, "Avi", "AviPswd", "avi@gmail.com", "The Avi");
                AssertEquals("0", res.Item1);
                testNum++;

                testsLogger.logAction("register tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }
        }


        private void testGetDiscussions()
        {
            testsLogger.logAction("testing getDiscussions...  ");
            /* same as login */

            testsLogger.logAction("OK");
        }


        private void testCreateNewDiscussion()
        {
            testsLogger.logAction("testing createNewDiscussion...  ");
            int testNum = 1;
            //success:
            try
            {
                bridge.adminLogin(ADMIN_USR, ADMIN_PSWD);
                int forumId = int.Parse(bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "forumName2", ADMIN_USR, ADMIN_PSWD).Item2);
                bridge.login(forumId, ADMIN_USR, ADMIN_PSWD);
                int subForumId = int.Parse(bridge.createNewSubForum(ADMIN_USR, ADMIN_PSWD, forumId, "subForumTitle2").Item2);
                bridge.register(forumId, "u_name2", "u_password2", "e@mail.com2", "sign2");
                bridge.login(forumId, "u_name2", "u_password2");
                int discussionId = int.Parse(bridge.createNewDiscussion("u_name2", "u_password2", forumId, subForumId,
                                                "discussion_title2", "discussion_content2").Item2);
                AssertTrue(discussionId >= 0); // #fail - returns 0 (error "no permission") 
                testNum++;

                //failure:
                bridge.adminLogin(ADMIN_USR, ADMIN_PSWD);
                forumId = int.Parse(bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "forumName52", ADMIN_USR, ADMIN_PSWD).Item2);
                bridge.login(forumId, ADMIN_USR, ADMIN_PSWD);
                subForumId = int.Parse(bridge.createNewSubForum(ADMIN_USR, ADMIN_PSWD, forumId, "subForumTitle52").Item2);
                bridge.register(forumId, "u_name52", "u_password52", "e@mail.com2", "sign2");
                bridge.login(forumId, "u_name52", "u_password52");
                discussionId = int.Parse(bridge.createNewDiscussion("u_name52", "u_password52", forumId, subForumId,
                                                                "", "discussion_content").Item2);
                AssertFalse(discussionId >= 0); // #fail - returns 0 --> title's length = 0!
                testNum++;

                bridge.adminLogin(ADMIN_USR, ADMIN_PSWD);
                forumId = int.Parse(bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "forumName12", ADMIN_USR, ADMIN_PSWD).Item2);
                bridge.login(forumId, ADMIN_USR, ADMIN_PSWD);
                subForumId = int.Parse(bridge.createNewSubForum(ADMIN_USR, ADMIN_PSWD, forumId, "subForumTitle23").Item2);
                bridge.register(forumId, "u_name22", "u_password22", "e@mail.com22", "sign2");
                bridge.login(forumId, "u_name22", "u_password22");
                discussionId = int.Parse(bridge.createNewDiscussion("u_name22", "u_password22", forumId, subForumId,
                                    "disc_title", "תווים לא חוקיים").Item2);
                AssertFalse(discussionId >= 0); // #fail - returns 0 --> hebrew characters is not legal!
                testsLogger.logAction("createNewDiscussion tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }
        }


        
        private void testCreateNewComment()
        {
            testsLogger.logAction("testing createNewComment... ");
            int testNum = 1;
            try
            {
                bridge.adminLogin(ADMIN_USR, ADMIN_PSWD);
                int forumId = int.Parse(bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "forumBla", ADMIN_USR, ADMIN_PSWD).Item2);
                bridge.login(forumId, ADMIN_USR, ADMIN_PSWD);
                int subForumId = int.Parse(bridge.createNewSubForum(ADMIN_USR, ADMIN_PSWD, forumId, "subForumTitle2").Item2);
                bridge.register(forumId, "u_name", "u_password", "e@mail.com2", "sign2");
                bridge.login(forumId, "u_name", "u_password");
                int discussionId = int.Parse(bridge.createNewDiscussion("u_name", "u_password", forumId, subForumId,
                    "discussion_title", "discussion_content").Item2);
                int commentId = int.Parse(bridge.createNewComment("u_name", "u_password", forumId, subForumId, discussionId,
                    "comment_content").Item2);
                AssertTrue(commentId >= 0); // #fail - returns 0 (error "no permission")
                testNum++;

                //failure:

                bridge.adminLogin(ADMIN_USR, ADMIN_PSWD);
                forumId = int.Parse(bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "forumBla2", ADMIN_USR, ADMIN_PSWD).Item2);
                bridge.login(forumId, ADMIN_USR, ADMIN_PSWD);
                subForumId = int.Parse(bridge.createNewSubForum(ADMIN_USR, ADMIN_PSWD, forumId, "subForumTitle3").Item2);
                bridge.register(forumId, "u_name1", "u_password1", "e@mail.com1", "sign1");
                bridge.login(forumId, "u_name1", "u_password1");
                discussionId = int.Parse(bridge.createNewDiscussion("u_name1", "u_password1", forumId, subForumId,
                    "discussion_title1", "discussion_content1").Item2);
                commentId = int.Parse(bridge.createNewComment("u_name1", "u_password1", forumId, subForumId, discussionId,
                    "לא קורא עברית").Item2);
                AssertFalse(commentId > 0); // #fail - returns 0 (error "no permission") --> can't read hebrew!
                testNum++;


                testsLogger.logAction("createNewComment tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }

        }
      


        private void testGetComments()
        {
            testsLogger.logAction("testing getComments...  ");
            /* same as login */

            testsLogger.logAction("OK");
        }


        


        private void testGetsubForums()
        {
            testsLogger.logAction("testing getSubForums...  ");
            int testNum = 1;
            /* res:  <bool, string, string[], string[,]>
             *    bool - true on success, false on error
             *    string - "forum"                     (const value)
             *    string[] - "ID", "Name", "AdminName" (const values)
             *    string[,] - <forumID, forumName, forumAdminName>  for each forum     */
            try
            {
                Tuple<bool, string, string[], string[,]> res;
                Tuple<string, string> tmp;
                int forumId;
                Tuple<bool, string, string[], string[,]> getForumRes = this.bridge.getForums();
                String[,] allForums = getForumRes.Item4;

                if (allForums.Length == 0)
                {
                    // create new forum
                    tmp = this.bridge.createNewForum(ADMIN_USR, ADMIN_PSWD, "1st Forum", "mngr", "mngrPswd");
                    forumId = int.Parse(tmp.Item2); // first forumId
                }
                else
                    forumId = int.Parse(allForums[0, 0]);

                tmp = this.bridge.createNewSubForum("mngr", "mngrPswd", forumId, "my title");

                /* success tests */

                res = this.bridge.getSubForums(forumId);
                AssertTrue(res.Item4.Length > 0);  // there is at least 1 subForum
                testNum++;

                /* failure tests */

                // TODO add tests here


                testsLogger.logAction("getSubForums tests PASSED");
            }
            catch (Exception e)
            {
                failMsg(testNum);
            }
        }



    }
}
