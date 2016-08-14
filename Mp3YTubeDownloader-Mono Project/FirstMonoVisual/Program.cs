using System;
using Gtk;
using Mono.WebBrowser;
using WebKit;
namespace Mp3MusicYoutubeDownloader
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.ShowAll ();
			Application.Run ();


		}
	}
}
