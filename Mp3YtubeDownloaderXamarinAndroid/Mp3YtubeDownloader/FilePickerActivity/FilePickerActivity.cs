
namespace Mp3YtubeDownloader
{
	using System;
	using Android.Util;
	using Android.App;
	using Android.OS;
	using Android.Support.V4.App;
	using Android.Widget;
	using Android.Content;
    [Activity(Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/ic_launcher")]
    public class FilePickerActivity : FragmentActivity
    {
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.FilePicker);
			Log.Info("", "On create Piker");
			Button btnSelect = FindViewById<Button>(Resource.Id.button_selectFolder);
			btnSelect.Click += ClickSelectFolder;
			Button btnCancel = FindViewById<Button>(Resource.Id.button_CancelPicker);
			btnCancel.Click += ClickCancel;
		}

		private void ClickSelectFolder(object o, EventArgs ea)
		{
			Intent resultIntent = new Intent(this, typeof(MainActivity));
			resultIntent.PutExtra("path", FileListFragment.SelectedDirctory);
			SetResult(Result.Ok,resultIntent);
			Finish();
		}

		private void ClickCancel(object o, EventArgs ea)
		{
			Intent resultItent = new Intent(this, typeof(MainActivity));
			SetResult(Result.Canceled);
			Finish();
		}

    }
}
