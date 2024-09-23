using Avalonia.Controls;
using Demo3.Models;
using System;
using System.Linq;

namespace Demo3
{
    public partial class MainWindow : Window
    {
        int filtr = 0;
        int sort = 0;
        string search = "";
        public MainWindow()
        {
            InitializeComponent();
            Filtr.ItemsSource = Actions.Manufacturers;
            Filtr.SelectedIndex = 0;
            Sort.SelectedIndex = 0;
        }
        public void Update()
        {
            ProductList.ItemsSource = Actions.Products.ToList();
            Amount.Text = Actions.Amount.ToString();
        }
        private void SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).Name == "Filtr")
            {
                filtr = Filtr.SelectedIndex;
            }
            else
            {
                sort = Sort.SelectedIndex;
            }
            Actions.ChangeProductList(search, filtr, sort);
            Update();
        }

        private void Add(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new EditWindow().Show();
            this.Close();

        }

        private void Change(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            new EditWindow(Int32.Parse((sender as Border).Tag.ToString())).Show();
            this.Close();
        }
    }
}