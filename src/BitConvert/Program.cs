using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BitConvert
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.ApplicationExit += (sender, args) => Properties.Settings.Default.Save();
			Application.Run(new MainForm());
		}
	}
}
