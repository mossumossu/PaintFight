using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaintFight {
    class Player {
        public int playerID;
        public int team;
        public bool paintedThisTurn = false;

        public bool hasGoal = false;

        public Cell playerCell;
        public Cell goalCell;
        public Arena arena;
        int[,] distArray;
        public Stack<Cell> moveStack = new Stack<Cell>();
        Queue<Cell> queue = new Queue<Cell>();

        public Player(int inID, int inTeam, Arena inArena, Cell inCell) {
            playerID = inID;
            team = inTeam;
            arena = inArena;

            playerCell = inCell;
            inCell.occupied = true;
            inCell.owner = inTeam;
        }

        public void Run() {
            while (arena.gameFinished == false) {
                paintedThisTurn = false;

                // obtain lock on gameboard while we pathfind and move
                Monitor.Enter(arena.lockObj);
                try {
                    FindGoal();
                    FindPath();
                    Move();
                }
                finally {
                    Monitor.Exit(arena.lockObj);
                }

                // if we painted this turn, find out if our cell is 'locked', wait on it if so
                if(paintedThisTurn == true) {
                    int value = arena.rnd.Next(-50, 16);
                    if (value > 0) {
                        Console.WriteLine(playerID + " is on locked cell, waiting " + value + "ms.");
                        Thread.Sleep(value);
                    }
                }
                
                // default wait between movements
                Thread.Sleep(5);
            }
        }

        public void FindGoal() {
            Cell currCell;
            int[] directionOrder = new int[] { 0, 1, 2, 3 };
            hasGoal = false;
            distArray = new int[80, 32];
            queue.Enqueue(playerCell);
            distArray[playerCell.x, playerCell.y] = 0;

            while (queue.Count != 0) {
                currCell = queue.Dequeue();
                
                directionOrder = directionOrder.OrderBy(x => arena.rnd.Next()).ToArray();

                for (int i = 0; i < directionOrder.Count(); i++) {
                    if (directionOrder[i] == 0) {
                        CheckCell(currCell, arena.arenaArray[currCell.x, currCell.y - 1]);
                    }
                    else if (directionOrder[i] == 1) {
                        CheckCell(currCell, arena.arenaArray[currCell.x + 1, currCell.y]);
                    }
                    else if (directionOrder[i] == 2) {
                        CheckCell(currCell, arena.arenaArray[currCell.x, currCell.y + 1]);
                    }
                    else if (directionOrder[i] == 3) {
                        CheckCell(currCell, arena.arenaArray[currCell.x - 1, currCell.y]);
                    }
                }
            }
        }

        public void CheckCell(Cell curr, Cell check) {
            if (check.wall == false && check.occupied == false && distArray[check.x, check.y] == 0) {
                distArray[check.x, check.y] = distArray[curr.x, curr.y] + 1;

                if (check.owner != this.team && hasGoal == false) {
                    hasGoal = true;
                    goalCell = check;
                }

                queue.Enqueue(check);
            }
            else if (check.wall == true || check.occupied == true) {
                if (check != playerCell) {
                    distArray[check.x, check.y] = 999;
                }
            }
        }

        public void FindPath() {
            bool pathCompleted = false;
            Cell currCell = goalCell;

            moveStack.Push(goalCell);

            Cell lowest = arena.arenaArray[goalCell.x, goalCell.y - 1];

            while (pathCompleted == false) {
                if (distArray[currCell.x, currCell.y - 1] < distArray[lowest.x, lowest.y]) {
                    lowest = arena.arenaArray[currCell.x, currCell.y - 1];
                }
                if (distArray[currCell.x + 1, currCell.y] < distArray[lowest.x, lowest.y]) {
                    lowest = arena.arenaArray[currCell.x + 1, currCell.y];
                }
                if (distArray[currCell.x, currCell.y + 1] < distArray[lowest.x, lowest.y]) {
                    lowest = arena.arenaArray[currCell.x, currCell.y + 1];
                }
                if (distArray[currCell.x - 1, currCell.y] < distArray[lowest.x, lowest.y]) {
                    lowest = arena.arenaArray[currCell.x - 1, currCell.y];
                }
                moveStack.Push(lowest);
                currCell = lowest;

                if (lowest == playerCell) {
                    pathCompleted = true;
                    moveStack.Pop();
                }
            }
        }

        public void Move() {
            Cell moveTo = moveStack.Pop();

            playerCell.occupied = false;
            playerCell = moveTo;

            moveTo.occupied = true;

            if (moveTo.owner != this.team) {
                moveTo.owner = this.team;
                paintedThisTurn = true;
                Console.WriteLine(playerID + " painted.");
            } else {
                Console.WriteLine(playerID + " moved.");
            }
            
        }
    }
}
