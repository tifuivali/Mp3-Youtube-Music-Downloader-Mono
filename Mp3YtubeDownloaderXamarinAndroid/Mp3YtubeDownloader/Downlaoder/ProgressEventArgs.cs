using System;
namespace Mp3YtubeDownloader
{
	public class ProgressEventArgs:EventArgs
	{
		public int Progress { get; }

		public ProgressEventArgs(int progress)
		{
			Progress = progress; 
		}
	}
}

