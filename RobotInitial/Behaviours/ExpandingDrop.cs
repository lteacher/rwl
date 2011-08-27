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
				targetViewModel.AddChildBlock(new LoopControlBlockView());
			}
			if (sourceViewModel.Type.Equals("Wait"))
			{
				targetViewModel.AddChildBlock(new WaitControlBlockView());
			}
			if (sourceViewModel.Type.Equals("Switch"))
			{
				targetViewModel.AddChildBlock(new SwitchControlBlockView());
			}
			e.Handled = true;
		}

		//private void ExpandElement(object sender, System.Windows.DragEventArgs e)
		//{
		//    // TODO: Add event handler implementation here.
		//    FrameworkElement dropTarget = (FrameworkElement)sender;
		//    LoopControlBlockViewModel loopBlockModel = (LoopControlBlockViewModel)dropTarget.DataContext;

		//    LoopControlBlockView loopBlock = new LoopControlBlockView();
		//    //UIElement dropSource = (UIElement)e.Data.GetData("Object");
		//    //dropTarget.RenderSize.Width += dropSource.RenderSize.Width;
		//    //MessageBox.Show("Size is " +dropTarget.RenderSize.Width);
		//    //((Panel)dropTarget).
		//    //Panel blah = new Panel();
		//    //blah.Children.
		//    loopBlockModel.AddChildBlock(loopBlock);
		//}

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