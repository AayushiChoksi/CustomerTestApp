using CustomersTestApp.ViewModels;
using System.Windows;

namespace CustomersTestApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}