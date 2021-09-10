using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RushHourEma
{
    class Controller : IController
    {
        IView view;
        IModel model;
        public Controller(IView v, IModel m)
        {
            view = v;
            model = m;
            view.SetController(this);
            model.attach((IModelObserver)view);
            view.changed += new ViewHandler<IView>(this.view_changed);
            StartGame();
        }
        public void view_changed(IView v, ViewEventArgs e)
        {
            model.setvalue(e.value);
        }
        public void ChangeValue()
        {
            model.ChangeValue();
            //do something
            //model.increment();
        }
        public void KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                model.MoveCar(Direction.UP);
            }
            else if (e.KeyChar == 'a')
            {
                model.MoveCar(Direction.LEFT);
            }
            else if (e.KeyChar == 's')
            {
                model.MoveCar(Direction.DOWN);

            }
            else if (e.KeyChar == 'd')
            {
                model.MoveCar(Direction.RIGHT);

            }
            else if (e.KeyChar == 'l')
            {
            }
            else if (e.KeyChar == 'r')
            {
                model.ResetMap();
            }
            //CheckIfWon() ? isOver = true : isOver = false;
        }
        public void StartGame()
        {
            model.AddCars();
        }

        public void SelectCar(string id)
        {
            model.SelectCar(id);
        }

        public Car ReturnCarFromID(string id)
        {
            return model.ReturnCarFromID(id);
        }
    }
    
}
