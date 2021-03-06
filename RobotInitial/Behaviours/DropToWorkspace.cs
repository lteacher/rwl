﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using RobotInitial.ViewModel;
using RobotInitial.View;

namespace RobotInitial.Behaviours
{
	class DropToWorkspace : Behavior<UIElement>
	{
		protected override void OnAttached()
		{

			AssociatedObject.Drop += (sender,e) => {

				//// Block the event from being handled if handled already in the panel
				//if (e.Handled) return;

				//Panel panel = (Panel)sender;

				//// Get the FrameworkElement that is to be dropped
				//FrameworkElement element = (FrameworkElement)e.Data.GetData("Object");

				//if (panel != null && element != null)
				//{
				//    double x = ((int)e.GetPosition(panel).X / 25) * 25;
				//    double y = ((int)e.GetPosition(panel).Y / 25) * 25;
				//    panel.Children.Add(element);

				//    // Note we are dropping from the centre of the brick
				//    element.SetValue(Canvas.LeftProperty, x);// - (element.Width / 2));
				//    element.SetValue(Canvas.TopProperty, y);// - (element.Height / 2));
				//    element.SetValue(Canvas.ZIndexProperty, 100);

				//    //Console.WriteLine("=>>>>>  At this point the Left is: {0}",Canvas.GetLeft(element));

				//    // set the value to return to the DoDragDrop call
				//    e.Effects = DragDropEffects.Copy;
				//}
				MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;
				PropertiesTabViewModel propertiesViewModel = (PropertiesTabViewModel)mainWindow.PropertiesView.DataContext;
				WorkspaceView workspaceView = (WorkspaceView)((FrameworkElement)((FrameworkElement)sender).Parent).Parent;
				WorkspaceViewModel workspaceViewModel = (WorkspaceViewModel)workspaceView.DataContext;

				// Switch off the animation for the selected block that was just dropped out of the sequence
				if (workspaceViewModel.SelectedBlock != null) ((ControlBlockViewModel)workspaceViewModel.SelectedBlock.DataContext).
																				  StopSelectedAnimation(workspaceViewModel.SelectedBlock);
				// Reset the properties panel as blank
				propertiesViewModel.setBlankProperties();

			};
		}
	}
}
