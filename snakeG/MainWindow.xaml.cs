using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace snakeG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<gridV, ImageSource> gridValToImg = new()
        {
            {gridV.Empty, ImageZ.Empty },
            {gridV.Snake, ImageZ.Body },
            {gridV.Food, ImageZ.Food }
        };

        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.up, 0 },
            {Direction.right, 90 },
            {Direction.down, 180 },
            {Direction.left, -90 }
        };
        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private gamestate gameStatsse;
        private bool gameRunning;


        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameStatsse = new gamestate(rows, cols);
        }

        private async Task RunGame()
        {
            Draw();
            await showCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await gameloop();
            await GameOver();
            gameStatsse = new gamestate(rows, cols);
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameStatsse.gameover)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.A:
                    gameStatsse.changeDirection(Direction.left);
                    break;
                case Key.W:
                    gameStatsse.changeDirection(Direction.up);
                    break;
                case Key.S:
                    gameStatsse.changeDirection(Direction.down);
                    break;
                case Key.D:
                    gameStatsse.changeDirection(Direction.right);
                    break;
            }
        }

        private async Task gameloop()
        {
            while (!gameStatsse.gameover)
            {
                await Task.Delay(100);
                gameStatsse.move();
                Draw();
            }
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    Image images2 = new Image
                    {
                        Source = ImageZ.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };

                    images[i, j] = images2;
                    GameGrid.Children.Add(images2);
                }
            }
            return images;
        }

        private void Draw()
        {
            DrawGrid();
            drawSnakeHead();
            ScoreText.Text = $"SCORE {gameStatsse.score}";
        }

        private void DrawGrid()
        {
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    gridV gridValue = gameStatsse.Grid[i, j];
                    gridImages[i, j].Source = gridValToImg[gridValue];
                    gridImages[i, j].RenderTransform = Transform.Identity;
                }
            }
        }

        private void drawSnakeHead()
        {
            position headPos = gameStatsse.headPosition();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = ImageZ.Head;

            int rotation = dirToRotation[gameStatsse.dirc];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task DrawDeadSnake()
        {
            List<position> pozicije = new List<position>(gameStatsse.snakePosition());
            for(int i = 0; i < pozicije.Count; i++)
            {
                position pos = pozicije[i];
                ImageSource source = (i == 0) ? ImageZ.DeadHead : ImageZ.DeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }
        private async Task showCountDown()
        {
            for(int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task GameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(200);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }
    }
}
