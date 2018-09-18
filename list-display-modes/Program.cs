using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Sockets;
using OpenTK;

namespace ListDisplayModes {
	internal static class Program {

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
					if(i == 0) Console.WriteLine("No displays found");
					break;
				}

				Console.WriteLine($"---- Display {i}{(display.IsPrimary ? " (Primary)" : "")} ----");
				Console.WriteLine("Native resolution:");
				PrintDisplayMode(display.Width, display.Height, display.RefreshRate, display.BitsPerPixel);
				Console.WriteLine("Available resolutions:");
				foreach(var mode in display.AvailableResolutions.OrderByDescending(order)) {
					PrintDisplayMode(mode.Width, mode.Height, mode.RefreshRate, mode.BitsPerPixel);
				}
				Console.WriteLine();
			}
			Console.ReadLine();
		}

		private static void PrintDisplayMode(int width, int height, float refreshRate, int bitsPerPixel) {
			Console.WriteLine($"    {width}x{height}".PadRight(14) + $"@{refreshRate:f2}Hz".PadRight(10) + $"{bitsPerPixel}bpp");
		}
	}
}
