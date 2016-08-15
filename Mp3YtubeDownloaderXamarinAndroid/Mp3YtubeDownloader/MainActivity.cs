﻿using Android.App;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using System.IO;
using Android.Content;

using System;
using Mp3YtubeDownloader.Downlaoder;
using Mp3YtubeDownloader.Downlaods;
using Mp3YtubeDownloader.Notifications;

namespace Mp3YtubeDownloader
{
	[Activity (Label = "Mp3 Ytube Downloader", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		private ExtendedWebView extendedWebView;
	    private DownlaodsManager downloadsManager;

	    protected override void OnCreate(Bundle savedInstanceState)
	    {
	        base.OnCreate(savedInstanceState);
	        // Set our view from the "main" layout resource
	        SetContentView(Resource.Layout.Main);
	        // Get our button from the layout resource,
	        // and attach an event to it
	        Button button_go = (Button) FindViewById(Resource.Id.button_go);
	        button_go.Click += ButtonGo_Click;
	        Button button_home = (Button) FindViewById(Resource.Id.button_home);
	        button_home.Click += ButtonHome_Click;
	        Button button_back = (Button) FindViewById(Resource.Id.button_back);
	        button_back.Click += ButtonBack_Click;
	        Button button_forward = (Button) FindViewById(Resource.Id.button_forward);
	        button_forward.Click += ButtonForward_Click;
	        Button button_download_video = (Button) FindViewById(Resource.Id.button_download_video);
	        button_download_video.Click += ButtonDownloadVideo_Click;


	        WebView webView = (WebView) FindViewById(Resource.Id.webView1);
	        webView.LoadUrl(Resources.GetString(Resource.String.default_adress));
	        EditText text_adress = (EditText) FindViewById(Resource.Id.text_adress);
	        extendedWebView = new ExtendedWebView(text_adress);
	        webView.SetWebViewClient(extendedWebView);
	        webView.Settings.JavaScriptEnabled = true;
	        webView.KeyPress += (sender, e) =>
	        {
	            if (e.KeyCode == Android.Views.Keycode.Back)
	            {
	                webView.GoBack();
	            }
	        };


	        downloadsManager = new DownlaodsManager();
	        downloadsManager.DirectoryToSave = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Download");
	    }

	    private void OnDetailsPerformed(object o, DetailsEventArgs dea)
	    {
            TextView txtStatus = (TextView)FindViewById(Resource.Id.textView_status);
	        txtStatus.Text = dea.Message;
	    }

        private void OnDownloadProgressChangedPerformed(object o,ProgressEventArgs e)
		{
            ProgressBar progressBar = (ProgressBar)FindViewById(Resource.Id.progressBar1);
		    progressBar.Progress = e.Progress;
		}

		public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
		{
			MenuInflater.Inflate(Resource.Layout.appmenu, menu);
			return base.OnPrepareOptionsMenu(menu);
		}


		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Resource.Id.exit_button:
					{
						Process.KillProcess(Process.MyPid());
						return true;
					}
				case Resource.Id.set_path:
					{
						var filePickerIntent = new Intent(this, typeof(FilePickerActivity));
						StartActivityForResult(filePickerIntent,0);
						return true;
					}

			}
			return true;
			//return base.OnOptionsItemSelected(item);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			switch (requestCode)
			{
				case 0:
					{

						if (resultCode == Result.Ok)
						{
							downloadsManager.DirectoryToSave = data.GetStringExtra("path");
							Toast.MakeText(this, "Your video will be saved in: " +
							                     downloadsManager.DirectoryToSave, ToastLength.Long).Show();
						}
						break;
					}
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
		}

		protected override void OnPause()
		{
			base.OnPause();
		}

	    private void  RefreshStateDownloadButtons()
	    {
            Button btnDownloadMp4 =
                FindViewById<Button>(Resource.Id.button_download_video);
            btnDownloadMp4.Enabled = IsWebAdressForYoutubeVideo();
        }

		private void ButtonDownloadVideo_Click(object o, EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			downloadsManager.AddDownload(new VideoDownloader(webView.Url));
            downloadsManager.AddNotificationForLastDownload(this);
            downloadsManager.StartLastDownload();      
		}

        private bool IsWebAdressForYoutubeVideo()
        {
            WebView webView = (WebView)FindViewById(Resource.Id.webView1);
            return YotubeUtils.IsYoutubeValidUri(webView.Url);
        }

        private void ButtonGo_Click(object o,EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			EditText text_adress = (EditText)FindViewById(Resource.Id.text_adress);
			webView.LoadUrl(text_adress.Text);
            RefreshStateDownloadButtons();
		}

		private void ButtonHome_Click(object o, EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			webView.LoadUrl(Resources.GetString(Resource.String.default_adress));
            RefreshStateDownloadButtons();
        }


		private void ButtonBack_Click(object o, EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
		    if (webView.CanGoBack())
		    {
		        webView.GoBack();
                RefreshStateDownloadButtons();
            }
		}

		private void ButtonForward_Click(object o, EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
		    if (webView.CanGoForward())
		    {
		        webView.GoForward();
                RefreshStateDownloadButtons();
            }
		}



	}
}


