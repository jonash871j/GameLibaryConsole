using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TicTacToeLogic;

public class ImageMatrix
{
    private struct FilepathHolder
    {
        public int id;
        public string filepath;

        public FilepathHolder(int id, string filepath)
        {
            this.id = id;
            this.filepath = filepath;
        }
    }

    private Brick[,] idArray;

    private List<FilepathHolder> filepathHolders;
    private Button[,] buttonArray;
    private Grid grid;

    private int cellXAmount, cellYAmount;
    private int cellWidth, cellHeight;
    private int width, height;

    private int clickedCellIndex = 0;
    private int clickedCellX = 0;
    private int clickedCellY = 0;

    public int CellXAmount
    {
        get { return cellXAmount; }
        private set { cellXAmount = value; }
    }
    public int CellYAmount
    {
        get { return cellYAmount; }
        private set { cellYAmount = value; }
    }
    public int CellWidth
    {
        get { return cellWidth; }
        private set { cellWidth = value; }
    }
    public int CellHeight
    {
        get { return cellHeight; }
        private set { cellHeight = value; }
    }
    public int Width
    {
        get { return width; }
        private set { width = value; }
    }
    public int Height
    {
        get { return height; }
        private set { height = value; }
    }

    public int ClickedCellIndex
    {
        get { return clickedCellIndex; }
        private set { clickedCellIndex = value; }
    }
    public int ClickedCellX
    {
        get { return clickedCellX; }
        private set { clickedCellX = value; }
    }
    public int ClickedCellY
    {
        get { return clickedCellY; }
        private set { clickedCellY = value; }
    }

    public event EventHandler OnButtonClicked;

    public ImageMatrix(Grid grid, Brick[,] idArray, int cellWidth, int cellHeight)
    {
        CellWidth = cellWidth;
        CellHeight = cellHeight;
        CellXAmount = idArray.GetLength(1);
        CellYAmount = idArray.GetLength(0);
        Width = CellWidth * CellXAmount;
        Height = CellHeight * CellYAmount;

        this.idArray = idArray;
        filepathHolders = new List<FilepathHolder>();
        buttonArray = new Button[cellYAmount, cellXAmount];

        this.grid = grid;
        grid.Width = Width;
        grid.Height = Height;
    }

    public void Delete()
    {
        filepathHolders = null;
        buttonArray = null;
        grid.Children.Clear();
        grid.RowDefinitions.Clear();
        grid.ColumnDefinitions.Clear();
    }

    public void UpdateArray(Brick[,] idArray)
    {
        int testCellXAmount = idArray.GetLength(1);
        int testCellYAmount = idArray.GetLength(0);

        if ((testCellXAmount != CellXAmount) || (testCellYAmount != CellYAmount))
            return;

        this.idArray = idArray;
        UpdateAll();
    }

    // Used to initialize button array, sets button properties and draw it to window
    public void Initialize(Brush background, Brush foreground)
    {
        //Image image;

        for (int y = 0; y < cellYAmount; y++)
        {
            RowDefinition gridY = new RowDefinition();
            gridY.Height = new GridLength(CellHeight);

            for (int x = 0; x < cellXAmount; x++)
            {
                ColumnDefinition gridX = new ColumnDefinition();
                gridX.Width = new GridLength(CellWidth);

                buttonArray[y, x] = new Button();
                buttonArray[y, x].Background = background;
                buttonArray[y, x].Foreground = foreground;
                buttonArray[y, x].BorderThickness = new Thickness(0.5);
                buttonArray[y, x].BorderBrush = foreground;

                // Event
                buttonArray[y, x].Tag = y * CellXAmount + x;
                buttonArray[y, x].Click += Clicked;

                grid.Children.Add(buttonArray[y, x]);
                Grid.SetColumn(buttonArray[y, x], x);
                Grid.SetRow(buttonArray[y, x], y);
                grid.ColumnDefinitions.Add(gridX);
            }
            grid.RowDefinitions.Add(gridY);
        }
    }

    /// <summary>
    /// Used to add image and give it a id
    /// </summary>
    public void AddImage(int id, string filepath)
    {
        FilepathHolder filepathHolder = new FilepathHolder(id, filepath);
        filepathHolders.Add(filepathHolder);
    }

    /// <summary>
    /// Used to update specfic cell
    /// </summary>
    public void UpdateSpecific(int x, int y)
    {
        if (x < 0) x = 0;
        if (y < 0) y = 0;

        if (x > CellXAmount - 1) x = CellXAmount - 1;
        if (y > CellYAmount - 1) y = CellYAmount - 1;

        for (int i = 0; i < filepathHolders.Count; i++)
        {
            if ((int)idArray[y, x].BrickType == filepathHolders[i].id)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(filepathHolders[i].filepath, UriKind.Relative));
                buttonArray[y, x].Content = image;
                return;
            }
        }
        buttonArray[y, x].Content = (int)idArray[y, x].BrickType;
    }

    /// <summary>
    /// Used to update all cells
    /// </summary>
    public void UpdateAll()
    {
        for (int y = 0; y < CellYAmount; y++)
        {
            for (int x = 0; x < CellXAmount; x++)
            {
                UpdateSpecific(x, y);
            }
        }
    }

    private void Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;

        ClickedCellIndex = Convert.ToInt32(((Button)sender).Tag);
        ClickedCellY = ClickedCellIndex / (CellXAmount);
        ClickedCellX = ClickedCellIndex % (CellXAmount);
        OnButtonClicked(sender, e);
    }
}