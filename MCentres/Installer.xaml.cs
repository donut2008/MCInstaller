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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http;
using Microsoft.Win32;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.ApplicationSettings;
using WinRT;
using WinRT.Interop;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MCInstaller
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Installer : Page
	{
		public string WinBuild, ScriptPath;
		private StorageFile _file;
		private string _validSHA = "C298ECC73D57C1BD141AA5733D54FDD92A5B001AE42A90C4D760A80001933B2B";

		private readonly MainWindow _mWindow = new MainWindow();
		
		public Installer()
		{
			this.InitializeComponent();
			RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
			var build = "." + rkey.GetValue("UBR");
			WindowsVersion.Text = Environment.OSVersion.Version.Build + build;
			WinBuild = WindowsVersion.Text;
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			RootFrame.Navigate(typeof(MainWindow));
		}

		private async void Install_Click(object sender, RoutedEventArgs e)
		{
			_file = null;
			Ps1Path.Text = "";
			InvalidSHAWarn.Visibility = Visibility.Collapsed;
			Ps1Selector.IsPrimaryButtonEnabled = true;
			await Ps1Selector.ShowAsync();
		}

		private async void PS1Picker_Click(object sender, RoutedEventArgs e)
		{
			Ps1Path.Text = "";
			InvalidSHAWarn.Visibility = Visibility.Collapsed;
			Ps1Selector.IsPrimaryButtonEnabled = true;
			FileOpenPicker ps1Picker = new();
			InitializeWithWindow.Initialize(ps1Picker, _mWindow.FindHWND());
			ps1Picker.ViewMode = PickerViewMode.Thumbnail;
			ps1Picker.SuggestedStartLocation = PickerLocationId.ComputerFolder;
			ps1Picker.FileTypeFilter.Add(".ps1");
			_file = await ps1Picker.PickSingleFileAsync();

			if (_file != null)
			{
				string FPath = _file.Path;
				Ps1Path.Text = FPath;
				if (GetSHA256Checksum(FPath) != _validSHA)
				{
					InvalidSHAWarn.Visibility = Visibility.Visible;
					Ps1Selector.IsPrimaryButtonEnabled = false;
				}
			}
		}

		private void Ps1Selector_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			ScriptPath = Ps1Path.Text;
		}

		public static string GetSHA256Checksum(string filename)
		{
			using (var sha256 = SHA256.Create())
			{
				using (var stream = File.OpenRead(filename))
				{
					var hash = sha256.ComputeHash(stream);
					return BitConverter.ToString(hash).Replace("-", "");
				}
			}
		}
	}
}