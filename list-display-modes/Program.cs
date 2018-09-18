using System;
using System.Linq;
using OpenTK;

namespace ListDisplayModes {
	internal static class Program {
		private static void Main() {for(int i = 0; ; i++) {
				var display = DisplayDevice.GetDisplay((DisplayIndex)i);
				if(display == null) {
					if(i == 0) Console.WriteLine("No displays found");
					break;
				}

				Console.WriteLine($"---- Display {i}{(display.IsPrimary ? " (Primary)" : "")} ----");
				Console.WriteLine("Native resolution:");
				PrintDisplayMode(display.Width, display.Height, display.RefreshRate, display.BitsPerPixel);
				Console.WriteLine("Available resolutions:");
				foreach(var mode in display.AvailableResolutions) {
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
