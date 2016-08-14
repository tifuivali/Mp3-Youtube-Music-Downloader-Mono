using System;
using Android.Webkit;
using Android.Widget;
namespace Mp3YtubeDownloader
{
	public class ExtendedWebView:WebViewClient
	{
		private EditText text_adress;

		public ExtendedWebView(EditText txt_adress)
		{
			this.text_adress = txt_adress;
		}
		public override bool ShouldOverrideUrlLoading(WebView view, string url)
		{
			view.LoadUrl(url);
			return true;
		}

		public override void OnPageFinished(WebView view, string url)
		{
			base.OnPageFinished(view, url);
			this.text_adress.Text = view.Url;
		}
	
	}
}


