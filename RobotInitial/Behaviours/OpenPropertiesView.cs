using System;
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
using System.Windows.Media.Animation;
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
			WorkspaceView workspaceView = getWorspaceView(clickedBlock);
			WorkspaceViewModel workspaceViewModel = (WorkspaceViewModel)workspaceView.DataContext;

			// If the same object is clicked then just return handled dont need to do it twice
			if (ReferenceEquals(workspaceViewModel.SelectedBlock, clickedBlock)) {
				e.Handled = true;
				return;
			}

			if (clickedBlock.GetType() == typeof(MoveControlBlockView)) {			
				MoveControlBlockViewModel viewModel = (MoveControlBlockViewModel)clickedBlock.DataContext;
				if(workspaceViewModel.SelectedBlock != null)((ControlBlockViewModel)workspaceViewModel.SelectedBlock.DataContext).
																			StopSelectedAnimation(workspaceViewModel.SelectedBlock);
				workspaceViewModel.SelectedBlock = clickedBlock;
				viewModel.StartSelectedAnimation(clickedBlock);
				propertiesViewModel.setPropertiesView(viewModel.PropertiesView);
			}
			else if (clickedBlock.GetType() == typeof(LoopControlBlockView)) {
				LoopControlBlockViewModel viewModel = (LoopControlBlockViewModel)clickedBlock.DataContext;
				if (workspaceViewModel.SelectedBlock != null) ((ControlBlockViewModel)workspaceViewModel.SelectedBlock.DataContext).
																			  StopSelectedAnimation(workspaceViewModel.SelectedBlock);
				workspaceViewModel.SelectedBlock = clickedBlock;
				viewModel.StartSelectedAnimation(clickedBlock);
				propertiesViewModel.setPropertiesView(viewModel.PropertiesView);
			}
			else if (clickedBlock.GetType() == typeof(SwitchTabBlockView)) {
				SwitchTabBlockViewModel viewModel = (SwitchTabBlockViewModel)clickedBlock.DataContext;
				if (workspaceViewModel.SelectedBlock != null) ((ControlBlockViewModel)workspaceViewModel.SelectedBlock.DataContext).
																			  StopSelectedAnimation(workspaceViewModel.SelectedBlock);
				workspaceViewModel.SelectedBlock = clickedBlock;
				viewModel.StartSelectedAnimation(clickedBlock);
				propertiesViewModel.setPropertiesView(viewModel.PropertiesView);
			}
			else if (clickedBlock.GetType() == typeof(WaitControlBlockView)) {
				WaitControlBlockViewModel viewModel = (WaitControlBlockViewModel)clickedBlock.DataContext;
				if (workspaceViewModel.SelectedBlock != null) ((ControlBlockViewModel)workspaceViewModel.SelectedBlock.DataContext).
																			  StopSelectedAnimation(workspaceViewModel.SelectedBlock);
				workspaceViewModel.SelectedBlock = clickedBlock;
				viewModel.StartSelectedAnimation(clickedBlock);
				propertiesViewModel.setPropertiesView(viewModel.PropertiesView);
			}
			else if (clickedBlock.GetType() == typeof(ScrollViewer) ||
				clickedBlock.GetType() == typeof(SequenceView)) {
				if (workspaceViewModel.SelectedBlock != null) ((ControlBlockViewModel)workspaceViewModel.SelectedBlock.DataContext).
																				  StopSelectedAnimation(workspaceViewModel.SelectedBlock);
				RobotInitial.Model.Block block = workspaceViewModel.GetConnectedModel();
                Console.WriteLine(block.ToString());
                //DeleteThisHackTestMethod(block);

				propertiesViewModel.setBlankProperties();
			}

			e.Handled = true;
		}

		// Do some fun times to get the workspace as usual!
		private WorkspaceView getWorspaceView(FrameworkElement source) {
			FrameworkElement treeParent = source;
			while (treeParent != null) {
				if (treeParent == null) break;
				treeParent = (FrameworkElement)VisualTreeHelper.GetParent(treeParent);

				// If the treeparent is WorkspaceView then return it
				if (treeParent is WorkspaceView) {
					return treeParent as WorkspaceView;
				}
			}
			return treeParent as WorkspaceView;
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