using System;

namespace FirstMonoVisual
{
	public partial class MainWin : Gtk.Window
	{
		public MainWin () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

