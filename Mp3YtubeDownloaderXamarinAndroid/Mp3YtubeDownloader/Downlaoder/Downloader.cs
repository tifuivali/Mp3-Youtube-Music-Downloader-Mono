using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using YoutubeExtractor;

namespace Mp3YtubeDownloader.Downlaoder
{
	public class Downloader
	{
	    public int VideoResolution { get; set; }

		public event EventHandler<ProgressEventArgs> OnProgressDownloadChanged;

		public event EventHandler<DetailsEventArgs> OnDetails;
		public string PathToSave;


        public async void DownloadVideo(string link)
        {
            OnDetails?.Invoke(this, new DetailsEventArgs("Start Download..."));
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
            VideoInfo video = videoInfos.OrderByDescending(x => x.Resolution).FirstOrDefault();

            if (video != null && video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }

            OnProgressDownloadChanged?.Invoke(this, new ProgressEventArgs(0));
            var progressReporter = new Progress<DownloadBytesProgress>();
            progressReporter.ProgressChanged += (s, args) =>
            {
                OnProgressDownloadChanged?.Invoke(this, new ProgressEventArgs((int)(100 * args.PercentComplete)));

            };
            if (video == null) return;
            OnDetails?.Invoke(this, new DetailsEventArgs($"Downloading {video.Title}..."));
            await CreateDownloadTask(video.DownloadUrl, progressReporter, $"{Path.Combine(PathToSave, video.Title)}.mp4");
        }

	

        public async Task<int> CreateDownloadTask(string urlToDownload, IProgress<DownloadBytesProgress> progessReporter,string fileName)
		{
			var receivedBytes = 0;
		    var client = new WebClient();
			var fileStream = File.OpenWrite(fileName);
			using (var stream = await client.OpenReadTaskAsync(urlToDownload))
			{
				var buffer = new byte[4096];
				var totalBytes = int.Parse(client.ResponseHeaders[HttpResponseHeader.ContentLength]);



				for (;;)
				{
					var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
					fileStream.Write(buffer, 0, bytesRead);
					if (bytesRead == 0)
					{
						await Task.Yield();
						break;
					}


					receivedBytes += bytesRead;
					if (progessReporter != null)
					{
						var args = new DownloadBytesProgress(urlToDownload, receivedBytes, totalBytes);
						progessReporter.Report(args);
					}
				}
			}
			fileStream.Close();
			Application.SynchronizationContext.Post(_ =>
			{
			    OnDetails?.Invoke(this, new DetailsEventArgs("Download fished path:" + fileName));
			}, null);
			return receivedBytes;
		}



	}

	public class DownloadBytesProgress
	{
		public DownloadBytesProgress(string fileName, int bytesReceived, int totalBytes)
		{
			Filename = fileName;
			BytesReceived = bytesReceived;
			TotalBytes = totalBytes;
		}

		public int TotalBytes { get; private set; }

		public int BytesReceived { get; private set; }

		public float PercentComplete => (float)BytesReceived / TotalBytes;

	    public string Filename { get; private set; }

		public bool IsFinished => BytesReceived == TotalBytes;
	}


}

