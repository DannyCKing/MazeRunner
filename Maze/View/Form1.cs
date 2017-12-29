using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Maze.Model;
using System.Threading;

namespace Maze
{
    public partial class Form1 : Form
    {

        MazeBoardModel gameModel;

        static AutoResetEvent wait = new AutoResetEvent(false);

        public Form1()
        {
            gameModel = new MazeBoardModel(50, 50, PaintMovingPieces);

            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(0, 0);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int sq_width = (int)1900.0 / gameModel.Width;
            int sq_height = (int)1050.0 / gameModel.Height;

            // If there is an image and it has a location,  
            // paint it when the Form is repainted. 
            base.OnPaint(e);

            System.Drawing.SolidBrush darkBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            System.Drawing.SolidBrush startBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
            System.Drawing.SolidBrush endBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            System.Drawing.Graphics formGraphics;
            Pen pen = new Pen(darkBrush);

			if (gameModel.Width >= 30 && gameModel.Height >= 30)
			{
				pen.Width = 1;
			}
			if (gameModel.Width >= 20 && gameModel.Height >= 20)
			{
				pen.Width = 2;
			}
			else if (gameModel.Width >= 10 && gameModel.Height >= 10)
			{
				pen.Width = 4;
			}
			else if (gameModel.Width > 5 && gameModel.Height > 5)
			{
				pen.Width = 6;
			}
			else if (gameModel.Width > 2 && gameModel.Height > 2)
			{
				pen.Width = 10;
			}
			else
			{
				pen.Width = 1;
			}
			formGraphics = this.CreateGraphics();

            for (int i = 0; i < gameModel.Width; i++)
            {
                for (int j = 0; j < gameModel.Height; j++)
                {
                    if (gameModel.board[i, j].IsStart)
                    {
                        formGraphics.FillRectangle(startBrush, new Rectangle(i * sq_width, j * sq_height, sq_width, sq_height));
                    }
                    if (gameModel.board[i, j].IsEnd)
                    {
                        formGraphics.FillRectangle(endBrush, new Rectangle(i * sq_width, j * sq_height, sq_width, sq_height));
                    }

                    //Paint Edges
                    if (gameModel.board[i, j].right == null)
                    {
                        formGraphics.DrawLine(pen, new Point(i * sq_width + sq_width, j * sq_height), new Point(i * sq_width + sq_width, j * sq_height + sq_height));
                    }
                    //Paint Edges
                    if (gameModel.board[i, j].down == null)
                    {
                        formGraphics.DrawLine(pen, new Point(i * sq_width, j * sq_height + sq_height), new Point(i * sq_width + sq_width, j * sq_height + sq_height));
                    }

                }
            }

			// When board is ready to be drawn
            gameModel.Begin();
        }

        private void PaintMovingPieces(int i, int j)
        {
            int sq_width = (int)1900.0 / gameModel.Width;
            int sq_height = (int)1050.0 / gameModel.Height;

            // If there is an image and it has a location,  
            // paint it when the Form is repainted. 

            System.Drawing.SolidBrush visitedBrush = new System.Drawing.SolidBrush(System.Drawing.Color.LightSkyBlue);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();

            formGraphics.FillRectangle(visitedBrush, new Rectangle(i * sq_width +1, j * sq_height + 1, sq_width - 2, sq_height - 2));
        }

        private void OnFormClosing(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
}
