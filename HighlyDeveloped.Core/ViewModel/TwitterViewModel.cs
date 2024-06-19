using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighlyDeveloped.Core.ViewModel
{
    //Summary
    // This will hold the twitter tweet data for rendering the latest tweets
    //Summary

    public class TwitterViewModel
    {
        public string TwitterHandle { get; set; } // Twitter Handle
        public bool Error { get; set; } // For error flagging
        public string Message { get; set; } // Error message
        public string Json { get; set; } // Twitter API responses in Json

    }
}
