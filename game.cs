using System;
using System.Drawing;
using System.Windows.Forms;

public class PongGame : Form
{
    private int PaddleWidth, PaddleHeight, BallSize;
    private int leftPaddleY, rightPaddleY, ballX, ballY, ballSpeedX, ballSpeedY;
    private int leftScore = 0, rightScore = 0;
    private int paddleSpeed = 10;
    private const int winScore = 10;
    private bool leftUp, leftDown, rightUp, rightDown;
    private Timer gameTimer;
    private int maxSpeed = 20;

    public PongGame()
    {
        ballSpeedX = 5;
        ballSpeedY = 5;

        this.Text = "Pong Game";
        this.Size = new Size(800, 600);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;

        this.KeyDown += new KeyEventHandler(OnKeyDown);
        this.KeyUp += new KeyEventHandler(OnKeyUp);

        gameTimer = new Timer();
        gameTimer.Interval = 16;
        gameTimer.Tick += new EventHandler(OnGameTick);
        gameTimer.Start();

        this.Resize += new EventHandler(OnResize);
        OnResize(this, EventArgs.Empty);
    }

    private void OnResize(object sender, EventArgs e)
    {
        float formWidth = this.ClientSize.Width;
        float formHeight = this.ClientSize.Height;

        PaddleHeight = (int)(formHeight * 0.2);
        PaddleWidth = (int)(formWidth * 0.02);

        BallSize = (int)(formWidth * 0.03);

        ballSpeedX = (int)(Math.Min(formWidth / 50, maxSpeed));
        ballSpeedY = (int)(Math.Min(formHeight / 50, maxSpeed));

        leftPaddleY = (this.ClientSize.Height - PaddleHeight) / 2;
        rightPaddleY = (this.ClientSize.Height - PaddleHeight) / 2;
        ballX = this.ClientSize.Width / 2;
        ballY = this.ClientSize.Height / 2;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W) leftUp = true;
        if (e.KeyCode == Keys.S) leftDown = true;
        if (e.KeyCode == Keys.Up) rightUp = true;
        if (e.KeyCode == Keys.Down) rightDown = true;
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.W) leftUp = false;
        if (e.KeyCode == Keys.S) leftDown = false;
        if (e.KeyCode == Keys.Up) rightUp = false;
        if (e.KeyCode == Keys.Down) rightDown = false;
    }

    private void OnGameTick(object sender, EventArgs e)
    {
        if (leftUp && leftPaddleY > 0) leftPaddleY -= paddleSpeed;
        if (leftDown && leftPaddleY < this.ClientSize.Height - PaddleHeight) leftPaddleY += paddleSpeed;
        if (rightUp && rightPaddleY > 0) rightPaddleY -= paddleSpeed;
        if (rightDown && rightPaddleY < this.ClientSize.Height - PaddleHeight) rightPaddleY += paddleSpeed;

        ballX += ballSpeedX;
        ballY += ballSpeedY;

        if (ballY <= 0 || ballY >= this.ClientSize.Height - BallSize)
        {
            ballSpeedY = -ballSpeedY;
        }

        if (ballX <= PaddleWidth && ballY >= leftPaddleY && ballY <= leftPaddleY + PaddleHeight)
        {
            ballSpeedX = -ballSpeedX;
            IncreaseBallSpeed();
        }
        if (ballX >= this.ClientSize.Width - PaddleWidth - BallSize && ballY >= rightPaddleY && ballY <= rightPaddleY + PaddleHeight)
        {
            ballSpeedX = -ballSpeedX;
            IncreaseBallSpeed();
        }

        if (ballX <= 0)
        {
            rightScore++;
            ResetBall();
        }
        if (ballX >= this.ClientSize.Width - BallSize)
        {
            leftScore++;
            ResetBall();
        }

        if (leftScore >= winScore || rightScore >= winScore)
        {
            gameTimer.Stop();
            MessageBox.Show($"Game Over! {(leftScore >= winScore ? "Left" : "Right")} player wins!", "Game Over");
            this.Close();
        }

        this.Invalidate();
    }

    private void IncreaseBallSpeed()
    {
        if (ballSpeedX < maxSpeed && ballSpeedX > 0) ballSpeedX++;
        if (ballSpeedX > -maxSpeed && ballSpeedX < 0) ballSpeedX--;
        if (ballSpeedY < maxSpeed && ballSpeedY > 0) ballSpeedY++;
        if (ballSpeedY > -maxSpeed && ballSpeedY < 0) ballSpeedY--;
    }

    private void ResetBall()
    {
        ballX = this.ClientSize.Width / 2;
        ballY = this.ClientSize.Height / 2;
        ballSpeedX = 5;
        ballSpeedY = 5;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        e.Graphics.FillRectangle(Brushes.Blue, 0, leftPaddleY, PaddleWidth, PaddleHeight);
        e.Graphics.FillRectangle(Brushes.Blue, this.ClientSize.Width - PaddleWidth, rightPaddleY, PaddleWidth, PaddleHeight);
        e.Graphics.FillEllipse(Brushes.Red, ballX, ballY, BallSize, BallSize);

        string scoreText = $"{leftScore} - {rightScore}";
        var font = new Font("Arial", 24);
        var textSize = e.Graphics.MeasureString(scoreText, font);
        e.Graphics.FillRectangle(Brushes.Black, (this.ClientSize.Width - textSize.Width) / 2 - 10, 10, textSize.Width + 20, textSize.Height + 10);
        e.Graphics.DrawString(scoreText, font, Brushes.White, (this.ClientSize.Width - textSize.Width) / 2, 10);
    }

    public static void Main()
    {
        Application.Run(new PongGame());
    }
}
