using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using System;
using Windows.Storage.Pickers;
using WinRT;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HeicJpgBatchC
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        private DesktopAcrylicController acrylic;
        private SystemBackdropConfiguration acrylicConfig;


        public MainWindow()
        {
            InitializeComponent();


            //Title = "[HeicJpgBatchC] HEIC to JPG Batch Converter";

            //Use MainWindow InitializeAcrylic or Window.SystemBackdrop, not both
            InitializeAcrylic();

            AppWindow.Resize(new Windows.Graphics.SizeInt32(1400, 1400));

            //ExtendsContentIntoTitleBar = true;

            //AppWindow.TitleBar.ButtonBackgroundColor =
            //    Microsoft.UI.Colors.Transparent;

            //AppWindow.TitleBar.ButtonInactiveBackgroundColor =
            //    Microsoft.UI.Colors.Transparent;


        }

        private void InitializeAcrylic()
        {
            if (!DesktopAcrylicController.IsSupported())
                return;

            DispatcherQueue.EnsureSystemDispatcherQueue();

            acrylicConfig = new SystemBackdropConfiguration
            {
                IsInputActive = true,
                Theme = SystemBackdropTheme.Dark
            };

            acrylic = new DesktopAcrylicController
            {
                Kind = DesktopAcrylicKind.Base,
                TintColor = Windows.UI.Color.FromArgb(255, 0, 0, 0),
                TintOpacity = 0.01f,
                LuminosityOpacity = 0.1f
            };

            acrylic.AddSystemBackdropTarget(
                this.As<ICompositionSupportsSystemBackdrop>());

            acrylic.SetSystemBackdropConfiguration(acrylicConfig);

            Closed += (sender, args) =>
            {
                acrylic?.Dispose();
                acrylic = null;
            };
        }


        private async void btnBrowse_Click(
    object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            FolderPicker picker = new FolderPicker();

            picker.FileTypeFilter.Add("*");

            // Associate picker with this window
            IntPtr hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(picker, hwnd);

            var folder = await picker.PickSingleFolderAsync();

            if (folder != null)
            {
                txtDirectory.Text = folder.Path;
            }
        }
    }
}
