using WinUIEx;
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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Composition.SystemBackdrops;
using WinRT;
using Microsoft.UI.Windowing;
using WinRT.Interop;
using Microsoft.UI;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MCInstaller
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : WindowEx
	{
		public MainWindow()
		{
			this.InitializeComponent();
			TrySetMicaBackdrop();
			SetCustomTitleBar();
		}

		UISettings UISettings = new UISettings();
		private void SetCustomTitleBar()
        {
			var RootUI = (FrameworkElement)Content;

			if(AppWindowTitleBar.IsCustomizationSupported())
            {
				AppWindow appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this)));
				var titleBar = appWindow.TitleBar;
				titleBar.ExtendsContentIntoTitleBar = true;
				void SetColor()
                {
					appWindow.Title = "MCInstaller";
					titleBar.ExtendsContentIntoTitleBar = true;
					titleBar.ButtonBackgroundColor = Colors.Transparent;
					titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
					titleBar.ButtonInactiveForegroundColor = UISettings.GetColorValue(UIColorType.Accent);
					titleBar.ButtonHoverBackgroundColor = ((SolidColorBrush)App.Current.Resources.ThemeDictionaries["SystemControlBackgroundListLowBrush"]).Color;
					titleBar.ButtonPressedBackgroundColor = ((SolidColorBrush)App.Current.Resources.ThemeDictionaries["SystemControlBackgroundListMediumBrush"]).Color;
				}
				RootUI.ActualThemeChanged += (_, _) => SetColor();
				SetColor();
            }
            else
            {
                ExtendsContentIntoTitleBar = true;
				SetTitleBar(AppTitleBar);
            }
		}

		WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See separate sample below for implementation
		MicaController m_micaController;
		SystemBackdropConfiguration m_configurationSource;

		bool TrySetMicaBackdrop()
		{
			if (MicaController.IsSupported())
			{
				m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
				m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

				// Hooking up the policy object
				m_configurationSource = new SystemBackdropConfiguration();
				this.Activated += Window_Activated;
				this.Closed += Window_Closed;
				((FrameworkElement)this.Content).ActualThemeChanged += Window_ThemeChanged;

				// Initial configuration state.
				m_configurationSource.IsInputActive = true;
				SetConfigurationSourceTheme();

				m_micaController = new MicaController();

				// Enable the system backdrop.
				// Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
				m_micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
				m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
				return true; // succeeded
			}
			return false; // Mica is not supported on this system
		}

		private void Window_Activated(object sender, WindowActivatedEventArgs args)
		{
			m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
		}

		private void Window_Closed(object sender, WindowEventArgs args)
		{
			// Make sure any Mica/Acrylic controller is disposed so it doesn't try to
			// use this closed window.
			if (m_micaController != null)
			{
				m_micaController.Dispose();
				m_micaController = null;
			}
			this.Activated -= Window_Activated;
			m_configurationSource = null;
		}

		private void Window_ThemeChanged(FrameworkElement sender, object args)
		{
			if (m_configurationSource != null)
			{
				SetConfigurationSourceTheme();
			}
		}

		private void SetConfigurationSourceTheme()
		{
			switch (((FrameworkElement)this.Content).ActualTheme)
			{
				case ElementTheme.Dark: m_configurationSource.Theme = SystemBackdropTheme.Dark; break;
				case ElementTheme.Light: m_configurationSource.Theme = SystemBackdropTheme.Light; break;
				case ElementTheme.Default: m_configurationSource.Theme = SystemBackdropTheme.Default; break;
			}
		}

        private void PageNavigator_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
			var selectedItemTag = (NavigationViewItem)args.SelectedItem;
			if (args.IsSettingsSelected)
			{
				rootFrame.Navigate(typeof(Settings));
			}
			else
			{
				switch (selectedItemTag.Tag.ToString())
				{
					case "H":
                        rootFrame.Navigate(typeof(HomePage));
						break;
					case "I":
						rootFrame.Navigate(typeof(Installer));
						break;
					case "U":
						rootFrame.Navigate(typeof(Uninstaller));
						break;
				}
			}
		}

        private void PageNavigator_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
			if (rootFrame.CanGoBack)
				rootFrame.GoBack();
        }
    }
}