using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Mp3YtubeDownloader.Downlaoder
{
    public class YotubeUtils
    {
        public static bool IsYoutubeValidUri(string url)
        {
            if (url.StartsWith("https://m.youtube.com/watch"))
                return true;
            if (url.StartsWith("http://m.youtube.com/watch"))
                return true;
            if (url.StartsWith("http://www.m.youtube.com/watch"))
                return true;
            if (url.StartsWith("https://www.m.youtube.com/watch"))
                return true;
            if (url.StartsWith("http://www.m.youtube.com/watch"))
                return true;
            if (url.StartsWith("http://www.youtube.com/watch"))
                return true;
            if (url.StartsWith("https://www.youtube.com/watch"))
                return true;
            return false;

        }
    }
}