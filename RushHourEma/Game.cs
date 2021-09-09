using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RushHourEma
{
    public class Game
    {
        public Car selectedCar;
        public string selectedCarID;
        public Map map;
        public bool isOver;

        public Game() { }

        public void KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                OnCarMoved(selectedCarID, Direction.UP);
            }
            else if (e.KeyChar == 'a')
            {
                OnCarMoved(selectedCarID, Direction.LEFT);
            }
            else if (e.KeyChar == 's')
            {
                OnCarMoved(selectedCarID, Direction.DOWN);
            }
            else if (e.KeyChar == 'd')
            {
                OnCarMoved(selectedCarID, Direction.RIGHT);
            }
            else if (e.KeyChar == 'l')
            {
                LoadMapFromFile();
            }
            else if (e.KeyChar == 'r')
            {
                ResetMap();
            }
            //CheckIfWon() ? isOver = true : isOver = false;
        }

        private void LoadMapFromFile()
        {
            throw new NotImplementedException();
        }

        private void ResetMap()
        {
            throw new NotImplementedException();
        }

        private bool CheckIfWon()
        {
            throw new NotImplementedException();
        }

        internal void OnCarMoved(string id, Direction direction)
        {
            selectedCar = map.MoveCar(id, direction);
        }
    }
}
