using System;
using YoutubeExtractor;
using System.Collections.Generic;

namespace Mp3MusicYoutubeDownloader
{
	public class ExtendedAudioSQDownloader:AudioDownloader
	{
		private DownloadItem item;

		public DownloadItem CurrentDownloadItem {
			set{ this.item = value; }
			get{ return this.item; }
		}
		public ExtendedAudioSQDownloader (VideoInfo video,string savePath,DownloadItem item):base(video,savePath)
		{
			this.item = item;
		}


	}
}

