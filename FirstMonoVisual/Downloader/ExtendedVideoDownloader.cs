using System;
using YoutubeExtractor;
namespace Mp3MusicYoutubeDownloader
{
	public class ExtendedVideoDownloader:VideoDownloader
	{
		private DownloadItem downloadItem;
		public DownloadItem CurretDownloadItem {
			set{ this.downloadItem = value; }
			get{ return this.downloadItem; }
		}

		public ExtendedVideoDownloader (VideoInfo video, string savePath, DownloadItem item) : base (video, savePath)
		{
			this.CurretDownloadItem = item;
		}
	}
}

