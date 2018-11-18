using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunshineConsole;
using OpenTK.Graphics;
using OpenTK.Input;
using System.Threading;

namespace PaintFight {
    class Program {
        static void Main(string[] args) {
            ConsoleWindow console = new ConsoleWindow(32, 80, "Paint Fight!");

            console.Write(6, 34, "PAINT FIGHT!", Color4.Lime);

            console.Write(12, 30, "PRESS 'Z' TO QUICKPLAY.", Color4.HotPink);

            // update the window until a key is pressed or the window is closed:
            while (console.WindowUpdate()) {

                if (console.KeyPressed) {
                    Key key = console.GetKey();
                    if (key == Key.Z) {
                        //initialize arena
                        Arena myArena = new Arena();
                        Cell currCell = new Cell();

                        myArena.Init();

                        //initial draw
                        //myArena.DrawArena(console);
                        Thread t0 = myArena.StartDraw();

                        //initialize players
                        Player p1 = new Player(1, 1, myArena, myArena.arenaArray[10, 10]);
                        Thread t1 = new Thread(new ThreadStart(p1.Run));
                        
                        Player p2 = new Player(2, 2, myArena, myArena.arenaArray[70, 10]);
                        Thread t2 = new Thread(new ThreadStart(p2.Run));

                        Player p3 = new Player(3, 1, myArena, myArena.arenaArray[10, 12]);
                        Thread t3 = new Thread(new ThreadStart(p3.Run));

                        Player p4 = new Player(4, 2, myArena, myArena.arenaArray[70, 12]);
                        Thread t4 = new Thread(new ThreadStart(p4.Run));

                        //Player p5 = new Player(5, 1, myArena, myArena.arenaArray[10, 14]);
                        //Thread t5 = new Thread(new ThreadStart(p5.Run));

                        //Player p6 = new Player(6, 2, myArena, myArena.arenaArray[70, 14]);
                        //Thread t6 = new Thread(new ThreadStart(p6.Run));

                        //Player p7 = new Player(7, 1, myArena, myArena.arenaArray[12, 16]);
                        //Thread t7 = new Thread(new ThreadStart(p7.Run));

                        //Player p8 = new Player(8, 2, myArena, myArena.arenaArray[72, 16]);
                        //Thread t8 = new Thread(new ThreadStart(p8.Run));

                        t1.Start();
                        t2.Start();
                        t3.Start();
                        t4.Start();
                        //t5.Start();
                        //t6.Start();
                        //t7.Start();
                        //t8.Start();

                        //myArena.DrawArena(console);

                        //t2.Join();

                        //myArena.DrawArena(console);




                        //start match (with timer)

                        //something to regularly redraw
                    }
                }
            }
        }
    }
}
