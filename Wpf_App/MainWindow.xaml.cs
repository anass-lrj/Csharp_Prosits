using System.Windows;
using System.Windows.Media;

namespace Wpf_App;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        MonTexte.Text = "Bravo, le code fonctionne !";
        MonTexte.Foreground = Brushes.Green;
    }
}