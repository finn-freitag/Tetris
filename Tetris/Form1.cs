using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tetris.GameLogic;

namespace Tetris
{
    public partial class Form1 : Form
    {
        GameBoard gameBoard;
        Renderer renderer;
        int elapsedTime = 0;
        bool increaseSpeed = true;
        int defaultInterval = 300;
        long Score = 0;

        public Form1()
        {
            InitializeComponent();

            gameBoard = new GameBoard(new Size(10, 20));
            renderer = new Renderer() { GameBoard = gameBoard, ImageSize = pictureBox1.Size };
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                gameBoard.MoveHorizontal(gameBoard.MovingTile, -1);
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                gameBoard.MoveHorizontal(gameBoard.MovingTile, 1);
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                if (gameBoard.IsSpaceDown(gameBoard.MovingTile))
                {
                    gameBoard.MoveVertical(gameBoard.MovingTile, 1);
                    Score++;
                    label1.Text = "Score: " + Score;
                }
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                gameBoard.Rotate(gameBoard.MovingTile);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (pictureBox1 == null || renderer == null)
                return;
            renderer.ImageSize = pictureBox1.Size;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gameBoard.Update())
            {
                pictureBox1.Image = renderer.Render();
                Score++;
                label1.Text = "Score: " + Score;
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("Game over! (Score: " + Score + ")");
                Score = 0;
                gameBoard = new GameBoard(new Size(10, 20));
                renderer = new Renderer() { GameBoard = gameBoard, ImageSize = pictureBox1.Size };
                timer1.Interval = defaultInterval;
                timer1.Start();
            }
            elapsedTime += timer1.Interval;
            if (elapsedTime > 10000)
            {
                elapsedTime = 0;
                if (increaseSpeed)
                    timer1.Interval = Math.Max(timer1.Interval - (int)numericUpDown1.Value, 5);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (renderer == null)
                return;
            renderer.ShowNextTile = (sender as ModCheckbox).Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            increaseSpeed = (sender as ModCheckbox).Checked;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
    }
}
