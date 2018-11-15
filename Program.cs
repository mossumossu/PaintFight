using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunshineConsole;
using OpenTK.Graphics;
using OpenTK.Input;

namespace PaintFight {
    class Program {
        static void Main(string[] args) {
            // Create a window, 16 rows high and 40 columns across:
            ConsoleWindow console = new ConsoleWindow(32, 80, "Sunshine Console Hello World");

            // Write to the window at row 6, column 14:
            console.Write(6, 34, "PAINT FIGHT!", Color4.Lime);

            // Finally, update the window until a key is pressed or the window is closed:
            while (!console.KeyPressed && console.WindowUpdate()) {
                /* WindowUpdate() does a few very important things:
                ** It renders the console to the screen;
                ** It checks for input events from the OS, such as keypresses, so that they
                **   can reach the program;
                ** It returns false if the console has been closed, so that the program
                **   can be properly ended. */
            }
        }
    }
}
