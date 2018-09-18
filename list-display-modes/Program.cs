using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using OpenTK;

namespace ListDisplayModes {
	internal static class Program {

		private static readonly List<string> Log = new List<string>();

		private static void Main() {

			var orderFuncs = new Func<DisplayResolution, float>[] {
				mode => mode.Width * mode.Height,
				mode => mode.Width,
				mode => mode.Height,
				mode => mode.RefreshRate,
				mode => mode.BitsPerPixel
			};

			Console.WriteLine("Specify order priority");
			Console.WriteLine(" [0] Screen area (default)");
			Console.WriteLine(" [1] Width");
			Console.WriteLine(" [2] Height");
			Console.WriteLine(" [3] Refresh rate");
			Console.WriteLine(" [4] Color depth");

			int selection = int.TryParse(Console.ReadLine(), out selection) && selection <= 4 ? selection : 0;
			var order = orderFuncs[selection];

			for(int i = 0; ; i++) {
				var display = DisplayDevice.GetDisplay((DisplayIndex)i);
				if(display == null) {
					if(i == 0) WriteLine("No displays found");
					break;
				}

				WriteLine($"---- Display {i}{(display.IsPrimary ? " (Primary)" : "")} ----");
				WriteLine("Native resolution:");
				PrintDisplayMode(display.Width, display.Height, display.RefreshRate, display.BitsPerPixel);
				WriteLine("Available resolutions:");
				foreach(var mode in display.AvailableResolutions.OrderByDescending(order)) {
					PrintDisplayMode(mode.Width, mode.Height, mode.RefreshRate, mode.BitsPerPixel);
				}
				WriteLine();
			}

			Console.WriteLine();
			Console.WriteLine("Enter file name to save log or leave empty to skip");
			string filename = Console.ReadLine();
			if(string.IsNullOrWhiteSpace(filename)) return;

			try {
				File.WriteAllLines(filename, Log);
			}
			catch(IOException e) {
				Console.WriteLine(e);
			}
		}

		private static void WriteLine(string text = "") {
			Console.WriteLine(text);
			Log.Add(text);
		}

		private static void PrintDisplayMode(int width, int height, float refreshRate, int bitsPerPixel) {
			WriteLine($"    {width}x{height}".PadRight(14) + $"@{refreshRate:f2}Hz".PadRight(10) + $"{bitsPerPixel}bpp");
		}
	}
}
