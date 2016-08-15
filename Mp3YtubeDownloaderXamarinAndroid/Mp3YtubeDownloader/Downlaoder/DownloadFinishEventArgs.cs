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
    public class DownloadFinishEventArgs:EventArgs
    {
        public string DownloadName { get; }

        public long TotalBytesRecived { get; }

        public DownloadFinishEventArgs(string downloadName, long totalBytes)
        {
            DownloadName = downloadName;
            TotalBytesRecived = totalBytes;
        }
    }
}