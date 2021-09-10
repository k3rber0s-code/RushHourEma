using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RushHourEma
{
    public class Map
    {
        public Car selectedCar;
        public readonly string MapName;
        public readonly string FileName;
        public readonly int MapSize;
        public readonly Vector2 ExitPosition;
        public List<Car> Cars;
        public bool[,] FreeFields;

        public Map(string fileName, string mapName, int mapSize, List<string> carDescriptions, int exitPosition)
        {
            FileName = fileName;
            MapName = mapName;
            MapSize = mapSize;
            ExitPosition = new Vector2(exitPosition / mapSize, exitPosition % mapSize);
            SetupCars(mapSize, carDescriptions);
            SetupFreeFields();
        }
        public static Map LoadLevelFromFile(string fileName)
        {
            Dictionary<string, string> lines = new Dictionary<string, string>();
            using (var file = new StreamReader(fileName))
            {
                string line;
                while (!file.EndOfStream && file.ReadLine() != "MAP_DEFINITION_START") { }
                while (!file.EndOfStream && (line = file.ReadLine()) != "MAP_DEFINITION_END")
                {
                    var split = line.Split("=");
                    string property = split[0];
                    string value = split[1];
                    lines.Add(property, value);
                }
            }
            var mapName = lines["name"];
            var mapSize = int.Parse(lines["size"]);
            var exitPosition = int.Parse(lines["exit"]);
            List<string> carDescriptions = new List<string>();
            foreach (var val in lines["cars"].Split(";"))
            {
                carDescriptions.Add(val);
            }

            return new Map(fileName, mapName, mapSize, carDescriptions, exitPosition);
        }
        private void SetupCars(int mapSize, List<string> carDescriptions)
        {
            Cars = new List<Car>();
            foreach (var description in carDescriptions)
            {
                int position = Int32.Parse(description.Split(',')[0]);
                int width = Int32.Parse(description.Split(',')[1]);
                int height = Int32.Parse(description.Split(',')[2]);
                bool isGoalCar = (Int32.Parse(description.Split(',')[3]) == 1 ? true : false);

                Cars.Add(new Car(position / mapSize, position % mapSize, width, height, isGoalCar));
            }

        }

        private void SetupFreeFields()
        {
            FreeFields = new bool[MapSize, MapSize];
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    if ((i == 0 || j == 0 || i == MapSize - 1 || j == MapSize - 1) && !(i == ExitPosition.X && j == ExitPosition.Y))
                    {
                        FreeFields[i, j] = false;
                    }
                    else
                    {
                        FreeFields[i, j] = true;
                    }
                }
            }
            foreach (Car car in Cars)
            {
                if (car.CarOrientation == Orientation.HORIZONTAL)
                {
                    for (int i = 0; i < car.Width; i++)
                    {
                        FreeFields[car.XPos + i, car.YPos] = false;
                    }
                }
                else if (car.CarOrientation == Orientation.VERTICAL)
                {
                    for (int i = 0; i < car.Height; i++)
                    {
                        FreeFields[car.XPos, car.YPos + i] = false;
                    }
                }
            }
        }

        internal bool CheckForWin(Car car)
        {
            
            {
                bool check1 = (car.XPos == ExitPosition.X && car.YPos == ExitPosition.Y);
                bool check2 = (car.CarOrientation == Orientation.HORIZONTAL && (car.XPos+1 == ExitPosition.X && car.YPos == ExitPosition.Y));
                bool check3 = (car.CarOrientation == Orientation.VERTICAL && (car.XPos == ExitPosition.X && car.YPos + 1 == ExitPosition.Y));
                if (car.IsGoalCar 
                    && (check1 || check2 || check3))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Car MoveCar(string id, Direction direction)
        {
            if (GetCarByID(id) != null)
            {
                var car = GetCarByID(id);
                Cars.Remove(car);
                if (IsValidMove(car, direction))
                {
                    car.Move(direction, this);
                    UpdateFreeFields(car);
                }
                Cars.Add(car);
                return car;
            }
            else
            {
                throw new NullReferenceException(); //TODO
            }
        }
        private bool IsValidMove(Car car, Direction direction)
        {
            return CheckDirectionValidity(car, direction);


        }
        private void UpdateFreeFields(Car car)
        {
            if (car.CarOrientation == Orientation.HORIZONTAL)
            {
                for (int i = 0; i < car.Width; i++)
                {
                    FreeFields[car.XPos + i, car.YPos] = false;
                }
            }
            else if (car.CarOrientation == Orientation.VERTICAL)
            {
                for (int i = 0; i < car.Height; i++)
                {
                    FreeFields[car.XPos, car.YPos + i] = false;
                }
            }
        }

        private static bool CheckDirectionValidity(Car car, Direction direction)
        {
            if (direction == Direction.UP && car.CarOrientation == Orientation.VERTICAL
                            || direction == Direction.DOWN && car.CarOrientation == Orientation.VERTICAL
                            || direction == Direction.LEFT && car.CarOrientation == Orientation.HORIZONTAL
                            || direction == Direction.RIGHT && car.CarOrientation == Orientation.HORIZONTAL)
            {
                return true;
            }
            else return false;
        }

        public Car GetCarByID(string id)
        {
            return Cars.Find(x => x.Id == id);
        }


        public Map Reset()
        {
            return LoadLevelFromFile(FileName);
        }

    }
    public enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }

}
