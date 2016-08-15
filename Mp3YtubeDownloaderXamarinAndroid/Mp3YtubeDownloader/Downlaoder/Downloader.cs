using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.App;

namespace Mp3YtubeDownloader.Downlaoder
{
	public abstract class Downloader
	{
	    public abstract int VideoResolution { get; set; }

        public abstract int DownloadId { get; set; }

	    public abstract string DownloadUrl { get; set; }

		public abstract event EventHandler<ProgressEventArgs> OnProgressDownloadChanged;

		public abstract event EventHandler<DetailsEventArgs> OnDetails;

	    public abstract event EventHandler<EventArgs> OnDownloadStart;

	    public abstract event EventHandler<DownloadFinishEventArgs> OnDownloadFinished;

	    public abstract string PathToSave { get; set; }


	    public abstract void DownloadContent();


	    protected async Task<int> CreateDownloadTask(string urlToDownload, IProgress<DownloadBytesProgress> progessReporter,string fileName)
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
					receivedBytes += bytesRead;
					if (progessReporter != null)
					{
						var args = new DownloadBytesProgress(urlToDownload, receivedBytes, totalBytes);
						progessReporter.Report(args);
					}
                    if (bytesRead == 0)
                    {
                        await Task.Yield();
                        break;
                    }
                }
			}
			fileStream.Close();
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

