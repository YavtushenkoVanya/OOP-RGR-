using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class Form1 : Form
    {
        private PictureBox paddle;
        private PictureBox ball;
        private List<PictureBox> bricks;
        private Timer gameTimer;
        private int ballSpeedX = 4;
        private int ballSpeedY = 4;
        private int paddleSpeed = 10;
        private bool leftArrowPressed = false;
        private bool rightArrowPressed = false;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Ініціалізація ракетки
            paddle = new PictureBox
            {
                Size = new Size(100, 20),
                BackColor = Color.Blue,
                Top = ClientSize.Height - 30,
                Left = (ClientSize.Width / 2) - 50
            };
            Controls.Add(paddle);

            // Ініціалізація кульки
            ball = new PictureBox
            {
                Size = new Size(20, 20),
                BackColor = Color.Red,
                Top = paddle.Top - 20,
                Left = paddle.Left + 40
            };
            Controls.Add(ball);

            // Ініціалізація блоків
            bricks = new List<PictureBox>();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    var brick = new PictureBox
                    {
                        Size = new Size(60, 20),
                        BackColor = Color.Green,
                        Top = (i * 22) + 50,
                        Left = (j * 62) + 10
                    };
                    bricks.Add(brick);
                    Controls.Add(brick);
                }
            }

            // Ініціалізація таймера гри
            gameTimer = new Timer
            {
                Interval = 20
            };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            // Обробка подій клавіатури
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                leftArrowPressed = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                rightArrowPressed = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                leftArrowPressed = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                rightArrowPressed = false;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // Рух ракетки
            if (leftArrowPressed && paddle.Left > 0)
            {
                paddle.Left -= paddleSpeed;
            }
            if (rightArrowPressed && paddle.Right < ClientSize.Width)
            {
                paddle.Left += paddleSpeed;
            }

            // Рух кульки
            ball.Left += ballSpeedX;
            ball.Top += ballSpeedY;

            // Відбиття кульки від стінок
            if (ball.Left <= 0 || ball.Right >= ClientSize.Width)
            {
                ballSpeedX = -ballSpeedX;
            }
            if (ball.Top <= 0)
            {
                ballSpeedY = -ballSpeedY;
            }

            // Відбиття кульки від ракетки
            if (ball.Bounds.IntersectsWith(paddle.Bounds))
            {
                ballSpeedY = -ballSpeedY;
            }

            // Відбиття кульки від блоків
            foreach (var brick in bricks)
            {
                if (ball.Bounds.IntersectsWith(brick.Bounds))
                {
                    Controls.Remove(brick);
                    bricks.Remove(brick);
                    ballSpeedY = -ballSpeedY;
                    break;
                }
            }

            // Перевірка на програш
            if (ball.Top >= ClientSize.Height)
            {
                gameTimer.Stop();
                MessageBox.Show("Ви програли!");
            }
        }
    }
}
