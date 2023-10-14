using Civil3DExtractSurfacesFromSolid.ViewModels;
using System;
using System.Windows;

namespace Civil3DExtractSurfacesFromSolid
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel vm = new MainWindowViewModel();
            vm.WindowCloseHandler += (s, e) => this.Close();
            DataContext = vm;
        }
    }
}