using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using RobotInitial.ViewModel;

namespace RobotInitial
{
    // Based on the demo application from the MVVM tutorial
    // at http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    //
	public partial class App : Application
	{
        protected override void  OnStartup(StartupEventArgs e)
        {
 	        base.OnStartup(e);

            MainWindow window = new MainWindow();

            var viewModel = new MainWindowViewModel();

            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                window.Close();
            };

            viewModel.RequestClose += handler;


            window.DataContext = viewModel;

            window.Show();
        }
	}
}