using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GameOfLife
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Random rng = new Random();
        private int columnCount = 80;
        private int rowCount = 80;
        private Cell[,] cells;
        private bool[,] oldCellValues;
        private bool[,] newCellValues;

        public MainWindow()
        {
            InitializeComponent();
            Init();
            Randomize(null, null);
        }

        private void Init()
        {
            Cell cell;
            cells = new Cell[columnCount, rowCount];
            oldCellValues = new bool[columnCount, rowCount];
            newCellValues = new bool[columnCount, rowCount];

            for (int row = 0; row < rowCount; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });

                for (int column = 0; column < columnCount; column++)
                {
                    cell = new Cell();
                    grid.Children.Add(cell);
                    Grid.SetColumn(cell, column);
                    Grid.SetRow(cell, row);
                    cells[column, row] = cell;
                }
            }

            grid.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });

            Grid.SetColumn(panel, 0);
            Grid.SetColumnSpan(panel, columnCount);
            Grid.SetRow(panel, rowCount);
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            foreach (Cell c in cells) 
            {
                c.IsChecked = false;
            }
        }

        private void Randomize(object sender, RoutedEventArgs e)
        {
            foreach (Cell c in cells)
            {
                c.IsChecked = rng.Next(12) == 0;
            }
        }
        
        private void NextGeneration(object sender, RoutedEventArgs e)
        {
            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    oldCellValues[column, row] = cells[column, row].IsChecked;
                }
            }

            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    cells[column, row].IsChecked = CalculateNewValue(column, row);
                }
            }
        }

        private async void NextGenerationAsync(object sender, RoutedEventArgs e)
        {
            Stopwatch totalTimer = new Stopwatch();
            totalTimer.Start();

            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    oldCellValues[column, row] = cells[column, row].IsChecked;
                }
            }

            newCellValues = await NextGenerationAsyncInternal();

            Stopwatch uiTimer = new Stopwatch();
            uiTimer.Start();

            for (int row = 0; row < rowCount; row++)
            {
                for (int column = 0; column < columnCount; column++)
                {
                    cells[column, row].IsChecked = newCellValues[column, row];
                }
            }

            uiTimer.Stop();
            totalTimer.Stop();
            Console.WriteLine("UI duration: " + uiTimer.ElapsedMilliseconds + "ms");
            Console.WriteLine("Total duration: " + totalTimer.ElapsedMilliseconds + "ms");
        }

        private Task<bool[,]> NextGenerationAsyncInternal()
        {
            return Task.Run(() =>
            {
                bool[,] newGeneration = new bool[columnCount, rowCount];

                for (int row = 0; row < rowCount; row++)
                {
                    for (int column = 0; column < columnCount; column++)
                    {
                        newGeneration[column, row] = CalculateNewValue(column, row);
                    }
                }

                return newGeneration;
            });
        }

        private bool CalculateNewValue(int column, int row)
        {
            int neighboursCount = 0;

            for (int y = row - 1; y < row + 2; y++)
            {
                for (int x = column - 1; x < column + 2; x++)
                {
                    if (x == column && y == row)
                    {
                        continue;
                    }

                    if (GetCell(x, y))
                    {
                        neighboursCount += 1;
                    }

                    if (neighboursCount > 3)
                    {
                        return false;
                    }
                }
            }

            if (neighboursCount == 3 || (oldCellValues[column, row] && neighboursCount == 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetCell(int x, int y)
        {
            if (x < 0)
            {
                x = columnCount + x;
            }
            else if (x >= columnCount)
            {
                x = x - columnCount;
            }

            if (y < 0)
            {
                y = rowCount + y;
            }
            else if (y >= rowCount)
            {
                y = y - rowCount;
            }

            return oldCellValues[x, y];
        }
    }
}
