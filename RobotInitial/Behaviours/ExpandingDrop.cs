using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interactivity;
using RobotInitial.ViewModel;
using RobotInitial.View;
using RobotInitial.Controls;
//using Microsoft.Expression.Interactivity.Core;

namespace RobotInitial.Behaviours
{
	public class BlockExpand : Behavior<UIElement>
	{
		public BlockExpand()
		{
			// Insert code required on object creation below this point.

			//
			// The line of code below sets up the relationship between the command and the function
			// to call. Uncomment the below line and add a reference to Microsoft.Expression.Interactions
			// if you choose to use the commented out version of MyFunction and MyCommand instead of
			// creating your own implementation.
			//
			// The documentation will provide you with an example of a simple command implementation
			// you can use instead of using ActionCommand and referencing the Interactions assembly.
			//
			//this.MyCommand = new ActionCommand(this.MyFunction);
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			// Insert code that you would want run when the Behavior is attached to an object.
			AssociatedObject.Drop += new DragEventHandler(Item_Dropped);
			//AssociatedObject.DragEnter += new System.Windows.DragEventHandler(ExpandElement);
			AssociatedObject.MouseDown += new System.Windows.Input.MouseButtonEventHandler(AssociatedObject_MouseDown);
			//AssociatedObject.MouseEnter += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseEnter);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			// Insert code that you would want run when the Behavior is removed from an object.
		}

		private void Item_Dropped(object sender, System.Windows.DragEventArgs e)
		{
			FrameworkElement dropTarget = (FrameworkElement)sender;
			ControlBlockViewModel targetViewModel = (ControlBlockViewModel)dropTarget.DataContext;

			FrameworkElement dropSource = (FrameworkElement)e.Data.GetData("Object");
			ControlBlockViewModel sourceViewModel = (ControlBlockViewModel)dropSource.DataContext;
			if(sourceViewModel.Type.Equals("Move"))
			{	
			    targetViewModel.AddChildBlock(new MoveControlBlockView());
			}
			if (sourceViewModel.Type.Equals("Loop"))
			{
				LoopControlBlockView child = new LoopControlBlockView();
				targetViewModel.AddChildBlock(child);

				// Setup the measured size
				child.Measure(new Size(child.MaxWidth,child.MaxHeight));

				// if the child to be added is bigger than the parent then
				// it the 'Root' parent must refit to the workspace, this
				// means we need the root control so now we have to find it!
				FrameworkElement parent = (FrameworkElement)dropTarget.Parent;
				FrameworkElement treeParent = (FrameworkElement)VisualTreeHelper.GetParent(dropTarget);

				// Find the ultimate parent
				//Console.WriteLine("******** START ********");
				double tempChildHeight = child.DesiredSize.Height;
				double maxChildHeight = tempChildHeight;
				while(treeParent != null) {
					treeParent = (FrameworkElement)VisualTreeHelper.GetParent(treeParent);
					if(treeParent == null) break;
					if(treeParent.GetType() == typeof(LoopControlBlockView)) {
						parent = treeParent;
						maxChildHeight = tempChildHeight;
						tempChildHeight = parent.DesiredSize.Height;
					}
					if(treeParent.GetType() == typeof(WorkspaceView)) break;
				}

				

				// If the parent height is less than or the same as the child
				Console.WriteLine("Parent Height: {0}, Child Height: {1}, MaxCHild: {2}", parent.RenderSize.Height, child.DesiredSize.Height,maxChildHeight);

				if(((FrameworkElement)dropTarget.Parent).RenderSize.Height <= child.DesiredSize.Height) {
					Console.WriteLine("===>>>>  MOVING UP");

					// Get the top point of the root parent
					double top = Canvas.GetTop(parent);

					// From here if the child is still less than the root parent
					// then the default increase can be subtracted!
					if(child.DesiredSize.Height <= parent.RenderSize.Height) {
						// Also make sure that some other parent doesnt have room
						Console.WriteLine("The subtraction here is: {0}", parent.RenderSize.Height - maxChildHeight);
						if(!(parent.RenderSize.Height - maxChildHeight > 50)) {
							top = top-25;
						}
					}
					// Otherwise we need a bigger offset!
					else {
						top = top - (((child.DesiredSize.Height+50) - parent.RenderSize.Height)/2);
					}

					parent.SetValue(Canvas.TopProperty, top);
				}
			}
			if (sourceViewModel.Type.Equals("Wait"))
			{
				targetViewModel.AddChildBlock(new WaitControlBlockView());
			}
			if (sourceViewModel.Type.Equals("Switch"))
			{
				SwitchControlBlockView child = new SwitchControlBlockView();
				
				targetViewModel.AddChildBlock(child);

				// if the child to be added is bigger than the parent then
				// it the 'Root' parent must refit to the workspace, this
				// means we need the root control so now we have to find it!
				FrameworkElement parent = (FrameworkElement)dropTarget.Parent;
				FrameworkElement treeParent = (FrameworkElement)VisualTreeHelper.GetParent(dropTarget);

				// Find the ultimate parent
				//Console.WriteLine("******** START ********");
				while (treeParent != null)
				{
					treeParent = (FrameworkElement)VisualTreeHelper.GetParent(treeParent);
					if (treeParent == null) break;
					if (treeParent.GetType() == typeof(LoopControlBlockView))
					{
						parent = treeParent;
					}
					if (treeParent.GetType() == typeof(WorkspaceView)) break;
				}

				// Setup the measured size
				child.Measure(new Size(child.MaxWidth, child.MaxHeight));

				// If the parent height is less than or the same as the child
				Console.WriteLine("Parent Height: {0}, Child Height: {1}", parent.RenderSize.Height, child.DesiredSize.Height);

				if (((FrameworkElement)dropTarget.Parent).RenderSize.Height <= child.DesiredSize.Height)
				{
					Console.WriteLine("===>>>>  MOVING UP");

					// Get the top point of the root parent
					double top = Canvas.GetTop(parent);

					// From here if the child is still less than the root parent
					// then the default increase can be subtracted!
					if (child.DesiredSize.Height <= parent.RenderSize.Height)
					{
						top = top - 25;
					}
					// Otherwise we need a bigger offset!
					else
					{
						top = top - (((child.DesiredSize.Height + 50) - parent.RenderSize.Height) / 2);
					}

					parent.SetValue(Canvas.TopProperty, top);
				}
			}
			e.Handled = true;
		}

		private void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			// TODO: Add event handler implementation here.
			//FrameworkElement dropTarget = (FrameworkElement)sender;
			//LoopControlBlockViewModel loopBlockModel = (LoopControlBlockViewModel)dropTarget.DataContext;
			//loopBlockModel.ExpandControl();
			//e.Handled = true;
		}

		private void AssociatedObject_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			// TODO: Add event handler implementation here.
			//MessageBox.Show("Mouse Entered!!");

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