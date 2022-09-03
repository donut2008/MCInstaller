using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MCInstaller
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        private readonly MainWindow _mw = new MainWindow();
        private readonly Installer _ins = new Installer();
        public Settings()
        {
            this.InitializeComponent();
            AppVer.Text = "Version 6.1.095_fix-error";
            MicaStatus.Text = "Mica backdrop status: " + _mw.MicaInfo.ToString();
            WinVer.Text = "Windows version: " + _ins.WinBuild;
        }

        private void ThemeChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int ThemeIndex = ThemeChooser.SelectedIndex;
            switch (ThemeIndex)
            {
                case 0:
                    (this.XamlRoot.Content as FrameworkElement).RequestedTheme = ElementTheme.Light;
                    break;
                case 1:
                    (this.XamlRoot.Content as FrameworkElement).RequestedTheme = ElementTheme.Dark;
                    break;
                case 2:
                    (this.XamlRoot.Content as FrameworkElement).RequestedTheme = ElementTheme.Default;
                    break;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (RootFrame.CanGoBack)
                RootFrame.GoBack();
        }
    }
}
