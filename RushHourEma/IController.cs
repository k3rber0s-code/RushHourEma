using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RushHourEma
{
    public interface IController
    {
        void ChangeValue();
        void StartGame();
        void SelectCar(string id);
        void KeyPressed(KeyPressEventArgs e);
    }
}
