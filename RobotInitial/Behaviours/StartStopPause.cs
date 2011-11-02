using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows.Controls;
using RobotInitial.View;
using RobotInitial.ViewModel;
using System.Windows;
using LynxTest2.Communications;
using System.Net;

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

			// Make sure robot is still connected

			// Make sure the program is still running

			// Pause the program

			// Pause the running animation

			// Display the play button as resume

			Console.WriteLine("Nothing Happened!!!");
		}

		private void DoStartButtonAction(object sender, System.Windows.Input.MouseEventArgs e) {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Get the start block
			RobotInitial.Model.StartBlock startBlock = mainWindowViewModel.ActiveWorkspaceViewModel.GetConnectedModel();

			

			// Make sure the connection is still active
			if(mainWindowViewModel.Connected == false) {
				Console.WriteLine("NOT CONNECTED");
				return;
			}

			// TEMPORARY, Print the pseudocode
			Console.WriteLine(startBlock.ToString());

			// Start the program
			try {
				Network.Instance.startProgram(startBlock);
			} catch(LynxBusyException exc) {
				Console.WriteLine("LYNX IS BUSY!");
				return;
			}

			// Start the program running animation and await status, 

			// The status waiting is completed, stop the running animation and reset the widget

			////============== TEMPORARY HACK FOR TESTING ======================
			//// Use a real address here! get it from a drop down or something ?
			//string robotAddIpAddress = "127.0.0.1";

			//// Connect to the Lynx Robot OR run the start command someway
			//// I would like to just set and start the program here and have a separate area to connect in the toolbar maybe
			//Network.connectToLynx(new IPEndPoint(IPAddress.Parse(robotAddIpAddress), 7331), startBlock);
			////============== TEMPORARY HACK FOR TESTING ======================
		}

		private void DoStopButtonAction(object sender, System.Windows.Input.MouseEventArgs e) {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Make sure robot is still connected
			if(mainWindowViewModel.Connected != true) {
				// recover UI, this should not be happening here
				return;
			}

			// Make sure the program is still running

			// Stop the program
			Network.Instance.stopProgram();

			// Stop the running animation

			// Reset the start stop widget

			Console.WriteLine("Nothing Happened!!!");
		}
	}
}
