using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintFight {
    class Cell {
        public bool wall;
        public bool blocked;
        public bool occupied;
        public int owner;
        public int x;
        public int y;

        public Cell(int iX, int iY) {
            wall = false;
            blocked = false;
            occupied = false;
            owner = 0;
            x = iX;
            y = iY;
        }
        public Cell() {
        }
    }
}
