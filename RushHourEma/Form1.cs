﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RushHourEma
{
    public partial class Form1 : Form, IView, IModelObserver
    {
        IController controller;
        public event ViewHandler<IView> changed;
        // View will set the associated controller, this is how view is linked to the controller.
        public void SetController(IController cont)
        {
            controller = cont;
        }
        public Game game; // OUT
        public Dictionary<PictureBox, string> dictionary;
        string text; // OUT
        PictureBox selectedPB; // OUT
        PictureBox selectedPictureBox;

        Color bgdColor = Color.Red;
        Color baseColor = Color.Green;
        Color highlightColor = Color.White;

        public int mapSize;
        private int squareSize;

        public int SquareSize
        {
            get { return this.Width / mapSize; }
            set { squareSize = value; }
        }



        public Form1()
        {
            dictionary = new Dictionary<PictureBox, string>();
            InitializeComponent();
            Width = 400;
            Height = 425;
        }
        public void AddEntity(Car car, bool bringToFront)
        {
            var newBox = new PictureBox();
            newBox.SizeMode = PictureBoxSizeMode.StretchImage;
            newBox.Width = car.Width*SquareSize;
            newBox.Height = car.Height*SquareSize;
            newBox.Location = new Point(car.XPos*SquareSize, car.YPos*SquareSize);
            newBox.Top = car.YPos;
            newBox.BackColor = baseColor;
            dictionary.Add(newBox, car.Id);
            newBox.MouseClick += NewBox_MouseClick;
            this.Controls.Add(newBox);
            if (bringToFront) newBox.BringToFront();
        }
        private void NewBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (selectedPictureBox != null) selectedPictureBox.BackColor = baseColor; // de-highlight current
            selectedPictureBox = sender as PictureBox;
            selectedPictureBox.BackColor = highlightColor;
            //controller.ChangeValue();

            string id = GetCarIDFromPB(sender as PictureBox);
            controller.SelectCar(id);

            ShowMessage(id);
            
        }

        private string GetCarIDFromPB(PictureBox pictureBox)
        {
            return dictionary[pictureBox];
        }

        private void ShowHelp()
        {
            string message = "move - WASD\nload map - L\nreset map - R\nhelp - F1";
            string title = "Key Bindings";
            MessageBox.Show(message, title);
        }
        private void ShowMessage(string message)
        {
            string title = "title";
            MessageBox.Show(message, title);
        }
        private void Window_Resize(object sender, EventArgs e)
        {
            Width = Height - 25;
        }

        private void Window_Load(object sender, EventArgs e)
        {
            BackColor = Color.Red;
            // BackgroundImage = Resources.background;
            BackgroundImageLayout = ImageLayout.Tile;
            game = new Game();
            this.KeyPress += Form1_KeyPress;


            //ShowHelp();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                MessageBox.Show("HELLO");
            }
            controller.KeyPressed(e);
        }

        public void valueIncremented(IModel m, ModelEventArgs e)
        {
            text = "" + e.newval;
            MessageBox.Show(text);
        }
        public void carSelected(IModel m, ModelEventArgs e)
        {
            //selectedPB = e.newpb;
        }
        public void carsAdded(IModel m, ModelEventArgs e)
        {
            mapSize = e.newMapSize;
            foreach(Car car in e.newcars)
            {
                AddEntity(car, false);
            }
         
        }
    }
}