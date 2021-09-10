using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RushHourEma
{
    public delegate void ModelHandler<IModel>(IModel sender, ModelEventArgs e);
    // The ModelEventArgs class which is derived from th EventArgs class to 
    // be passed on to the controller when the value is changed
    public class ModelEventArgs : EventArgs
    {
        public int newval;
        public PictureBox newpb;
        public List<Car> newcars;
        public int newMapSize;
        public Point newLocation;
        public Vector2 newExitPosition;
        public bool isGameOver;


        public ModelEventArgs(int v)
        {
            newval = v;
        }
        public ModelEventArgs(PictureBox pb)
        {
            newpb = pb;
        }
        public ModelEventArgs(List<Car> cars, int mapSize, Vector2 exitPosition)
        {
            newcars = cars;
            newMapSize = mapSize;
            newExitPosition = exitPosition;
        }
        public ModelEventArgs(Point location)
        {
            newLocation = location;
        }
        public ModelEventArgs(bool isOver)
        {
            isGameOver = isOver;
        }
    }
    public class Model : IModel
    {
        public event ModelHandler<Model> carsAdded;
        public event ModelHandler<Model> carMoved;
        public event ModelHandler<Model> gameOver;
        public event ModelHandler<Model> mapReseted;


        string[] levelPaths;
        string currentMapPath;
        int levelCounter;

        Car selectedCar;
        Map map;

        public Model()
        {
            AddMaps();
            currentMapPath = levelPaths[levelCounter];
            map = Map.LoadLevelFromFile(currentMapPath);
        }
        // Set value function to set the value in case if the user directly changes the value
        // in the textbox and the view change event fires in the controller

        // Change the value and fire the event with the new value inside ModelEventArgs
        // This will invoke the function valueIncremented in the model and will be displayed
        // in the textbox subsequently

        // Attach the function which is implementing the IModelObserver so that it can be
        // notified when a value is changed
        public void Attach(IModelObserver imo)
        {
            carsAdded += new ModelHandler<Model>(imo.carsAdded);
            carMoved += new ModelHandler<Model>(imo.carMoved);
            gameOver += new ModelHandler<Model>(imo.gameOver);
            mapReseted += new ModelHandler<Model>(imo.mapReseted);
        }
        public void AddCars()
        {
            carsAdded.Invoke(this, new ModelEventArgs(this.map.Cars, this.map.MapSize, this.map.ExitPosition));
        }
        public void SelectCar(string ID)
        {
            selectedCar = map.GetCarByID(ID);
        }
        public Car ReturnCarFromID(string ID)
        {
            return map.GetCarByID(ID);
        }
        public void MoveCar(Direction direction)
        {
            if (selectedCar != null)
            {
                var newCar = map.MoveCar(selectedCar.Id, direction);
                carMoved.Invoke(this, new ModelEventArgs(new Point(newCar.XPos, newCar.YPos)));
                bool isOver = map.CheckForWin(selectedCar);
                if(isOver)
                {
                    levelCounter++;
                    gameOver.Invoke(this, new ModelEventArgs(isOver));

                }
            }

        }
        public void ResetMap()
        {
            map = Map.LoadLevelFromFile(currentMapPath);
            mapReseted.Invoke(this, new ModelEventArgs(this.map.Cars, this.map.MapSize, this.map.ExitPosition));

        }

        public void AddMaps()
        {
            levelCounter = 0;
            levelPaths = new string[5];
            levelPaths[0] = (@"C:\Users\toman\source\repos\RushHourEma\RushHourEma\maps\1.txt");
            levelPaths[1] = (@"C:\Users\toman\source\repos\RushHourEma\RushHourEma\maps\2.txt");
            levelPaths[2] = (@"C:\Users\toman\source\repos\RushHourEma\RushHourEma\maps\3.txt");
            levelPaths[3] = (@"C:\Users\toman\source\repos\RushHourEma\RushHourEma\maps\4.txt");
            levelPaths[4] = (@"C:\Users\toman\source\repos\RushHourEma\RushHourEma\maps\5.txt");
        }
    }
}
