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
using TicTacToe;

namespace TicTacToe_in_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ImageMatrix ImageMatrix { get; set; }
        public GameManager Game { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Game = new GameManager(3);
           
            ImageMatrix = new ImageMatrix(gr_map, Game.Map.MapArray, 64, 64);
            ImageMatrix.OnButtonClicked += bn_game_map_Clicked;
            ImageMatrix.Initialize(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)));

            ImageMatrix.AddImage((int)Brick.Type.None, "Sprites/None.png");
            ImageMatrix.AddImage((int)Brick.Type.Circle, "Sprites/Circle.png");
            ImageMatrix.AddImage((int)Brick.Type.Cross, "Sprites/Cross.png");

            ImageMatrix.UpdateAll();

            tbl_message1.Text = Game.GetWinMessage();
            tbl_message2.Text = Game.GetBrickMessage();
        }

        private void bn_game_map_Clicked(object sender, EventArgs e)
        {
            if (Game.CheckWin() != WinType.None)
                Game.Reset();
            else
                Game.SetBrick(ImageMatrix.ClickedCellY, ImageMatrix.ClickedCellX);

            tbl_message1.Text = Game.GetWinMessage();
            tbl_message2.Text = Game.GetBrickMessage();

            ImageMatrix.UpdateAll();
        }

        private void bn_reset_Click(object sender, RoutedEventArgs e)
        {
            Game.Reset();
            ImageMatrix.UpdateAll();
        }
    }
}
