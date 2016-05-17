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
using System.Net.Sockets;

namespace RobotInitial.Behaviours {
	class StartStopPause : Behavior<FrameworkElement> {

		private delegate void NoArgDelegate();
		private delegate void IntArgDelegate(int arg);
		private delegate void MainWindowDelegate(MainWindowViewModel mainWindowViewModel);
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
			else if(AssociatedObject.Name == "ConnectButton") {
				((Button)AssociatedObject).Click += new RoutedEventHandler(ConnectButton_Click);
			}
			else if (AssociatedObject.Name == "DisconnectButton") {
				((Button)AssociatedObject).Click += new RoutedEventHandler(DisconnectButton_Click);
			}

		}

		void ConnectButton_Click(object sender, RoutedEventArgs e) {
			connectToRobot();
		}

		void DisconnectButton_Click(object sender, RoutedEventArgs e) {
			disconnectCurrentRobot();
		}

		public void connectToRobot() {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// hide the server refresh
			mainWindowViewModel.RefreshButtonVisibility = Visibility.Hidden;
			

			// Switch out the connection and disconnect buttons
			mainWindowViewModel.setConnectionVisible(ConnectionVisible.DISCONNECT_ADDRESS);

			// Launch separate thread
			//NoArgDelegate connector = new NoArgDelegate(makeConnection);
			MainWindowDelegate connector = new MainWindowDelegate(makeConnection);
			connector.BeginInvoke(mainWindowViewModel,null, null);
		}

		// Try to make a connection on a separate worker thread
		private void makeConnection(MainWindowViewModel mainWindowViewModel) {
			if (mainWindowViewModel.CurrentAddressText == null) return;
			try {
				if (mainWindowViewModel.RobotNames.Contains(mainWindowViewModel.CurrentAddressText)) {
					int index = mainWindowViewModel.RobotNames.IndexOf(mainWindowViewModel.CurrentAddressText);

					// Connect to the Lynx from the list of IP Endpoints
					Network.Instance.connectToLynx(mainWindowViewModel.RobotEndpoints[index], Network.STANDARD_TIMEOUT);
				}
				else {
					// try to parse an IPAddress
					IPAddress ip;
					if (IPAddress.TryParse(mainWindowViewModel.CurrentAddressText, out ip)) {
						// Connect using the input IP
						Network.Instance.connectToLynx(new IPEndPoint(ip, Network.DefaultPort), Network.STANDARD_TIMEOUT);
					}

					else {
						// Try getting a Host entry
						IPHostEntry IPEntry = Dns.GetHostEntry(mainWindowViewModel.CurrentAddressText);

						// If successfull, have a go at connecting from the first address
						Network.Instance.connectToLynx(new IPEndPoint(IPEntry.AddressList[0], Network.DefaultPort), Network.STANDARD_TIMEOUT);
					}
				}

				//======== Connection must have been a success here !
				//-------- Apparently, unless the clicked a stale connection address
				//-------- So the solution is to use a timeout
				// Get the current robot status, Asynchronously
				int response = Network.Instance.requestProgramStatus();


				// Update the properties back on the main UI thread
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
					 new IntArgDelegate(connectionAccept),response);
			}
			catch (LynxBusyException exc) {
				// Connection must have been a success here !
				// Update the properties back on the main UI thread
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
					 new NoArgDelegate(connectionFailed));
				Console.WriteLine("Lynx is currently Busy... Connection Failed");
			}
			catch (SocketException exc) {
				// Connection must have been a success here !
				// Update the properties back on the main UI thread
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
					 new NoArgDelegate(connectionFailed));
				Console.WriteLine("Socket Exception During Connection: {0}", exc);
			}
		}

		// Make changes on connection accepted
		private void connectionAccept(int response) {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// Set the flag as connected
			mainWindowViewModel.Connected = true;

			// Set UI values or stop the animation
			Console.WriteLine("Connection SUCCEEEDED!!");

			// Check the response from the robot to set the animation state
			if(response == Request_Handler.PROGRAM_PAUSED_RESPONSE) {
				// Setup the animation pause stuff
				doPauseUIUpdates();

				// Update the UI for that which should display the play button again
				updateUIAfterProgram();
			}
			else if (response == Request_Handler.PROGRAM_EXECUTING_RESPONSE) {
				// Hide the UI Play button
				mainWindow.StartStopControl.StartButtonGrid.SetValue(UIElement.VisibilityProperty, Visibility.Hidden);

				// Get and start the animation
				Storyboard story = (Storyboard)mainWindow.StartStopControl.FindResource("RunningAnimation");
				RadialGradientBrush currentBrush = (RadialGradientBrush)mainWindow.StartStopControl.AnimatedEllipse.Fill;
				currentBrush.GradientStops[0].Color = (Color)ColorConverter.ConvertFromString("#FF00BE03");
				mainWindow.StartStopControl.AnimatedEllipse.Visibility = Visibility.Visible;

				// Launch a fake start program to pickup on the execution
				story.Begin(mainWindow.StartStopControl, true);
				StartProgramDelegate programLauncher = new StartProgramDelegate(startProgram);
				programLauncher.BeginInvoke(null, null, null);
			}
			// Just reset completely
			else {
				// Do stop UI update
				doStopUIUpdates();

				// Update the UI for that which should display the play button again
				updateUIAfterProgram();
			}

			// Show the start stop widget
			mainWindowViewModel.showStartStopWidget();

			// Launch server poller

		}

		// Make changes on connection failure
		private void connectionFailed() {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			mainWindowViewModel.Connected = false;
			Console.WriteLine("Connection FAILED!!");

			// Show the connect button and enable the address
			mainWindowViewModel.setConnectionVisible(ConnectionVisible.CONNECT_ADDRESS);

			mainWindowViewModel.RefreshButtonVisibility = Visibility.Visible;

			// Most likely cause of failure is an invalid address, refresh the list
			mainWindowViewModel.updateRobotAddressList();
		}

		// Disconnect the current robot
		private void disconnectCurrentRobot() {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			if (Network.Instance.isConnected()) {
				Network.Instance.closeConnection();
			}

			// Not connected now!
			mainWindowViewModel.Connected = false;

			// Show the server refresh
			mainWindowViewModel.RefreshButtonVisibility = Visibility.Visible;
			mainWindowViewModel.setConnectionVisible(ConnectionVisible.CONNECT_ADDRESS);

			// hide the start stop widget
			mainWindowViewModel.hideStartStopWidget();

			// Update the robot list
			mainWindowViewModel.updateRobotAddressList();


			// Reset the start stop widget

		}

		// Start the program up on a separate thread
		private void startProgram(StartBlock start) {
			// Check the status is not running
			int response = Network.Instance.requestProgramStatus();
			if (response == Request_Handler.PROGRAM_EXECUTING_RESPONSE ||
				response == Request_Handler.PROGRAM_PAUSED_RESPONSE) {
				Console.WriteLine("Program Already Running!!");
			}
			else {
				// Start the program
				try {
					if(start != null) {
						Network.Instance.startProgram(start);
					}
				}
				catch (LynxBusyException exc) {
					Console.WriteLine("Lynx is Busy!");
				}
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
			
		}

		private void doStopUIUpdates() {
			// Get the Main Window view
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;

			// Get its view model
			MainWindowViewModel mainWindowViewModel = (MainWindowViewModel)mainWindow.DataContext;

			// If the program is paused
			if(mainWindowViewModel.ProgramPaused) {
				// Set as not paused
				mainWindowViewModel.ProgramPaused = false;

				// Do UI updates
				updateUIAfterProgram();
			}
		}


		// Pause the program on a separate thread
		private void resumeProgram() {

			// Check the status IS running
			int response = Network.Instance.requestProgramStatus();
			if (response != Request_Handler.PROGRAM_EXECUTING_RESPONSE &&
				response != Request_Handler.PROGRAM_PAUSED_RESPONSE) {
				Console.WriteLine("Program is NOT Running!!");
			}
			else {
				// Resume the program
				try {
					Network.Instance.resumeProgram();
				}
				catch (LynxBusyException exc) {
					Console.WriteLine("Lynx is Busy!");
				}
			}

			// Get the status here
			response = Network.Instance.requestProgramStatus();

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

			Console.WriteLine("Stopping Program!!");
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
