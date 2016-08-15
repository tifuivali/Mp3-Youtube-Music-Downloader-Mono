using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Mp3YtubeDownloader.Downlaoder;
using Mp3YtubeDownloader.Notifications;

namespace Mp3YtubeDownloader.Downlaods
{
    public class DownlaodsManager
    {
        private static int CurrentId = 0;

        public  string DirectoryToSave { get; set; }
        
        private List<Downloader> downloadList;

        public DownlaodsManager(List<Downloader> downloadsList)
        {
            DownloadsList = downloadsList;
        }

        public DownlaodsManager()
        {
            downloadList = new List<Downloader>();
            DownloadsList = downloadList;
        }

        public List<Downloader> DownloadsList
        {
            get { return downloadList.ToList(); }
            private set { downloadList = value; }
        }


        public void AddDownload(Downloader downloader)
        {
            if (downloadList.Count > 0)
            {
                CurrentId = downloadList.Max(x => x.DownloadId);
            }
            CurrentId++;

            downloader.DownloadId = CurrentId;
            downloader.PathToSave = DirectoryToSave;
            CurrentId++;
            downloadList.Add(downloader);
        }

        public void AddDownload(Downloader downloader, int id)
        {
            if(downloadList.Any(x => x.DownloadId == id))
                throw  new DownloadIdExceprion("A download with the same id already exists!");
            downloader.DownloadId = id;
            downloader.PathToSave = DirectoryToSave;
            downloadList.Add(downloader);
        }

        public IEnumerable<Downloader> GetDownloadById(int id)
        {
            if(downloadList.All(x => x.DownloadId != id))
                throw new DownloadIdExceprion($"Not found download with id {id}");
            return downloadList.Where(x => x.DownloadId == id);
        }

        public void DeleteDownload(int id)
        {
            downloadList.RemoveAll(x => x.DownloadId == id);
        }

        public void DeleteDownload(Downloader downloader)
        {
            downloadList.Remove(downloader);
        }

        public void ClearDownloads()
        {
            downloadList.Clear();
        }

        public void StartDownload(Downloader downloader)
        {
            if(!downloadList.Contains(downloader))
                throw new StartDownloadException("Not found downloader specified argument!");
            var download = downloadList.First(x => x == downloader);
            download.DownloadContent();
        }

        public void StartDownload(int downloadId)
        {
            if (downloadList.All(x => x.DownloadId != downloadId))
            {
                throw new StartDownloadException("Not found a dwonload with specified id");
            }
            var download = downloadList.First(x => x.DownloadId == downloadId);
            download.DownloadContent();
        }

        public void StartLastDownload()
        {
            var download = downloadList.Last();
            download.DownloadContent();
        }

        private void AppendNotification(Downloader download,Context context)
        {
            var downloadNotification = new DownloadNotification(context, download);
            downloadNotification.StartNotify();
        }

        public void AddNotification(Downloader downloader,Context context)
        {
            if (!downloadList.Contains(downloader))
                throw new StartDownloadException("Not found downloader specified argument!");
            var download = downloadList.First(x => x == downloader);
            AppendNotification(download,context);
        }

        public void AddNotification(Context context,int downloadId)
        {
            if (downloadList.All(x => x.DownloadId != downloadId))
            {
                throw new StartDownloadException("Not found a dwonload with specified id");
            }
            var download = downloadList.First(x => x.DownloadId == downloadId);
            AppendNotification(download,context);
        }

        public void AddNotificationForLastDownload(Context context)
        {
            var download = downloadList.Last();
            AppendNotification(download,context);
        }

    }
}