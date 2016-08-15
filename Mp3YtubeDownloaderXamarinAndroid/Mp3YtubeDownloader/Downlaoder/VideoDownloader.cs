using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Android.App;
using YoutubeExtractor;

namespace Mp3YtubeDownloader.Downlaoder
{
    public class VideoDownloader : Downloader
    {
        public override int VideoResolution { get; set; }

        public override int DownloadId { get; set; }

        public sealed override string DownloadUrl { get; set; }

        public override event EventHandler<ProgressEventArgs> OnProgressDownloadChanged;

        public override event EventHandler<DetailsEventArgs> OnDetails;

        public override event EventHandler<EventArgs> OnDownloadStart;

        public override event EventHandler<DownloadFinishEventArgs> OnDownloadFinished;

        public override string PathToSave { get; set; }

        public VideoDownloader(string link)
        {
            DownloadUrl = link;
        }

        public override async void DownloadContent()
        {
            OnDetails?.Invoke(this, new DetailsEventArgs("Start Download..."));
            OnDownloadStart?.Invoke(this, EventArgs.Empty);
            Thread thread = new Thread(StartDownload);
            thread.Start();
            //StartDownload();
        }


        private async void StartDownload()
        {
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(this.DownloadUrl);
            int maxResolution = 360;
            if (videoInfos.Any())
                maxResolution = videoInfos.Max(x => x.Resolution);
        

            var video = videoInfos.FirstOrDefault(x => x.VideoType == VideoType.Mp4 && x.Resolution == maxResolution && x.AudioBitrate>0);

            if (video == null)
            {
                video = videoInfos.FirstOrDefault(x => x.VideoType == VideoType.Mp4 && x.Resolution == 720 && x.AudioBitrate>0);
                if(video == null)
                    video = videoInfos.FirstOrDefault(x => x.VideoType == VideoType.Mp4 && x.AudioBitrate > 0);
            }

            if (video != null && video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            Application.SynchronizationContext.Post(_ =>
            {
                OnProgressDownloadChanged?.Invoke(video, new ProgressEventArgs(0));
            }, null);
            
            var progressReporter = new Progress<DownloadBytesProgress>();
            progressReporter.ProgressChanged += (s, args) =>
            {
                Application.SynchronizationContext.Post(_ =>
                {
                    OnProgressDownloadChanged?.Invoke(video, new ProgressEventArgs((int)(100 * args.PercentComplete)));
                }, null);
               

            };
            if (video == null) return;
            Application.SynchronizationContext.Post(_ =>
            {
                OnDetails?.Invoke(video, new DetailsEventArgs($"Downloading {video.Title}..."));
            }, null);

            var totalBytes = await CreateDownloadTask(video.DownloadUrl, progressReporter, $"{Path.Combine(PathToSave, video.Title)}.mp4");
            Application.SynchronizationContext.Post(_ =>
            {
                OnDetails?.Invoke(video, new DetailsEventArgs("Download fished path:" + video.Title));
            }, null);
            OnDownloadFinished?.Invoke(video, new DownloadFinishEventArgs(video.Title, totalBytes));
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(VideoDownloader))
            {
                return false;
            }
            var downloader = (VideoDownloader) obj;
            if (downloader.DownloadId != this.DownloadId)
                return false;
            return true;
        }
    }
}