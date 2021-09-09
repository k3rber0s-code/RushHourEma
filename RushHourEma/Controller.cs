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
        public void KeyPressed(KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
            }
            else if (e.KeyChar == 'a')
            {
            }
            else if (e.KeyChar == 's')
            {
            }
            else if (e.KeyChar == 'd')
            {
            }
            else if (e.KeyChar == 'l')
            {
            }
            else if (e.KeyChar == 'r')
            {
            }
            //CheckIfWon() ? isOver = true : isOver = false;
        }
        public void StartGame()
        {
            model.AddCars();
        }

        public void SelectCar(string id)
        {
            model.GetCarFromID(id);
        }
    }
}
