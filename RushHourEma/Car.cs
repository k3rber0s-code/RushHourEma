using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RushHourEma
{
    public class Car
    {
        public readonly string Id;
        private int xPos;

        public int XPos
        {
            get { return xPos; }
            set { xPos = value; }
        }
        private int yPos;

        public int YPos
        {
            get { return yPos; }
            set { yPos = value; }
        }

        public readonly bool IsGoalCar;
        public readonly int Width;
        public readonly int Height;

        private Orientation carOrientation;

        public Orientation CarOrientation
        {
            get { return (Width > Height ? Orientation.HORIZONTAL : Orientation.VERTICAL); }
            set { carOrientation = value; }
        }



        private bool _isOnGoal;

        public bool isOnGoal
        {
            get { return _isOnGoal; }
            set { _isOnGoal = value; }
        }


        public Car(int _xPos, int _yPos, int _width, int _height, bool _isGoalCar=false)
        {
            this.Id = Guid.NewGuid().ToString();
            this.XPos = _xPos;
            this.YPos = _yPos;
            this.isOnGoal = false;
            IsGoalCar = _isGoalCar;
            Width = _width;
            Height = _height;

        }
        public virtual bool Move(Direction direction, Map map)
        {
            int goalXPos = XPos;
            int goalYPos = YPos;

            DeleteFromFreeFields(map);

            switch (direction)
            {
                case Direction.UP:
                    goalYPos -= 1;
                    break;
                case Direction.RIGHT:
                    goalXPos += 1;
                    break;
                case Direction.DOWN:
                    goalYPos += 1;
                    break;
                case Direction.LEFT:
                    goalXPos -= 1;
                    break;
            }

            if (CarOrientation == Orientation.HORIZONTAL)
            {
                for (int i = 0; i < Width; i++)
                {
                    if (map.FreeFields[goalXPos + i, goalYPos] == false)
                    {
                        return false;
                    }
                }
            }
            else if (CarOrientation == Orientation.VERTICAL)
            {
                for (int i = 0; i < Height; i++)
                {
                    if (map.FreeFields[goalXPos, goalYPos + i] == false)
                    {
                        return false;
                    }
                }
            }


            //var neighbour = map.GetEntityAtPosition(goalXPos, goalYPos);
            //if (neighbour != null && !neighbour.Move(direction, incomingForce - weight + force))
            //{
            //    return false;
            //}
            xPos = goalXPos;
            yPos = goalYPos;
            //isOnGoal = map.goals.Find(x => x.xPos == xPos && x.yPos == yPos) != null;
            //map.OnEntityMoved(this.id, this.xPos, this.yPos);
            return true;

        }

        private void DeleteFromFreeFields(Map map)
        {
            if (CarOrientation == Orientation.HORIZONTAL)
            {
                for (int i = 0; i < Width; i++)
                {
                    map.FreeFields[XPos + i, YPos] = true;
                }
            }
            else if (CarOrientation == Orientation.VERTICAL)
            {
                for (int i = 0; i < Height; i++)
                {
                    map.FreeFields[XPos, YPos + i] = true;
                }
            }
        }
    }
    public enum Orientation
    {
        HORIZONTAL,
        VERTICAL
    }
}
