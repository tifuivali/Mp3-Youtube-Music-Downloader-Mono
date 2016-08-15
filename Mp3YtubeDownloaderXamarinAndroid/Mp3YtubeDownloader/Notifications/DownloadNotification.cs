using System;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Mp3YtubeDownloader.Downlaoder;
using YoutubeExtractor;

namespace Mp3YtubeDownloader.Notifications
{
    public class DownloadNotification
    {
        private NotificationManager notificationManager;

        public int Id { get; }

        public Context CurrentContext { get; set; }

        public Downloader Download { get; set; }

        public DownloadNotification(Context context,Downloader download)
        {
            CurrentContext = context;
            Download = download;
            
        }

        public void StartNotify()
        {
            notificationManager = CurrentContext.GetSystemService(Context.NotificationService) as NotificationManager;
            Download.OnDownloadStart += OnDownloadStartPerformed;
            Download.OnProgressDownloadChanged += OnDownloadProgressChangedPerform;
        }


        private void OnDownloadProgressChangedPerform(object o, ProgressEventArgs e)
        {
            VideoInfo video = (VideoInfo) o;
            NotifyProgress(e.Progress,video.Title);
        }

        private void NotifyProgress(int progress,string title)
        {
            var notification = new Notification.Builder(CurrentContext)
               .SetSmallIcon(Resource.Mipmap.Icon)
               .SetContentTitle(title)
               .SetContentText($"Downloading...[{progress} %]")
               .Build();
            notificationManager.Notify(Download.DownloadId, notification);
        }

        private void OnDownloadStartPerformed(object o, EventArgs ea)
        {
            NotifyStartDownload();
        }

        private void  NotifyStartDownload()
        {
            var notification = new Notification.Builder(CurrentContext)
                .SetSmallIcon(Resource.Mipmap.Icon)
                .SetContentTitle("YTube Downloader")
                .SetContentText("Start Download...")
                .Build();
            notificationManager.Notify(Download.DownloadId,notification);
        }




    }
}