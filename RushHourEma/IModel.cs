using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RushHourEma
{
    public interface IModel
    {
        void attach(IModelObserver imo);
        void SelectCar(string id);
        Car ReturnCarFromID(string id);
        void AddCars();
        void MoveCar(Direction direction);
        void AddMaps();
        void ResetMap();
    }
}