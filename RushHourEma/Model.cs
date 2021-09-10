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
        public event ModelHandler<Model> changedValue;
        public event ModelHandler<Model> changedColor; //OUT
        public event ModelHandler<Model> carsAdded;
        public event ModelHandler<Model> carMoved;
        public event ModelHandler<Model> gameOver;

        int value;
        public Car selectedCar;
        string selectedCarID; // Change to Car type? TODO OUT
        Map map;

        // implementation of IModel interface set the initial value to 0
        public Model()
        {
            map = Map.LoadLevelFromFile(@"C:\Users\toman\source\repos\RushHourEma\RushHourEma\maps\default.txt");
        }
        // Set value function to set the value in case if the user directly changes the value
        // in the textbox and the view change event fires in the controller
        public void setvalue(int v)
        {
            value = v;
        }
        // Change the value and fire the event with the new value inside ModelEventArgs
        // This will invoke the function valueIncremented in the model and will be displayed
        // in the textbox subsequently
        public void ChangeValue()
        {
            value += 30;
            changedValue.Invoke(this, new ModelEventArgs(value));
        }
        // Attach the function which is implementing the IModelObserver so that it can be
        // notified when a value is changed
        public void attach(IModelObserver imo)
        {
            changedValue += new ModelHandler<Model>(imo.valueIncremented);
            changedColor += new ModelHandler<Model>(imo.carSelected);
            carsAdded += new ModelHandler<Model>(imo.carsAdded);
            carMoved += new ModelHandler<Model>(imo.carMoved);
            gameOver += new ModelHandler<Model>(imo.gameOver);


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
        public List<Car> ReturnCars()
        {
            return map.Cars;
        }

        public void MoveCar(Direction direction)
        {
            if (selectedCar != null)
            {
                var newCar = map.MoveCar(selectedCar.Id, direction);
                carMoved.Invoke(this, new ModelEventArgs(new Point(newCar.XPos, newCar.YPos)));
                bool isOver = map.CheckForWin();
                if(isOver)
                {
                    gameOver.Invoke(this, new ModelEventArgs(isOver));
                }
            }

        }

        public void ResetMap()
        {
            throw new NotImplementedException();
        }
    }
}
