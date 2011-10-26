﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using RobotInitial.View;
using RobotInitial.ViewModel;
//using Microsoft.Expression.Interactivity.Core;

namespace RobotInitial.Behaviours
{
	public class OpenPropertiesView : Behavior<UIElement>
	{
		public OpenPropertiesView()
		{
			// Insert code required on object creation below this point.
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			// Insert code that you would want run when the Behavior is attached to an object.
			AssociatedObject.MouseDown +=new MouseButtonEventHandler(MousePressed);		
		}

		private void MousePressed(object sender,MouseEventArgs e) {
			FrameworkElement clickedBlock = (FrameworkElement)((FrameworkElement)sender).Parent;
			MainWindowView mainWindow = (MainWindowView)Application.Current.MainWindow;
			PropertiesTabViewModel propertiesViewModel = (PropertiesTabViewModel)mainWindow.PropertiesView.DataContext;
			Console.WriteLine(clickedBlock.GetType());

			if (clickedBlock.GetType() == typeof(MoveControlBlockView)) {			
				MoveControlBlockViewModel viewModel = (MoveControlBlockViewModel)clickedBlock.DataContext;
				propertiesViewModel.setPropertiesView(viewModel.PropertiesView);
			}
			else if (clickedBlock.GetType() == typeof(LoopControlBlockView)) {
				LoopControlBlockViewModel viewModel = (LoopControlBlockViewModel)clickedBlock.DataContext;
				propertiesViewModel.setPropertiesView(viewModel.PropertiesView);
			}
			else if (clickedBlock.GetType() == typeof(SwitchTabBlockView)) {
				SwitchTabBlockViewModel viewModel = (SwitchTabBlockViewModel)clickedBlock.DataContext;
				propertiesViewModel.setPropertiesView(viewModel.PropertiesView);
			}
			else if (clickedBlock.GetType() == typeof(WaitControlBlockView)) {
				WaitControlBlockViewModel viewModel = (WaitControlBlockViewModel)clickedBlock.DataContext;
				propertiesViewModel.setPropertiesView(viewModel.PropertiesView);
			}
			else if (clickedBlock.GetType() == typeof(ScrollViewer) ||
				clickedBlock.GetType() == typeof(SequenceView)) {

                //WorkspaceViewModel blah = (WorkspaceViewModel)((FrameworkElement)clickedBlock.Parent).DataContext;
                //RobotInitial.Model.Block block = blah.GetConnectedModel();
                //DeleteThisHackTestMethod(block);

				propertiesViewModel.setBlankProperties();
			}

			e.Handled = true;
		}

        //private static void DeleteThisHackTestMethod(RobotInitial.Model.Block block) {
        //    if (block == null) {
        //        return;
        //    }
        //    Console.WriteLine("Current: " + block);
        //    if (block is RobotInitial.Model.LoopBlock) {
        //        Console.WriteLine("Entering Loop Path");
        //        DeleteThisHackTestMethod((block as RobotInitial.Model.LoopBlock).LoopPath);
        //        Console.WriteLine("Exiting Loop Path");
        //    } else if (block is RobotInitial.Model.SwitchBlock<bool>) {
        //        Console.WriteLine("Entering True Path");
        //        DeleteThisHackTestMethod((block as RobotInitial.Model.SwitchBlock<bool>).GetPath(true));
        //        Console.WriteLine("Entering False Path");
        //        DeleteThisHackTestMethod((block as RobotInitial.Model.SwitchBlock<bool>).GetPath(false));
        //        Console.WriteLine("Exiting Switch");
        //    }
        //    DeleteThisHackTestMethod(block.Next);
        //}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			// Insert code that you would want run when the Behavior is removed from an object.
		}

		/*
		public ICommand MyCommand
		{
			get;
			private set;
		}
		 
		private void MyFunction()
		{
			// Insert code that defines what the behavior will do when invoked.
		}
		*/
	}
}