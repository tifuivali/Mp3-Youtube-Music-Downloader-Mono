using System;
using System.Diagnostics;

namespace Mp3MusicYoutubeDownloader
{
    public static class M4A2MP3
    {
        /// <summary>
        /// converts an M4A file to an MP3 file.  Note:
        /// make sure that lame.exe and faad.exe are in the same directory
        /// as this dll.
        /// Inspired by (and using code from)
        /// http://yakkowarner.blogspot.com/2008/06/my-m4a2mp3-script.html
        /// </summary>
        /// <param name="fromPath">The m4a file (use full path)</param>
        /// <param name="toPath">the mp3 file (use full path)</param>
		public static void ExecuteCommand(string command)
		{
			Process proc = new System.Diagnostics.Process ();
			proc.StartInfo.FileName = "/bin/bash";
			proc.StartInfo.Arguments = "-c \" " + command + " \"";
			proc.StartInfo.UseShellExecute = false; 
			proc.StartInfo.RedirectStandardOutput = true;
			proc.Start ();

			while (!proc.StandardOutput.EndOfStream) {
				Console.WriteLine (proc.StandardOutput.ReadLine ());
			}
		}

		public static void ConvertToMp3(string fromPath,string toPath)
		{
			string comand = String.Format ("ffmpeg -i '{0}' -vn  -acodec libmp3lame -ac 2 -ab 380k -ar 48000 '{1}'", fromPath, toPath);
			Console.WriteLine (comand);
			ExecuteCommand (comand);

		}
    }
}
