package md58b27a20ac0a4b976604c36d00e54dc34;


public class ExtendedWebView
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_shouldOverrideUrlLoading:(Landroid/webkit/WebView;Ljava/lang/String;)Z:GetShouldOverrideUrlLoading_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_onPageFinished:(Landroid/webkit/WebView;Ljava/lang/String;)V:GetOnPageFinished_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"";
		mono.android.Runtime.register ("Mp3YtubeDownloader.ExtendedWebView, Mp3YtubeDownloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ExtendedWebView.class, __md_methods);
	}


	public ExtendedWebView () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ExtendedWebView.class)
			mono.android.TypeManager.Activate ("Mp3YtubeDownloader.ExtendedWebView, Mp3YtubeDownloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ExtendedWebView (android.widget.EditText p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == ExtendedWebView.class)
			mono.android.TypeManager.Activate ("Mp3YtubeDownloader.ExtendedWebView, Mp3YtubeDownloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Widget.EditText, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public boolean shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1)
	{
		return n_shouldOverrideUrlLoading (p0, p1);
	}

	private native boolean n_shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1);


	public void onPageFinished (android.webkit.WebView p0, java.lang.String p1)
	{
		n_onPageFinished (p0, p1);
	}

	private native void n_onPageFinished (android.webkit.WebView p0, java.lang.String p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
