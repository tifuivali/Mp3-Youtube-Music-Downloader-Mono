using System;
using Gtk;
namespace Mp3MusicYoutubeDownloader
{
	public class DownloadList
	{
		private Container container;
		private FormatType type;
		private TreeView listViewDownlaods;
		public FormatType TypeFormatDownload;
		private TreeViewColumn colTitle;
		private TreeViewColumn colType;
		private TreeViewColumn colStatus;
		private TreeViewColumn colProgress;
		private ListStore downloadsListStore;

		public FormatType DownloadType
		{
			set{ this.type = value; }
			get{return this.type;}
		}

		public Container ListContainer
		{
			set{ this.container = value; }
			get{ return this.container; }
		}

		public TreeView ListDownloads
		{
			get{ return this.listViewDownlaods;}
		}


		public DownloadList (Container listContainter)
		{
			this.container = listContainter;
			downloadsListStore = new ListStore (typeof(DownloadItem));
			Build ();
		}

		private void Build()
		{
			this.listViewDownlaods = new TreeView ();
			colTitle = new TreeViewColumn ();
			colProgress = new TreeViewColumn ();
			colStatus = new TreeViewColumn ();
			colType = new TreeViewColumn ();
			colType.Title = "Type";
			colProgress.Title = "Progress";
			colStatus.Title = "Status";
			colTitle.Title = "Title";
			CellRendererText titleCellRender = new CellRendererText ();
			CellRendererText typeCellRender = new CellRendererText ();
			CellRendererText statusCellRender = new CellRendererText ();
			CellRendererProgress progressCellRender = new CellRendererProgress ();
			colTitle.PackStart (titleCellRender, true);
			colType.PackStart (typeCellRender, true);
			colStatus.PackStart (statusCellRender, true);
			colProgress.PackStart (progressCellRender,true);
			colTitle.SetCellDataFunc (titleCellRender, new TreeCellDataFunc (this.RenderTitleDownload));
			colType.SetCellDataFunc (typeCellRender, new TreeCellDataFunc (RenderTypeDownload));
			colStatus.SetCellDataFunc (statusCellRender, new TreeCellDataFunc (RenderStatusDownload));
			colProgress.SetCellDataFunc (progressCellRender, new TreeCellDataFunc (RenderProgressDownload));
			listViewDownlaods.AppendColumn (colTitle);
			listViewDownlaods.AppendColumn (colType);
			listViewDownlaods.AppendColumn (colStatus);
			listViewDownlaods.AppendColumn (colProgress);
			listViewDownlaods.Model = downloadsListStore;
			container.Add (listViewDownlaods);
		}
			

		private void RenderTitleDownload (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			DownloadItem downloadItem = (DownloadItem) model.GetValue (iter, 0);
			(cell as Gtk.CellRendererText).Text = downloadItem.Title;
			model.EmitRowChanged (model.GetPath (iter), iter);
		}

		private void RenderTypeDownload (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			DownloadItem downloadItem = (DownloadItem) model.GetValue (iter, 0);
			(cell as Gtk.CellRendererText).Text = downloadItem.Type.ToString();
			model.EmitRowChanged (model.GetPath (iter), iter);
		}

		private void RenderStatusDownload (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			DownloadItem downloadItem = (DownloadItem) model.GetValue (iter, 0);
			(cell as Gtk.CellRendererText).Text = downloadItem.Status;
			model.EmitRowChanged (model.GetPath (iter), iter);
		}
			

		private void RenderProgressDownload (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			DownloadItem downloadItem = (DownloadItem) model.GetValue (iter, 0);
			string progress=downloadItem.Progress.ToString();
			if (progress.Length > 5)
				progress = progress.Substring (0, 5);
				
			(cell as Gtk.CellRendererProgress).Text = progress;
			(cell as CellRendererProgress).Value =(int)downloadItem.Progress;
			model.EmitRowChanged (model.GetPath (iter), iter);
		}

		public void addToDownloadsList(DownloadItem item)
		{
			
			downloadsListStore.AppendValues (item);
		}


	}

	public class DownloadItem
	{
		public string Title;
		public FormatType Type;
		public string Status;
		public float Progress;

		public DownloadItem(string title,FormatType type,string status,float progress)
		{
			this.Progress = progress;
			this.Status = status;
			this.Title = title;
			this.Type = type;
		}
	}



}

