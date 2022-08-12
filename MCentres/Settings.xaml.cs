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
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MCInstaller
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Settings : Page
	{
		public Settings()
		{
			this.InitializeComponent();
			AppVer.Text = "Version 5.1.047";
		}

		private async void ThemeChooser_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int ThemeIndex = ThemeChooser.SelectedIndex;
            switch (ThemeIndex)
            {
                case 0:
                    try
                    {
                        (this.XamlRoot.Content as FrameworkElement).RequestedTheme = ElementTheme.Light;
                    }
                    catch (NullReferenceException ex)
                    {
                        ContentDialog themeChangeError = new ContentDialog()
                        {
                            Title = "Error changing theme",
                            Content = ex.Message.ToString(),
                            CloseButtonText = "Close"
                        };
                        await themeChangeError.ShowAsync();
                    }
                    break;
                case 1:
                    try
                    {
                        (this.XamlRoot.Content as FrameworkElement).RequestedTheme = ElementTheme.Dark;
                    }
                    catch (NullReferenceException ex)
                    {
                        ContentDialog themeChangeError = new ContentDialog()
                        {
                            Title = "Error changing theme",
                            Content = ex.Message.ToString(),
                            CloseButtonText = "Close"
                        };
                        await themeChangeError.ShowAsync();
                    }
                    break;
				case 2:
                    try
                    {
                        (this.XamlRoot.Content as FrameworkElement).RequestedTheme = ElementTheme.Default;
                    }
                    catch (NullReferenceException ex)
                    {
                        ContentDialog themeChangeError = new ContentDialog()
                        {
                            Title = "Error changing theme",
                            Content = ex.Message.ToString(),
                            CloseButtonText = "Close"
                        };
                        await themeChangeError.ShowAsync();
                    }
                    break;
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			if (rootFrame.CanGoBack)
				rootFrame.GoBack();
		}
	}
}
