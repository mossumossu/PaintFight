using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using SunshineConsole;
using OpenTK.Graphics;
using OpenTK.Input;
using System.Threading;

namespace PaintFight {
    class Arena {
        public Object lockObj = new Object();
        public Cell[,] arenaArray = new Cell[80, 32];
        public bool gameFinished = false;
        public Random rnd = new Random();

        public void Init() {

            //fill with cells
            for (int i = 0; i < 80; i++) {
                for (int j = 0; j < 32; j++) {
                    arenaArray[i, j] = new Cell(i, j);
                }
            }
            //outer walls
            for (int k = 0; k < 32; k++) {
                arenaArray[0, k].wall = true;
                arenaArray[79, k].wall = true;
            }
            for (int l = 0; l < 80; l++) {
                arenaArray[l, 0].wall = true;
                arenaArray[l, 31].wall = true;
            }
            //some inner barriers

        }

        public Thread StartDraw() {
            var t = new Thread(() => DrawWindow());
            t.Start();
            return t;
        }

        private void DrawWindow() {
            ConsoleWindow console = new ConsoleWindow(32, 80, "Paint Fight!");
            Cell currCell;
            SetTimer();
            while (console.WindowUpdate()) {
                while (gameFinished == false) {
                    for (int i = 0; i < 80; i++) {
                        for (int j = 0; j < 32; j++) {
                            currCell = arenaArray[i, j];

                            if (currCell.wall == true) {
                                console.Write(j, i, "x", Color4.White, Color4.White);
                            }
                            else if (currCell.owner != 0) {
                                if (currCell.owner == 1) {
                                    console.Write(j, i, "x", Color4.HotPink, Color4.HotPink);
                                }
                                else {
                                    console.Write(j, i, "x", Color4.Lime, Color4.Lime);
                                }
                            }
                            else {
                                console.Write(j, i, ".", Color4.White);
                            }
                            if (currCell.occupied == true) {
                                if (currCell.owner == 1) {
                                    console.Write(j, i, "@", Color4.White, Color4.HotPink);
                                }
                                else if (currCell.owner == 2) {
                                    console.Write(j, i, "@", Color4.White, Color4.Lime);
                                }

                            }
                        }
                    }
                    console.WindowUpdate();
                    Thread.Sleep(16);
                }
                int team1total = 0;
                int team2total = 0;

                for (int i = 0; i < 80; i++) {
                    for (int j = 0; j < 32; j++) {
                        currCell = arenaArray[i, j];
                        if(currCell.owner == 1) {
                            team1total++;
                        } else if (currCell.owner == 2){
                            team2total++;
                        }
                    }
                }
                if (team1total > team2total) {
                    console.Write(0, 0, "GAME FINISHED - TEAM 1 WINS! " + team1total + " to " + team2total, Color4.Black, Color4.White);
                } else {
                    console.Write(0, 0, "GAME FINISHED - TEAM 2 WINS! " + team1total + " to " + team2total, Color4.Black, Color4.White);
                }
                
            }

        }

        public void DrawArena(ConsoleWindow console) {
        }

        public static System.Timers.Timer aTimer;

        public void SetTimer() {
            aTimer = new System.Timers.Timer(20000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e) {
            gameFinished = true;
        }
    }
}
