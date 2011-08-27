using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using RobotInitial.Controls;
using RobotInitial.View;
using System.Windows.Controls;

namespace RobotInitial.Behaviours
{
	class DragWithAdornment : Behavior<UIElement>
	{
		public FrameworkElement DragScope { get; set; }
		private BlockDragAdorner blockAdorner = null;
		private AdornerLayer adornLayer;
		private bool _dragHasLeftScope = false;
		private ResourceDictionary dict;
		protected override void OnAttached()
		{

			AssociatedObject.MouseMove += (sender, e) => {
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					// Set the brick button
					UIElement brickButton = (UIElement)sender;

					// Create the data object which will hold a brick in future
					DataObject data = new DataObject();

					// Setup the resource dictionary, need this to set the style
					// of a each initialized custom control
					dict = new ResourceDictionary();
					dict.Source = new Uri("/MainWindowResources.xaml",
											UriKind.Relative);

					// TEMPORARY BRICK, LOVELY COLOURED BLOCKS
					if (sender is TaskBlockItem)
					{
						
						Rectangle rect = new Rectangle();
						//ControlBlock ctrl = null;
						FrameworkElement ctrl = null;
						//ControlBlockViewModel ctrlVM

						if (((TaskBlockItem)sender).Action.Equals("Move"))
						{
							ctrl = new MoveControlBlockView();
						}
						if (((TaskBlockItem)sender).Action.Equals("Loop")) { 
						    ctrl = new LoopControlBlockView();
						}
						if (((TaskBlockItem)sender).Action.Equals("Switch"))
						{
							ctrl = new SwitchControlBlockView();
						}
						if (((TaskBlockItem)sender).Action.Equals("Wait"))
						{
							ctrl = new WaitControlBlockView();
						}
						
						data.SetData("Object", ctrl);
						

						// Define the drag scope for the adorner
						DragScope = Application.Current.MainWindow.Content as FrameworkElement;
						System.Diagnostics.Debug.Assert(DragScope != null);

						// Enable Drag & Drop in the scope so that we can use DragOver
						bool previousDrop = DragScope.AllowDrop;
						DragScope.AllowDrop = true;

						// Set the drag event for the drag scope 
						DragEventHandler draghandler = new DragEventHandler(DragScope_DragOver);
						DragScope.PreviewDragOver += draghandler;

						// Set the Drag Leave event handler for the drag scope 
						DragEventHandler dragleavehandler = new DragEventHandler(DragScope_DragLeave);
						DragScope.DragLeave += dragleavehandler;

						// QueryContinue Drag goes with drag leave... 
						QueryContinueDragEventHandler queryhandler = new QueryContinueDragEventHandler(DragScope_QueryContinueDrag);
						DragScope.QueryContinueDrag += queryhandler;

						// Create the BrickDragAdorner giving the parent and the child
						blockAdorner = new BlockDragAdorner(DragScope, ctrl, 0.5);
						adornLayer = AdornerLayer.GetAdornerLayer(DragScope as Visual);
						adornLayer.Add(blockAdorner);

						// Do the drag drop
						DragDrop.DoDragDrop(brickButton, data, DragDropEffects.Move | DragDropEffects.Copy);

						// Remove the Adorner from the layer
						DragScope.AllowDrop = previousDrop;
						AdornerLayer.GetAdornerLayer(DragScope).Remove(blockAdorner);
						blockAdorner = null;

						// Remove the handlers for the drag scope
						DragScope.DragLeave -= dragleavehandler;
						DragScope.QueryContinueDrag -= queryhandler;
						DragScope.PreviewDragOver -= draghandler;
					}
				}
			};

		}

		private void DragScope_DragOver(object sender, DragEventArgs args)
		{
			if (blockAdorner != null)
			{
				blockAdorner.LeftOffset = args.GetPosition(DragScope).X /* - _startPoint.X */ ;
				blockAdorner.TopOffset = args.GetPosition(DragScope).Y /* - _startPoint.Y */ ;
			}
		}

		void DragScope_DragLeave(object sender, DragEventArgs e)
		{
			if (e.OriginalSource == DragScope)
			{
				Point p = e.GetPosition(DragScope);
				Rect r = VisualTreeHelper.GetContentBounds(DragScope);
				if (!r.Contains(p))
				{
					this._dragHasLeftScope = true;
					e.Handled = true;
				}
			}
		}

		void DragScope_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (this._dragHasLeftScope)
			{
				e.Action = DragAction.Cancel;
				e.Handled = true;
			}
		}

	}
}
