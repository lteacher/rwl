using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;

using RobotInitial.View;
using RobotInitial.ViewModel;
using RobotInitial.Services;

namespace RobotInitial
{
    /// <summary>
    /// App class
    /// 
    /// Entry point to the application.
    /// 
    /// Based on the demo application from the MVVM tutorial
    /// at http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
	public partial class App : Application
	{

        public App() : base()
        {
            ServiceLocator.RegisterService<IUndoService>(new UndoService());
        }
        /// <summary>
        /// OnStartup Event Handler
        /// 
        /// Loads the MainWindow class and binds the ViewModel to the View.
        /// Also adds a handler to handle the MainWindow close event.
        /// 
        /// TODO: Add some code to prompt for save when application closes.
        /// </summary>
        /// <param name="e"></param>
        protected override void  OnStartup(StartupEventArgs e)
        {
           // RobotInitial.Model.Test.test();
 	        base.OnStartup(e);

            MainWindowView window = new MainWindowView();
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