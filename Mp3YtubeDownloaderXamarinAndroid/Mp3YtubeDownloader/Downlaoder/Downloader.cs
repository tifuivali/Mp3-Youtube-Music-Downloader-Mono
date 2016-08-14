using System;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Android.OS;
using Android.App;
using YoutubeExtractor;
namespace Mp3YtubeDownloader
{
	public class Downloader
	{
		private WebClient webClient;

		public int VideoResolution { get; set; }

		public event EventHandler<ProgressEventArgs> OnProgressDownloadChanged;

		public event EventHandler<DetailsEventArgs> OnDetails;
		public string PathToSave;

		public Downloader()
		{
			webClient = new WebClient();
		}

		public void DownloadVideo(string link)
		{
			if (OnDetails != null)
			{
				OnDetails(this,new DetailsEventArgs("Start Download..."));
			}
			IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
			VideoInfo video = videoInfos.OrderByDescending(x => x.Resolution).FirstOrDefault();s

			if (video.RequiresDecryption)
			{
				DownloadUrlResolver.DecryptDownloadUrl(video);
			}

			if (OnProgressDownloadChanged != null)
			{
				OnProgressDownloadChanged(this, new ProgressEventArgs(0));
			}
			Progress<DownloadBytesProgress> progressReporter = new Progress<DownloadBytesProgress>();
			progressReporter.ProgressChanged += (s, args) =>
			{
				if (OnProgressDownloadChanged != null)
				{
					OnProgressDownloadChanged(this, new ProgressEventArgs((int)(100 * args.PercentComplete)));
				}

			};
			if (OnDetails != null)
			{
				OnDetails(this,new DetailsEventArgs("Downloading " + video.Title + "..."));
			}

			Task<int> downloadTask =CreateDownloadTask(video.DownloadUrl, progressReporter,Path.Combine(PathToSave, video.Title)+".mp4");
		}

		public async Task<int> CreateDownloadTask(string urlToDownload, IProgress<DownloadBytesProgress> progessReporter,string fileName)
		{
			int receivedBytes = 0;
			int totalBytes = 0;
			WebClient client = new WebClient();
			FileStream fileStream = File.OpenWrite(fileName);
			using (var stream = await client.OpenReadTaskAsync(urlToDownload))
			{
				byte[] buffer = new byte[4096];
				totalBytes = Int32.Parse(client.ResponseHeaders[HttpResponseHeader.ContentLength]);



				for (;;)
				{
					int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
					fileStream.Write(buffer, 0, bytesRead);
					if (bytesRead == 0)
					{
						await Task.Yield();
						break;
					}


					receivedBytes += bytesRead;
					if (progessReporter != null)
					{
						DownloadBytesProgress args = new DownloadBytesProgress(urlToDownload, receivedBytes, totalBytes);
						progessReporter.Report(args);
					}
				}
			}
			fileStream.Close();
			Application.SynchronizationContext.Post(_ =>
			{
				if (OnDetails != null)
				{
					OnDetails(this, new DetailsEventArgs("Download fished path:" + fileName));
				}
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

		public float PercentComplete { get { return (float)BytesReceived / TotalBytes; } }

		public string Filename { get; private set; }

		public bool IsFinished { get { return BytesReceived == TotalBytes; } }
	}


}

