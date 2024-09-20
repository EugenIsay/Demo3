using Avalonia.Controls;

namespace Demo3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ProductList.ItemsSource = Actions.Products;
        }
    }
}