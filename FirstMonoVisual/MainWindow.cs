using System;
using Gtk;
using WebKit;
using Mp3MusicYoutubeDownloader;
using System.Threading;

public partial class MainWindow: Gtk.Window
{
	private WebView webView;
	public static string HOME_PAGE="http://www.youtube.com";
	private string pathFolderSave;
	private Mp3MusicYoutubeDownloader.DownloadList downloadList;
	private Downloader downloader;
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		webView = new WebView ();
		this.scrolledWebWindow.Add(webView);
		this.text_adress.Text = HOME_PAGE;
		webView.LoadFinished += new LoadFinishedHandler (OnLoadFinished);
		webView.Open (text_adress.Text);
		pathFolderSave = Environment.GetFolderPath (Environment.SpecialFolder.MyMusic);
		downloadList = new DownloadList (this.scrolledListDownlaods);
		downloader = new Downloader (downloadList);
		downloader.SavePath = pathFolderSave;
	}

	private void OnLoadFinished(object sender,LoadFinishedArgs args)
	{
		WebView webView=(WebView) sender;
		text_adress.Text=webView.Uri;
		if(text_adress.Text.StartsWith("http://www.youtube.com/watch?")||
			text_adress.Text.StartsWith("https://www.youtube.com/watch?"))
		{
			button_DownloadVideo.Sensitive = true;
			button_downloadAudioSq.Sensitive = true;
			button_DownloadHQ.Sensitive = true;
		}
	    else{
			button_DownloadVideo.Sensitive = false;
			button_downloadAudioSq.Sensitive = false;
			button_DownloadHQ.Sensitive = false;
			}


	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnButtonGoClicked (object sender, EventArgs e)
	{
		webView.Open (text_adress.Text);

	}
	protected void OnSelectFolderActionActivated (object sender, EventArgs e)
	{
		FileChooserDialog fileChooser = new FileChooserDialog ("Select Folder", this, FileChooserAction.SelectFolder,
			                               "Cancel", ResponseType.Cancel, "Select Folder", ResponseType.Accept);

		if (fileChooser.Run () == (int)ResponseType.Accept) 
		{
			pathFolderSave = fileChooser.Filename;
			downloader.SavePath=fileChooser.Filename;
		}
		fileChooser.Destroy ();
	}
		
	protected void OnButtonBackClicked (object sender, EventArgs e)
	{
		if (webView.CanGoBack())
			webView.GoBack ();
	}

	protected void OnButtonHomeClicked (object sender, EventArgs e)
	{
		webView.Open (HOME_PAGE);

	}

	protected void OnButtonForwardClicked (object sender, EventArgs e)
	{
		if (webView.CanGoForward ())
			webView.GoForward ();
	}

	protected void OnButtonDownloadVideoClicked (object sender, EventArgs e)
	{
		downloader.DownloadVideo (text_adress.Text);
	}


	protected void OnOpenDownloadsActionActivated (object sender, EventArgs e)
	{
		System.Diagnostics.Process.Start (pathFolderSave);
	}

	protected void OnButtonDownloadAudioSqClicked (object sender, EventArgs e)
	{
		
		downloader.DownloadAudioSQ (text_adress.Text);
	}

	protected void OnButtonDownloadHQClicked (object sender, EventArgs e)
	{
		downloader.DownloadAudioHQ (text_adress.Text);
	}
}
