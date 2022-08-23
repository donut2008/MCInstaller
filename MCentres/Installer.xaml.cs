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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http;
using Microsoft.Win32;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MCInstaller
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Installer : Page
    {
        string currentbuild;
        static Uri requestUri = new Uri("https://github.com/donut2008/MCInstaller/");
        static HttpClient httpClient = new HttpClient();
        public Installer()
        {
            this.InitializeComponent();
            RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            var build = "." + rkey.GetValue("UBR").ToString();
            WindowsVersion.Text = Environment.OSVersion.Version.Build.ToString() + build;
            currentbuild = WindowsVersion.Text;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(MainWindow));
        }

        private async void Install_Click(object sender, RoutedEventArgs e)
        {
            string X64DL = "C:\\ProgramData\\MCInstaller\\DLLs\\x64\\" + currentbuild, X86DL = "C:\\ProgramData\\MCInstaller\\DLLs\\x86\\" + currentbuild;
            string X64Link = "https://github.comdonut2008/MCInstaller/";
            var headers = httpClient.DefaultRequestHeaders;
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                ContentDialog InvalidHeader = new ContentDialog
                {
                    Title = "Invalid header string",
                    Content = "Invalid header value: " + header,
                    CloseButtonText = "OK"
                };
                await InvalidHeader.ShowAsync();
            }
            header = "Mozilla/5.0 (Windows NT 10.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.0.0 Safari/537.36";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                ContentDialog invalidHeader = new ContentDialog
                {
                    Title = "Invalid header string",
                    Content = "Invalid header value: " + header,
                    CloseButtonText = "OK"
                };
                await invalidHeader.ShowAsync();
            }

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("x") + "Message: " + ex.Message;
                ContentDialog HttpError = new ContentDialog()
                {
                    Title = "Error",
                    Content = httpResponseBody,
                    CloseButtonText = "OK"
                };
                await HttpError.ShowAsync();
            }


        }

        public static async void DL_x64(string uri, string output)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out requestUri))
            {
                ContentDialog invalidOperationException = new ContentDialog()
                {
                    Title = "Invalid operation exception",
                    Content = "URI is invalid.",
                    CloseButtonText = "Close"
                };
            }

            byte[] fileBytes = await httpClient.GetByteArrayAsync(uri);
            File.WriteAllBytes(output, fileBytes);
        }

        public static async void DL_x86(string uri, string output)
        {
            if (!Uri.TryCreate(uri, UriKind.Absolute, out requestUri))
            {
                ContentDialog invalidOperationException = new ContentDialog()
                {
                    Title = "Invalid operation exception",
                    Content = "URI is invalid.",
                    CloseButtonText = "Close"
                };
            }

            byte[] fileBytes = await httpClient.GetByteArrayAsync(uri);
            File.WriteAllBytes(output, fileBytes);
        }
    }
}
