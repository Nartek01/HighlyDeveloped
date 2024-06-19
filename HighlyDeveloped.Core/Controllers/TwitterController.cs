//TwitterController.cs
using System.Web.Mvc;
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
    }
}
