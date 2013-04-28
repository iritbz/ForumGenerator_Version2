﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
//using System.Threading.Tasks;
using System.Threading;
using System.Xml;
using System.IO;

namespace ForumGenerator_Client.Communication
{
    class Communicator
    {
        public Tuple<int, string> sendLoginReq(int forumId, string userName, string password)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();
            
            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("ForumId", forumId.ToString());
            Tuple<string, string> t2 = new Tuple<string, string>("UserName", userName);
            Tuple<string, string> t3 = new Tuple<string, string>("Password", password);
            args_list.AddLast(t1);
            args_list.AddLast(t2);
            args_list.AddLast(t3);
            string logout_res = x_test.cCreateXml("login", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);


            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            Tuple<String, LinkedList<String>> res_method_args = x_test.getXmlParse(response);
            Tuple<int, String> result = new Tuple<int, string>(Convert.ToInt16(res_method_args.Item2.ElementAt(0)), res_method_args.Item2.ElementAt(1));

            return result;
        }


        public Tuple<int,string> sendLogoutReq(int forumId, string userName)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("ForumId", forumId.ToString());
            Tuple<string, string> t2 = new Tuple<string, string>("UserName", userName);
            args_list.AddLast(t1);
            args_list.AddLast(t2);
            string logout_res = x_test.cCreateXml("logout", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);



            return null;
        }

        public Tuple<int,string> sendAdminLoginReq(string userName, string password)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("UserName", userName);
            Tuple<string, string> t2 = new Tuple<string, string>("Password", password);
            args_list.AddLast(t1);
            args_list.AddLast(t2);
            string logout_res = x_test.cCreateXml("adminlogin", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);


            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            Tuple<String, LinkedList<String>> res_method_args = x_test.getXmlParse(response);
            Tuple<int, String> result = new Tuple<int, string>(Convert.ToInt16(res_method_args.Item2.ElementAt(0)), res_method_args.Item2.ElementAt(1));

            return result;
        }


        public Tuple<int,string> sendAdminLogoutReq(string userName)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("UserName", userName);
            args_list.AddLast(t1);
            string logout_res = x_test.cCreateXml("adminlogout", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);


            return null;
        }

        public Tuple<int, string> sendRegisterReq(int forumId, string userName, string password, string email, string signature)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
           
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("ForumId", forumId.ToString());
            Tuple<string, string> t2 = new Tuple<string, string>("UserName", userName);
            Tuple<string, string> t3 = new Tuple<string, string>("Password", password);
            Tuple<string, string> t4 = new Tuple<string, string>("Email", email);
            Tuple<string, string> t5 = new Tuple<string, string>("Signature", signature);

            args_list.AddLast(t1);
            args_list.AddLast(t2);
            args_list.AddLast(t3);
            args_list.AddLast(t4);
            args_list.AddLast(t5);

            string logout_res = x_test.cCreateXml("register", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();   
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            Tuple<String, LinkedList<String>> res_method_args = x_test.getXmlParse(response);       
            Tuple<int, String> result = new Tuple<int,string>( Convert.ToInt16(res_method_args.Item2.ElementAt(0)) , res_method_args.Item2.ElementAt(1));
            
            return result;
        }

        public LinkedList<Tuple<string, string>> sendGetForumsReq()
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/requests?function=getforums");
            post_request.Method = "GET";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> result = x_test.parseXmlGets(response);

            return result;
        }


        public LinkedList<Tuple<string, string>> sendGetSubForumsReq(int forumId)
        {
            string get = "http://localhost/requests?function=getsubforums&forumid=" + forumId.ToString();

            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create(get);
            post_request.Method = "GET";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            XmlHandler x_test = new XmlHandler();        
            LinkedList<Tuple<string, string>> result = x_test.parseXmlGets(response);

            return result;
        }


        public LinkedList<Tuple<string, string>> sendGetDiscussionsReq(int forumId, int subForumId)
        {
            string get = "http://localhost/requests?function=getdiscussions&forumid=" + forumId.ToString() + "&subforumid=" + subForumId.ToString(); 
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create(get);
     
            post_request.Method = "GET";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> result = x_test.parseXmlGets(response);

            return result;
        }

        public LinkedList<Tuple<string, string>> sendGetCommentsReq(int forumId, int subForumId, int dissId)
        {
            string get = "http://localhost/requests?function=getcomments&forumid=" + forumId.ToString() + "&subforumid=" + subForumId.ToString() + "&discussionid=" + dissId.ToString();
            
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create(get);
            post_request.Method = "GET";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> result = x_test.parseXmlGets(response);

            return result;
        }

        public LinkedList<Tuple<string, string>> sendGetUsersReq(int forumId)
        {
            string get = "http://localhost/requests?function=getusers&forumid=" + forumId.ToString();

            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create(get);
            post_request.Method = "GET";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();


            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> result = x_test.parseXmlGets(response);
        
            return result;

        }

        public Tuple<int, string> sendCreateNewForumReq(string userName, string password, string forumName, 
            string adminUserName, string adminPassword)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("UserName", userName);
            Tuple<string, string> t2 = new Tuple<string, string>("Password", password);
            Tuple<string, string> t3 = new Tuple<string, string>("ForumName", forumName);
            Tuple<string, string> t4 = new Tuple<string, string>("AdminUserName", adminUserName);
            Tuple<string, string> t5 = new Tuple<string, string>("AdminPassword", adminPassword);

            args_list.AddLast(t1);
            args_list.AddLast(t2);
            args_list.AddLast(t3);
            args_list.AddLast(t4);
            args_list.AddLast(t5);

            string logout_res = x_test.cCreateXml("createnewforum", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            Tuple<String, LinkedList<String>> res_method_args = x_test.getXmlParse(response);
            Tuple<int, String> result = new Tuple<int, string>(Convert.ToInt16(res_method_args.Item2.ElementAt(0)), res_method_args.Item2.ElementAt(1));

            return result;
        }

        public Tuple<int, string> sendCreateNewSubForumReq(string userName, string password, int forumId, string subForumTitle)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("UserName", userName);
            Tuple<string, string> t2 = new Tuple<string, string>("Password", password);
            Tuple<string, string> t3 = new Tuple<string, string>("ForumId", forumId.ToString());
            Tuple<string, string> t4 = new Tuple<string, string>("SubForumTitle", subForumTitle);

            args_list.AddLast(t1);
            args_list.AddLast(t2);
            args_list.AddLast(t3);
            args_list.AddLast(t4);

            string logout_res = x_test.cCreateXml("createnewsubforum", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);


            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            Tuple<String, LinkedList<String>> res_method_args = x_test.getXmlParse(response);
            Tuple<int, String> result = new Tuple<int, string>(Convert.ToInt16(res_method_args.Item2.ElementAt(0)), res_method_args.Item2.ElementAt(1));

            return result;
 
        }



        public Tuple<int, string> sendCreateNewDiscussionReq(string userName, string password, int forumId, int subForumId, string title, string content)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("UserName", userName);
            Tuple<string, string> t2 = new Tuple<string, string>("Password", password);
            Tuple<string, string> t3 = new Tuple<string, string>("ForumName", forumId.ToString());
            Tuple<string, string> t4 = new Tuple<string, string>("SubForumId", subForumId.ToString());
            Tuple<string, string> t5 = new Tuple<string, string>("Title", title);
            Tuple<string, string> t6 = new Tuple<string, string>("Content", content);

            args_list.AddLast(t1);
            args_list.AddLast(t2);
            args_list.AddLast(t3);
            args_list.AddLast(t4);
            args_list.AddLast(t5);
            args_list.AddLast(t6);


            string logout_res = x_test.cCreateXml("createnewdiscussion", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);


            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            Tuple<String, LinkedList<String>> res_method_args = x_test.getXmlParse(response);
            Tuple<int, String> result = new Tuple<int, string>(Convert.ToInt16(res_method_args.Item2.ElementAt(0)), res_method_args.Item2.ElementAt(1));

            return result;
        }


        public Tuple<int, string> sendCreateNewCommentReq(string userName, string password, int forumId, int subForumId, int discussionId, string content)
        {
            HttpWebRequest post_request = (HttpWebRequest)WebRequest.Create("http://localhost/");
            post_request.Method = "POST";
            post_request.ContentType = "text/xml";
            ASCIIEncoding encoding = new ASCIIEncoding();

            XmlHandler x_test = new XmlHandler();
            LinkedList<Tuple<string, string>> args_list = new LinkedList<Tuple<string, string>>();
            Tuple<string, string> t1 = new Tuple<string, string>("UserName", userName);
            Tuple<string, string> t2 = new Tuple<string, string>("Password", password);
            Tuple<string, string> t3 = new Tuple<string, string>("ForumName", forumId.ToString());
            Tuple<string, string> t4 = new Tuple<string, string>("SubForumId", subForumId.ToString());
            Tuple<string, string> t5 = new Tuple<string, string>("DiscussionId", discussionId.ToString());
            Tuple<string, string> t6 = new Tuple<string, string>("Content", content);

            args_list.AddLast(t1);
            args_list.AddLast(t2);
            args_list.AddLast(t3);
            args_list.AddLast(t4);
            args_list.AddLast(t5);
            args_list.AddLast(t6);

            string logout_res = x_test.cCreateXml("createnewcomment", args_list);
            byte[] data = Encoding.ASCII.GetBytes(logout_res);
            post_request.ContentLength = data.Length;
            Stream requestStream = post_request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);


            HttpWebResponse myHttpWebResponse = (HttpWebResponse)post_request.GetResponse();
            // Gets the stream associated with the response.
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encoding);
            Char[] read = new Char[512];
            // Reads 256 characters at a time.     
            int count = readStream.Read(read, 0, 512);
            string response = null;
            response = new String(read, 0, count);
            // Releases the resources of the response.
            myHttpWebResponse.Close();
            // Releases the resources of the Stream.
            readStream.Close();
            Tuple<String, LinkedList<String>> res_method_args = x_test.getXmlParse(response);
            Tuple<int, String> result = new Tuple<int, string>(Convert.ToInt16(res_method_args.Item2.ElementAt(0)), res_method_args.Item2.ElementAt(1));

            return result;
        }


    }
}
