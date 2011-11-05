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
using RobotInitial.Lynx_Server;
using RobotInitial.Model;
using System.Windows.Threading;

namespace RobotInitial.Behaviours {
	class StartStopPause : Behavior<Grid> {

		private delegate void NoArgDelegate();
		private delegate void StartProgramDelegate(StartBlock arg);

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

		// Start the program up on a separate thread
		private void startProgram(StartBlock start) {
			// Check the status is not running
			int response = Network.Instance.requestProgramStatus();
			if (response == Request_Handler.PROGRAM_EXECUTING_RESPONSE) {
				Console.WriteLine("Program Already Running!!");
				return;
			}

			// Start the program
			try {
				Network.Instance.startProgram(start);
			}
			catch (LynxBusyException exc) {
				Console.WriteLine("LYNX IS BUSY!");
				return;
			}

			// Get the status here
			response = Network.Instance.requestProgramStatus();

			Console.WriteLine("Waiting for completion!");

			// Start the program running animation and await status
			while (response == Request_Handler.PROGRAM_EXECUTING_RESPONSE) {
				response = Network.Instance.requestProgramStatus();
			}

			Console.WriteLine("Program Completed!");

			// Get back to the UI thread
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				 new NoArgDelegate(updateUIAfterProgram));
		}

		private void updateUIAfterProgram() {
			// Unhide the start button

			// Stop the playing animation

		}

		// Stop the program on a separate thread
		private void stopProgram() {
			// Stop the program
			Network.Instance.stopProgram();

			// Get back to the UI thread and make updates
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				 new NoArgDelegate(updateUIAfterProgram));
		}

		private void DoStartButtonAction(object sender, System.Windows.Input.MouseEventArgs e) {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Get the start block
			StartBlock startBlock = mainWindowViewModel.ActiveWorkspaceViewModel.GetConnectedModel();

			// TEMPORARY, Print the pseudocode
			Console.WriteLine(startBlock.ToString());

			// Make sure the connection is still active
			if(mainWindowViewModel.Connected == false) {
				Console.WriteLine("NOT CONNECTED");
				return;
			}

			// Hide the UI Play button

			// Create and launch the thread to start the program
			StartProgramDelegate programLauncher = new StartProgramDelegate(startProgram);
			programLauncher.BeginInvoke(startBlock,null,null);
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

			// Create and launch the thread to start the program
			NoArgDelegate programLauncher = new NoArgDelegate(stopProgram);
			programLauncher.BeginInvoke(null, null);

			Console.WriteLine("Stopping Program");
		}
	}
}
