using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RushHourEma
{
    public delegate void CarPosition(string id, int xPos, int yPos);
    public class Map
    {
        public event CarPosition CarMoved;
        public Car selectedCar;
        public readonly string MapName;
        public readonly string FileName;
        public readonly int MapSize;
        public List<Car> Cars;
        public bool[,] FreeFields;

        public Map(string fileName, string mapName, int mapSize, List<string> carDescriptions)
        {
            FileName = fileName;
            MapName = mapName;
            MapSize = mapSize;
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
            List<string> carDescriptions = new List<string>();
            foreach (var val in lines["cars"].Split(";"))
            {
                carDescriptions.Add(val);
            }

            return new Map(fileName, mapName, mapSize, carDescriptions);
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
                    FreeFields[i, j] = true;
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
                throw new NullReferenceException();
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
        private  bool CheckSpaceAvailability(Car car, Direction direction, Map map)
        {
            if (car.CarOrientation == Orientation.HORIZONTAL)
            {
                for (int i = 0; i < car.Width; i++)
                {
                    map.FreeFields[car.XPos + i, car.YPos] = true;
                }
            }
            else if (car.CarOrientation == Orientation.VERTICAL)
            {
                for (int i = 0; i < car.Height; i++)
                {
                    map.FreeFields[car.XPos, car.YPos + i] = true;
                }
            }

            car.Move(direction, map);

            if (car.CarOrientation == Orientation.HORIZONTAL)
            {
                for (int i = 0; i < car.Width; i++)
                {
                    if (map.FreeFields[car.XPos + i, car.YPos] == false)
                    {
                        return false;
                    }
                }
            }
            else if (car.CarOrientation == Orientation.VERTICAL)
            {
                for (int i = 0; i < car.Height; i++)
                {
                    if(map.FreeFields[car.XPos, car.YPos + i] == false)
                    {
                        return false;
                    }
                }
            }
            return true;
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
