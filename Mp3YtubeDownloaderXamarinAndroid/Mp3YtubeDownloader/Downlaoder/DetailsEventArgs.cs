using System;
namespace Mp3YtubeDownloader
{
	public class DetailsEventArgs:EventArgs
	{
		public string Message { get; set; }
		public DetailsEventArgs(string message)
		{
			Message = message;
		}
	}
}

