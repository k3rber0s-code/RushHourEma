using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        
        public ModelEventArgs(int v)
        {
            newval = v;
        }
        public ModelEventArgs(PictureBox pb)
        {
            newpb = pb;
        }
        public ModelEventArgs(List<Car> cars, int mapSize)
        {
            newcars = cars;
            newMapSize = mapSize;
        }
    }
    public class Model : IModel
    {
        public event ModelHandler<Model> changedValue;
        public event ModelHandler<Model> changedColor;
        public event ModelHandler<Model> carsAdded;

        int value;
        Car selectedCar;
        string selectedCarID; // Change to Car type? TODO
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
            value+= 30;
            changedValue.Invoke(this, new ModelEventArgs(value));
        }
        // Attach the function which is implementing the IModelObserver so that it can be
        // notified when a value is changed
        public void attach(IModelObserver imo)
        {
            changedValue += new ModelHandler<Model>(imo.valueIncremented);
            changedColor += new ModelHandler<Model>(imo.carSelected);
            carsAdded += new ModelHandler<Model>(imo.carsAdded);
        }
        public void AddCars()
        {
            carsAdded.Invoke(this, new ModelEventArgs(this.map.Cars, this.map.MapSize));
        }

        public void GetCarFromID(string ID)
        {
            selectedCar = map.GetCarByID(ID);
            //changedColor.Invoke(this, new ModelEventArgs());
        }
        public List<Car> ReturnCars()
        {
            return map.Cars;
        }
    }
}
