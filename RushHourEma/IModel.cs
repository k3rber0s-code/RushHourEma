using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RushHourEma
{
    public interface IModel
    {
        void attach(IModelObserver imo);
        void ChangeValue();
        void setvalue(int v);
        void GetCarFromID(string id);
        void AddCars();
        List<Car> ReturnCars();
    }
}