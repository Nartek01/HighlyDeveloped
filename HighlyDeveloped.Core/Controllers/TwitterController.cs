//TwitterController.cs
using HighlyDeveloped.Core.ViewModel;
using System;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace HighlyDeveloped.Core.Controllers
{
    public class TwitterController : SurfaceController
    {
        // Controller action
        public ActionResult GetTweets(string twitterHandle)
        {
            string PARTIAL_VIEW_FOLDER = "~/Views/Partials";

            return PartialView(PARTIAL_VIEW_FOLDER + "/Latest Tweets.cshtml", twitterHandle);
        }


        //Summary
        // This will use the twitter API, to get the latest tweets.
        // I'll read the API key from site settings
        // <param name="twitterHandle"></param>
        // <param name="numberOfTweets"></param>
        // <return></return>
        //Summary
        private string GetLatestTweets(string twitterHandle, int numberOfTweets)
        {
            // Get the keys from site settigns
            var siteSettings = Umbraco.ContentQuery // Comes from SurfaceController
                .ContentAtRoot() // Go back to root
                .DescendantsOrSelfOfType("siteSettings") // Look for siteSettings Model/Document Type Alias in root
                .FirstOrDefault(); // Get the first match(?)

            if (siteSettings == null)
            {
                throw new Exception("No site settigns");
            } else
            {
                // oauth_consumer...
                var consumerAPI = siteSettings.Value<string>("consumerApiKey");
                var consumerSecret = siteSettings.Value<string>("consumerApiKeySecret");

                // oauth_token...
                var accessToken = siteSettings.Value<string>("accessToken");
                var accessSecret = siteSettings.Value<string>("accessTokenSecret");
            }
        }
    }
}
