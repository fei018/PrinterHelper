using System;
using System.Windows;

namespace PrinterHelper
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length <= 0)
            {
                return;
            }

            new ArgumentMode().Run(e.Args);
        }
    }
}
