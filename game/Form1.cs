using System;
using System.Drawing;
using System.Windows.Forms;

namespace game
{
    public partial class Form1 : Form
    {
        Bitmap Backbuffer;

        const int BallAxisSpeed = 0;

        Point BallPos = new Point(350, 350);
		Double BallSpeedX = 0;
		Double BallSpeedY = 0;
		bool[] keys = new bool[4] { false, false, false, false };
		double[] move = new double[2] { 0, 0 };
		const int BallSize = 50;

        public Form1()
        {
            InitializeComponent();

            this.SetStyle(
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.DoubleBuffer, true);

            Timer GameTimer = new Timer();
            GameTimer.Interval = 10;
            GameTimer.Tick += new EventHandler(GameTimer_Tick);
            GameTimer.Start();

            this.ResizeEnd += new EventHandler(Form1_CreateBackBuffer);
            this.Load += new EventHandler(Form1_CreateBackBuffer);
            this.Paint += new PaintEventHandler(Form1_Paint);

			this.KeyDown += new KeyEventHandler(tick_keys);
			this.KeyUp += new KeyEventHandler(tick_keysUp);
			FormBorderStyle = FormBorderStyle.None;
			WindowState = FormWindowState.Maximized;
        }

		void tick_keys(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left)
				keys[0] = true;
			if (e.KeyCode == Keys.Right)
				keys[1] = true;
			if (e.KeyCode == Keys.Up)
				keys[2] = true;
			if (e.KeyCode == Keys.Down)
				keys[3] = true;
			if (e.KeyCode == Keys.Escape)
				System.Windows.Forms.Application.Exit();
		}

		void tick_keysUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left)
				keys[0] = false;
			if (e.KeyCode == Keys.Right)
				keys[1] = false;
			if (e.KeyCode == Keys.Up)
				keys[2] = false;
			if (e.KeyCode == Keys.Down)
				keys[3] = false;
		}

		void movement()
		{
			double mx = 0;
			double my = 0;
			int movementSpeed = 5;
			if (keys[0] == true)
				my -= movementSpeed;
			if (keys[1] == true)
				my += movementSpeed;
			if (keys[2] == true)
				mx -= movementSpeed;
			if (keys[3] == true)
				mx += movementSpeed;
			move[0] = my;
			move[1] = mx;
		}

		void checkBounds()
		{
			if (BallPos.X < BallSize / 2) {
				BallSpeedX = -BallSpeedX;
				BallPos.X = BallSize / 2;
			}
			if (BallPos.Y < BallSize / 2) {
				BallSpeedY = -BallSpeedY;
				BallPos.Y = BallSize / 2;
			}
			if (BallPos.X > ClientSize.Width - (BallSize / 2)) {
				BallSpeedX = -BallSpeedX;
				BallPos.X = ClientSize.Width - (BallSize / 2);
			}
			if (BallPos.Y > ClientSize.Height - (BallSize / 2)) {
				BallSpeedY = -BallSpeedY;
				BallPos.Y = ClientSize.Height - (BallSize / 2);
			}
		}

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (Backbuffer != null)
            {
                e.Graphics.DrawImageUnscaled(Backbuffer, Point.Empty);
            }
        }

        void Form1_CreateBackBuffer(object sender, EventArgs e)
        {
            if (Backbuffer != null)
                Backbuffer.Dispose();

            Backbuffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        void Draw()
        {
            if (Backbuffer != null)
            {
                using (var g = Graphics.FromImage(Backbuffer))
                {
                    g.Clear(Color.White);
                    g.FillEllipse(Brushes.Black, BallPos.X - BallSize / 2, BallPos.Y - BallSize / 2, BallSize, BallSize);
					//double rot = Math.Atan2(BallPos.X-MousePosition.X, BallPos.Y-MousePosition.Y);
					g.FillRectangle(Brushes.Black, new Rectangle(BallPos.X, BallPos.Y, 10, 50));
                }

                Invalidate();
            }
        }

        void GameTimer_Tick(object sender, EventArgs e)
        {
			movement();
            BallPos.X += Convert.ToInt32(BallSpeedX);
            BallPos.Y += Convert.ToInt32(BallSpeedY);
            BallSpeedX += move[0];
            BallSpeedY += move[1];
			BallSpeedX *= 0.75;
			BallSpeedY *= 0.75;
			checkBounds();
			Draw();
			
        }

        private void Form1_Load(object sender, EventArgs e)
        {
			
		}

  }
}