﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RushHourEma
{
    public partial class Form1 : Form, IView, IModelObserver
    {
        IController controller;
        // View will set the associated controller, this is how view is linked to the controller.
        public void SetController(IController cont)
        {
            controller = cont;
        }
        public Dictionary<PictureBox, string> Dictionary;
        public List<PictureBox> PictureBoxes;
        public Dictionary<PictureBox, int[]> Walls;
        PictureBox selectedPictureBox;

        Color bgdColor = Color.AntiqueWhite;
        Color baseColor = Color.Blue;
        Color highlightColor = Color.Yellow;

        public int mapSize;
        private int squareSize;

        public int SquareSize
        {
            get { return this.Width / mapSize; }
            set { squareSize = value; }
        }

        #region LOADING GAME
        public Form1()
        {
            Dictionary = new Dictionary<PictureBox, string>();
            Walls = new Dictionary<PictureBox, int[]>();
            PictureBoxes = new List<PictureBox>();
            InitializeComponent();
            Width = 800;
            Height = 825;
        }
        private void Window_Load(object sender, EventArgs e)
        {
            BackColor = bgdColor;
            //BackgroundImage = Properties.Resources.asphalt1;
            BackgroundImageLayout = ImageLayout.Tile;
            MaximizeBox = false;
            this.KeyPress += controller.KeyPressed;

            ShowHelp();
        }
        void IView.ShowHelp()
        {
            string message = "Welcome to Rush Hour!\nGet the Police out before it's too late!\nSelect car with mouse, move with WASD.\nTo see this box again, press F1.";
            string title = "GAMEPLAY";
            MessageBox.Show(message, title);
        }
        #endregion
        #region ADDING CARS AND WALLS
        public void AddCar(Car car, bool bringToFront)
        {
            var newBox = new PictureBox();
            newBox.SizeMode = PictureBoxSizeMode.StretchImage;
            newBox.Width = car.Width * SquareSize;
            newBox.Height = car.Height * SquareSize;
            newBox.Location = new Point(car.XPos * SquareSize, car.YPos * SquareSize);
            newBox.Top = car.YPos * SquareSize;

            if (car.IsGoalCar)
            {
                if (car.CarOrientation == Orientation.HORIZONTAL)
                {
                    newBox.Image = Properties.Resources.policehorizontal;
                }
                else
                {
                    newBox.Image = Properties.Resources.policevertical;

                }
                //newBox.BackColor = Color.Yellow;
            }
            else
            {
                if (car.CarOrientation == Orientation.HORIZONTAL)
                {
                    newBox.Image = Properties.Resources.horizontalcar;
                }
                else
                {
                    newBox.Image = Properties.Resources.verticalcar;

                }

                //newBox.BackColor = baseColor;
            }

            Dictionary.Add(newBox, car.Id);
            PictureBoxes.Add(newBox);

            newBox.MouseClick += NewBox_MouseClick;
            this.Controls.Add(newBox);

            if (bringToFront) newBox.BringToFront();
        }
        public void AddWalls(Vector2 exitPosition)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if ((i == 0 || j == 0 || i == mapSize - 1 || j == mapSize - 1) && !(i == exitPosition.X && j == exitPosition.Y))
                    {

                        var newBox = new PictureBox();
                        newBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        newBox.Width = SquareSize;
                        newBox.Height = SquareSize;
                        newBox.Location = new Point(i * SquareSize, j * SquareSize);
                        newBox.Top = j * SquareSize;
                        newBox.BackColor = Color.Red;
                        newBox.BringToFront();
                        newBox.Image = Properties.Resources.box;
                        this.Controls.Add(newBox);
                        PictureBoxes.Add(newBox);
                        Dictionary.Add(newBox, "wall");
                        Walls.Add(newBox, new int[2] { i, j });

                    }
                }
            }
        }
        #endregion
        #region MOUSECLICK REGISTER
        private void NewBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (selectedPictureBox != null) selectedPictureBox.BackColor = Color.Transparent; // de-highlight current
            selectedPictureBox = sender as PictureBox;
            selectedPictureBox.BackColor = highlightColor;

            string id = GetCarIDFromPB(sender as PictureBox);
            controller.SelectCar(id);

        }
        #endregion
        #region WINDOW RESIZING
        private void Window_Resize(object sender, EventArgs e)
        {
            Width = Height - 25;
            foreach (PictureBox pb in PictureBoxes)
            {
                ResizePictureBox(pb); //TODO - needs to be more efficient. Dock? Set proportions?
            }
        }
        private void ResizePictureBox(PictureBox pb)
        {
            if (GetCarIDFromPB(pb) != "wall")
            {
                Car car = controller.ReturnCarFromID(GetCarIDFromPB(pb));

                pb.Width = car.Width * SquareSize;
                pb.Height = car.Height * SquareSize;
                pb.Location = new Point(car.XPos * SquareSize, car.YPos * SquareSize);
                pb.Top = car.YPos * SquareSize;
            }
            else
            {
                pb.Width = SquareSize;
                pb.Height = SquareSize;
                pb.Location = new Point(Walls[pb][0] * SquareSize, Walls[pb][1] * SquareSize);
                pb.Top = Walls[pb][1] * SquareSize;
            }
        }
        #endregion

        private string GetCarIDFromPB(PictureBox pictureBox)
        {
            return Dictionary[pictureBox];
        }

        #region EVENTS FROM MODEL
        public void carSelected(IModel m, ModelEventArgs e)
        {
        }
        public void carsAdded(IModel m, ModelEventArgs e)
        {
            mapSize = e.newMapSize;
            AddWalls(e.newExitPosition);
            foreach (Car car in e.newcars)
            {
                AddCar(car, false);
            }
        }
        public void mapReseted(IModel m, ModelEventArgs e)
        {
            foreach (var pb in PictureBoxes)
            {
                if (Dictionary[pb] != "wall")
                {

                    pb.Dispose();
                }
            }
            foreach (Car car in e.newcars)
            {
                AddCar(car, false);
            }
        }
        public void carMoved(IModel m, ModelEventArgs e)
        {
            selectedPictureBox.Location = new Point(e.newLocation.X * SquareSize, e.newLocation.Y * SquareSize);
        }
        public void gameOver(IModel m, ModelEventArgs e)
        {
            string message = "YOU WON";
            ShowMessage(message);
        }
        #endregion
        #region MESSAGE BOXES
        private void ShowHelp()
        {
            string message = "Welcome to Rush Hour!\nGet the Police out before it's too late!\nSelect car with mouse, move with WASD.\nTo see this box again, press H.";
            string title = "GAMEPLAY";
            MessageBox.Show(message, title);
        }
        private void ShowMessage(string message)
        {
            string title = "title";
            MessageBox.Show(message, title);
        }
        #endregion

    }
}
