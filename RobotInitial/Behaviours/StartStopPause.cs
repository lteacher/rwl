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
using System.Windows.Media;
using System.Windows.Media.Animation;

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
				Console.WriteLine("Lynx is Busy!");
				return;
			}

			// Get the status here
			response = Network.Instance.requestProgramStatus();

			Console.WriteLine("Waiting for completion!");

			// Start the program running animation and await status
			while (response == Request_Handler.PROGRAM_EXECUTING_RESPONSE) {
				if(response == Request_Handler.PROGRAM_PAUSED_RESPONSE) {
					break;
				}
				response = Network.Instance.requestProgramStatus();
			}

			// Some debug text
			Console.WriteLine(response == Request_Handler.PROGRAM_PAUSED_RESPONSE ? "Program Paused!" : "Program Completed!");

			// Get back to the UI thread
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				 new NoArgDelegate(updateUIAfterProgram));
		}

		private void updateUIAfterProgram() {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Get the animation
			Storyboard story = (Storyboard)mainWindow.StartStopControl.FindResource("RunningAnimation");

			// Set the colour
			if (mainWindowViewModel.ProgramPaused) {
				RadialGradientBrush currentBrush = (RadialGradientBrush)mainWindow.StartStopControl.StartTriangle.Fill;
				currentBrush.GradientStops[1].Color = Colors.Orange;

				// Get the brush of the animated ellipse
				currentBrush = (RadialGradientBrush)mainWindow.StartStopControl.AnimatedEllipse.Fill;
				currentBrush.GradientStops[0].Color = Colors.Orange;
				story.Pause(mainWindow.StartStopControl);

			} else {
				RadialGradientBrush currentBrush = (RadialGradientBrush)mainWindow.StartStopControl.StartTriangle.Fill;
				currentBrush.GradientStops[1].Color = (Color)ColorConverter.ConvertFromString("#FF00FF04");

				// Get the brush of the animated ellipse
				currentBrush = (RadialGradientBrush)mainWindow.StartStopControl.AnimatedEllipse.Fill;
				currentBrush.GradientStops[0].Color = (Color)ColorConverter.ConvertFromString("#FF00BE03");
				story.Pause(mainWindow.StartStopControl);
				mainWindow.StartStopControl.AnimatedEllipse.Visibility = Visibility.Hidden;
			}

			// Unhide the start button
			mainWindow.StartStopControl.StartButtonGrid.SetValue(UIElement.VisibilityProperty, Visibility.Visible);

			// Stop the playing animation
			
		}

		// Stop the program on a separate thread
		private void stopProgram() {
			// Make sure the program is still running
			int response = Network.Instance.requestProgramStatus();
			if (response != Request_Handler.PROGRAM_EXECUTING_RESPONSE &&
				response != Request_Handler.PROGRAM_PAUSED_RESPONSE) {
				Console.WriteLine("Program is NOT Running!");
				return;
			}

			// Stop the program
			Network.Instance.stopProgram();

			// Get back to the UI thread and make updates
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				 new NoArgDelegate(doStopUIUpdates));
		}

		// Pause the program on a separate thread
		private void pauseProgram() {
			// Make sure the program is still running
			int response = Network.Instance.requestProgramStatus();
			if (response != Request_Handler.PROGRAM_EXECUTING_RESPONSE) {
				Console.WriteLine("Program is NOT Running!");
				return;
			}

			// Pause the program
			Network.Instance.pauseProgram();

			// Get back to the UI thread and make updates
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				 new NoArgDelegate(doPauseUIUpdates));
		}

		private void doPauseUIUpdates() {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Set the paused flag
			mainWindowViewModel.ProgramPaused = true;
			
			// Pause the running animation
		}

		private void doStopUIUpdates() {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Its not paused if it ever was
			mainWindowViewModel.ProgramPaused = false;
		}


		// Pause the program on a separate thread
		private void resumeProgram() {
			
			// Resume the program!
			Network.Instance.resumeProgram();

			// Get the status here
			int response = Network.Instance.requestProgramStatus();

			Console.WriteLine("Waiting for completion!");

			// Start the program running animation and await status
			while (response == Request_Handler.PROGRAM_EXECUTING_RESPONSE) {
				if (response == Request_Handler.PROGRAM_PAUSED_RESPONSE) {
					break;
				}
				response = Network.Instance.requestProgramStatus();
			}

			// Get back to the UI thread
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
			mainWindow.StartStopControl.StartButtonGrid.SetValue(UIElement.VisibilityProperty,Visibility.Hidden);

			// Get and start the animation
			Storyboard story = (Storyboard)mainWindow.StartStopControl.FindResource("RunningAnimation");
			mainWindow.StartStopControl.AnimatedEllipse.Visibility = Visibility.Visible;

			// Create and launch the thread to start the program
			if(mainWindowViewModel.ProgramPaused) {
				// Switch off the pause flag before moving to the new thread
				mainWindowViewModel.ProgramPaused = false;

				// Fix the color of the animation back up
				RadialGradientBrush currentBrush = (RadialGradientBrush)mainWindow.StartStopControl.AnimatedEllipse.Fill;
				currentBrush.GradientStops[0].Color = (Color)ColorConverter.ConvertFromString("#FF00BE03");

				story.Resume(mainWindow.StartStopControl);

				NoArgDelegate programResumer = new NoArgDelegate(resumeProgram);
				programResumer.BeginInvoke(null,null);
			}
			else {
				story.Begin(mainWindow.StartStopControl, true);
				StartProgramDelegate programLauncher = new StartProgramDelegate(startProgram);
				programLauncher.BeginInvoke(startBlock,null,null);
			}
		}

		private void DoStopButtonAction(object sender, System.Windows.Input.MouseEventArgs e) {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Make sure robot is still connected
			if(mainWindowViewModel.Connected != true) {
				// recover UI, this should not be happening here
				updateUIAfterProgram();
				return;
			}

			// Create and launch the thread to start the program
			NoArgDelegate programLauncher = new NoArgDelegate(stopProgram);
			programLauncher.BeginInvoke(null, null);

			Console.WriteLine("Stopping Program");
		}

		private void DoPauseButtonAction(object sender, System.Windows.Input.MouseEventArgs e) {

			// Make sure robot is still connected
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Make sure robot is still connected
			if (mainWindowViewModel.Connected != true) {
				// recover UI, this should not be happening here
				updateUIAfterProgram();
				return;
			}

			// Create and launch the thread to start the program
			NoArgDelegate programLauncher = new NoArgDelegate(pauseProgram);
			programLauncher.BeginInvoke(null, null);
		}
	}
}
