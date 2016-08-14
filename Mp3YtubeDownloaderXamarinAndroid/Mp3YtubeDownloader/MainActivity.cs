using Android.App;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using System.IO;
using Android.Content;

using System;

namespace Mp3YtubeDownloader
{
	[Activity (Label = "Mp3 Ytube Downloader", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{

		private ExtendedWebView extendedWebView;
		private Downloader downloader;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			// Get our button from the layout resource,
			// and attach an event to it
			Button button_go = (Button)FindViewById(Resource.Id.button_go);
			button_go.Click += ButtonGo_Click;
			Button button_home = (Button)FindViewById(Resource.Id.button_home);
			button_home.Click += ButtonHome_Click;
			Button button_back = (Button)FindViewById(Resource.Id.button_back);
			button_back.Click += ButtonBack_Click;
			Button button_forward = (Button)FindViewById(Resource.Id.button_forward);
			button_forward.Click += ButtonForward_Click;
			Button button_download_video = (Button)FindViewById(Resource.Id.button_download_video);
			button_download_video.Click += ButtonDownloadVideo_Click;


			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			webView.LoadUrl(Resources.GetString(Resource.String.default_adress));
			EditText text_adress = (EditText)FindViewById(Resource.Id.text_adress);
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
			ProgressBar progressBar = (ProgressBar)FindViewById(Resource.Id.progressBar1);
			TextView txtStatus = (TextView)FindViewById(Resource.Id.textView_status);
			downloader = new Downloader();
			downloader.PathToSave = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Download");
			downloader.OnDetails += (sender, e) =>
			  {
				  txtStatus.Append(e.Message);
			  };
			downloader.OnProgressDownloadChanged += (sender, e) =>
			{
				progressBar.Progress = e.Progress;	
			};
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
							downloader.PathToSave = data.GetStringExtra("path");
							Toast.MakeText(this, "Your video will be saved in: " +
							                     downloader.PathToSave, ToastLength.Long).Show();
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

		private void ButtonDownloadVideo_Click(object o, EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			downloader.DownloadVideo(webView.Url);
		}


		private void ButtonGo_Click(object o,EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			EditText text_adress = (EditText)FindViewById(Resource.Id.text_adress);
			webView.LoadUrl(text_adress.Text);
		}

		private void ButtonHome_Click(object o, EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			webView.LoadUrl(Resources.GetString(Resource.String.default_adress));
		}


		private void ButtonBack_Click(object o, EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			if (webView.CanGoBack())
				webView.GoBack();
		}

		private void ButtonForward_Click(object o, EventArgs ea)
		{
			WebView webView = (WebView)FindViewById(Resource.Id.webView1);
			if (webView.CanGoForward())
				webView.GoForward();
		}



	}
}


