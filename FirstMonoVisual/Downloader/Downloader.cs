using System;
using System.Collections.Generic;
using System.Linq;
using YoutubeExtractor;
using System.Threading;
using System.IO;
using Gtk;
namespace Mp3MusicYoutubeDownloader
{
	
	public class Downloader
	{
		private DownloadList downloadList;
		private string pathDonloads;

		public string SavePath
		{
			set{this.pathDonloads = value;}
			get{ return this.pathDonloads; }				
		}

		public Downloader (DownloadList downloadList)
		{
			this.downloadList = downloadList;
		}

		public Downloader (DownloadList downloadList,string savePath)
		{
			this.downloadList = downloadList;
			this.SavePath = savePath;
		}

		public void DownloadVideo(string url)
		{
			Thread th = new Thread (() => DoDownloadVideo (url));
			th.Start ();

		}

		public void DoDownloadVideo(string url)
		{
			DownloadItem downloadItem = new DownloadItem ("Last Download", FormatType.None, "Pending", 0);
			downloadList.addToDownloadsList (downloadItem);
			IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);
			VideoInfo video = videoInfos
				.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720||info.Resolution==480);
			if (video.RequiresDecryption)
			{
				DownloadUrlResolver.DecryptDownloadUrl(video);
			}
			downloadItem.Progress = 0;
			downloadItem.Status = "Downloading";
			downloadItem.Title = video.Title;
			downloadItem.Type = FormatType.Mp4;

			ExtendedVideoDownloader videoDownloader = new ExtendedVideoDownloader(video, Path.Combine(SavePath, video.Title +
				video.VideoExtension),downloadItem);

			videoDownloader.DownloadProgressChanged+=new EventHandler<ProgressEventArgs>(OnProgressChangedVideo);
			videoDownloader.DownloadFinished += new EventHandler(OnDownloadFinishVideo);
			try{
				videoDownloader.Execute ();
			}
			catch(Exception exc) {
				MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,"Download "+video.Title+" failed!\n"+exc.Message);
				md.Run ();
				md.Destroy();
			}
		}

		public void DownloadAudioSQ(string url)
		{
			Thread th = new Thread (() => DoDownloadAudioSQ (url));
			th.Start ();

		}

		public void DownloadAudioHQ(string url)
		{
			Thread th = new Thread (() => DoDownloadAudioHQ (url));
			th.Start ();
		}


		private void DoDownloadAudioHQ(string url)
		{
			DownloadItem downloadItem = new DownloadItem ("Last Download", FormatType.None, "Pending", 0);
			downloadList.addToDownloadsList (downloadItem);
			IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);
			VideoInfo video = videoInfos
				.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720||info.Resolution==480);
			if (video.RequiresDecryption)
			{
				DownloadUrlResolver.DecryptDownloadUrl(video);
			}
			downloadItem.Progress = 0;
			downloadItem.Status = "Downloading";
			downloadItem.Title = video.Title;
			downloadItem.Type = FormatType.Mp4;
			string fromPath = Path.Combine (SavePath, video.Title +
			                 video.VideoExtension);
			string toPath = fromPath.Substring (0, fromPath.Length - 4) + ".mp3";
			ExtendedVideoDownloader videoDownloader = new ExtendedVideoDownloader(video,fromPath,downloadItem);

			videoDownloader.DownloadProgressChanged+=new EventHandler<ProgressEventArgs>(OnProgressChangedVideo);
			videoDownloader.DownloadFinished += new EventHandler(OnDownloadFinishVideo);
			try
			{
			    videoDownloader.Execute ();
                //convert the video
			    M4A2MP3.ConvertToMp3(fromPath,toPath);
				File.SetAttributes(fromPath,FileAttributes.Normal);
				File.Delete(fromPath);
				videoDownloader.CurretDownloadItem.Status="Completed";

			}
			catch(Exception exc) {
				MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,"Download "+video.Title+" failed!\n"+exc.Message);
				md.Run ();
				md.Destroy();
			}

		}

		private void DoDownloadAudioSQ(string url)
		{
			DownloadItem downloadItem = new DownloadItem ("Last Donwload", FormatType.None, "Pending", 0);
			downloadList.addToDownloadsList (downloadItem);
			IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);
			VideoInfo video = videoInfos
				.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720||info.Resolution==480);
			if (video.RequiresDecryption)
			{
				DownloadUrlResolver.DecryptDownloadUrl(video);
			}
			downloadItem.Progress = 0;
			downloadItem.Title = video.Title;
			downloadItem.Type = FormatType.Mp3SQ;
			downloadItem.Status = "Downloading";


			ExtendedAudioSQDownloader audioDownloader = new ExtendedAudioSQDownloader(video, Path.Combine(SavePath, video.Title +
				video.VideoExtension),downloadItem);





			audioDownloader.DownloadProgressChanged+=new EventHandler<ProgressEventArgs>(OnProgressChangedAudioSQ);
			audioDownloader.DownloadFinished += new EventHandler(OnDownloadFinishAudioSQ);
			try
			{
			audioDownloader.Execute ();
			}
			catch (Exception exc) {
				MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,"Download "+video.Title+" failed!\n"+exc.Message);
				md.Run ();
				md.Destroy();
			}
		}

	



		private void OnProgressChangedAudioSQ(object sender,ProgressEventArgs args)
		{
			ExtendedAudioSQDownloader audioDownloader = (ExtendedAudioSQDownloader)sender;
			audioDownloader.CurrentDownloadItem.Progress = (float)args.ProgressPercentage;
		}

		private void OnProgressChangedVideo(object sender,ProgressEventArgs args)
		{
			ExtendedVideoDownloader videoDownloader = (ExtendedVideoDownloader)sender;
			videoDownloader.CurretDownloadItem.Progress = (float)args.ProgressPercentage;
		}


		private void OnDownloadFinishAudioSQ(object sender,EventArgs args)
		{
			ExtendedAudioSQDownloader audioDownloader = (ExtendedAudioSQDownloader)sender;
			audioDownloader.CurrentDownloadItem.Status = "Completed";
		}

		private void OnDownloadFinishVideo(object sender,EventArgs args)
		{
			ExtendedVideoDownloader videoDownloader = (ExtendedVideoDownloader)sender;
			videoDownloader.CurretDownloadItem.Status = "Completed";
		}

		private void OnDownloadFinishVideoAudioHQ(object sender,EventArgs args)
		{
			ExtendedVideoDownloader videoDownloader = (ExtendedVideoDownloader)sender;
			videoDownloader.CurretDownloadItem.Status = "Converting";
			videoDownloader.CurretDownloadItem.Progress = 0;
		}
	}
}

