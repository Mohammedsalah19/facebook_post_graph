using Facebook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FbPOST_graphAPI
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            chechauthorization();


        }

        private void chechauthorization()
        {
            string appID = "1833430833392674";
            string appSecret = "8cd6f43bb6d643cc4b005d81347e5109";
            string scope = "publis_stream,manage_page";

            if (Request["code"]==null)
            {
                Response.Redirect(string.Format("https://graph.facebook.com/oauth/authorizee?client_id(0)&redirect_uri=(1)&scope=(2)",appID,Request.Url.AbsoluteUri,scope));

            }
            else
            {
                Dictionary<string, string> tokens = new Dictionary<string, string>();
                string url = string.Format("https://graph.facebook.com/oauth/access_token?client_id(0)&redirect_uri=(1)&scope=(2)&code=(3),client_secret=(4)",appID,Request.Url.AbsoluteUri,scope,Request["code"].ToString(),appSecret);

                HttpWebRequest request = WebRequest.Create(url)as HttpWebRequest;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {


                    StreamReader reader =new  StreamReader(response.GetResponseStream());
                    string valus = reader.ReadToEnd();
                    foreach (string token in valus.Split('&'))
                    {
 
                        tokens.Add(token.Substring(0, token.IndexOf("=")), token.Substring(token.IndexOf("=") + 1, token.Length - token.IndexOf("=")-1));
                    }

                }

                string access_token = tokens["access_token"];

                var client = new FacebookClient(access_token);
                client.Post("me/feed", new { messsage = "Hey FB graph " });

            }

         }
    }
}