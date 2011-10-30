using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows.Controls;
using RobotInitial.View;
using RobotInitial.ViewModel;
using System.Windows;

namespace RobotInitial.Behaviours {
	class StartStopPause : Behavior<Grid> {

		protected override void OnAttached() {
			base.OnAttached();

			if(AssociatedObject.Name == "StartButtonGrid") {
				AssociatedObject.MouseDown += new System.Windows.Input.MouseButtonEventHandler(DoStartButtonAction);

			}
			else if(AssociatedObject.Name == "StopButtonGrid") {
				AssociatedObject.MouseDown += new System.Windows.Input.MouseButtonEventHandler(DoStopButtonAction);
			}
			else if(AssociatedObject.Name == "PauseButtonGrid") {
				AssociatedObject.MouseDown += new System.Windows.Input.MouseButtonEventHandler(DoPauseButtonAction);
			}
		}

		private void DoPauseButtonAction(object sender, System.Windows.Input.MouseEventArgs e) {
			Console.WriteLine("Nothing Happened!!!");
		}

		private void DoStartButtonAction(object sender, System.Windows.Input.MouseEventArgs e) {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Get the start block
			RobotInitial.Model.Block startBlock = mainWindowViewModel.ActiveWorkspaceViewModel.GetConnectedModel();

			// TEMPORARY, Print the pseudocode
			Console.WriteLine(startBlock.ToString());
			
		}

		private void DoStopButtonAction(object sender, System.Windows.Input.MouseEventArgs e) {
			Console.WriteLine("Nothing Happened!!!");
		}
	}
}
