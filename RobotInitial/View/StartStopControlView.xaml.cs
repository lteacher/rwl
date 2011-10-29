﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RobotInitial.ViewModel;

namespace RobotInitial.View {
	/// <summary>
	/// Interaction logic for StartStopControlView.xaml
	/// </summary>
	public partial class StartStopControlView : UserControl {
		public StartStopControlView() {
			InitializeComponent();
			this.StartButtonGrid.MouseDown +=new MouseButtonEventHandler(DoStartButtonAction);
			this.StopButtonGrid.MouseDown += new MouseButtonEventHandler(DoStopButtonAction);
			this.PauseButtonGrid.MouseDown += new MouseButtonEventHandler(DoPauseButtonAction);
		}

		private void PressedColourChange(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			Grid PressedButton = (Grid)sender;
			if(PressedButton.Name == "StartButtonGrid") {
				this.StartEllipse.Opacity = 0.6;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.StartEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.Green;
			}
			else if(PressedButton.Name == "StopButtonGrid") {
				this.StopEllipse.Opacity = 0.6;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.StopEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.Red;
			}
			else if(PressedButton.Name == "PauseButtonGrid") {
				this.PauseEllipse.Opacity = 0.6;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.PauseEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.Orange;
			}
		}

		private void ReleasedColourChange(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			Grid ReleasedButton = (Grid)sender;
			if (ReleasedButton.Name == "StartButtonGrid") {
				this.StartEllipse.Opacity = 1;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.StartEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.White;
			}
			else if (ReleasedButton.Name == "StopButtonGrid") {
				this.StopEllipse.Opacity = 1;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.StopEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.White;
			}
			else if (ReleasedButton.Name == "PauseButtonGrid") {
				this.PauseEllipse.Opacity = 1;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.PauseEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.White;
			}
		}

		private void ResetColours(object sender, System.Windows.Input.MouseEventArgs e)
		{
			Grid ReleasedButton = (Grid)sender;
			if (ReleasedButton.Name == "StartButtonGrid") {
				this.StartEllipse.Opacity = 1;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.StartEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.White;
			}
			else if (ReleasedButton.Name == "StopButtonGrid") {
				this.StopEllipse.Opacity = 1;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.StopEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.White;
			}
			else if (ReleasedButton.Name == "PauseButtonGrid") {
				this.PauseEllipse.Opacity = 1;
				RadialGradientBrush currentBrush = (RadialGradientBrush)this.PauseEllipse.Fill;
				currentBrush.GradientStops[1].Color = Colors.White;
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
