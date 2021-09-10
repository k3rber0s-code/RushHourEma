using System;
using System.Numerics;

namespace RushHourEma
{
    public delegate void ViewHandler<IView>(IView sender, ViewEventArgs e);
    public class ViewEventArgs : EventArgs
    {
        public int value;
        public ViewEventArgs(int v) { value = v; }
    }

    public interface IView
    {
        void SetController(IController cont);
        void AddCar(Car car, bool bringToFront = false);
        void AddWalls(Vector2 exitLocation);
        void ShowHelp();
    }
}