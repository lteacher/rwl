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
using RobotInitial.ViewModel;

namespace RobotInitial.Behaviours
{
	class DragWithAdornment : Behavior<UIElement>
	{
		public FrameworkElement DragScope { get; set; }
		private BlockDragAdorner blockAdorner = null;
		private AdornerLayer adornLayer;
		private bool _dragHasLeftScope = false;
		private ResourceDictionary dict;
		private Point _startPoint;
		protected override void OnAttached()
		{

			AssociatedObject.MouseDown += (sender, e) => {
				_startPoint = e.GetPosition((FrameworkElement)sender);
			};

			AssociatedObject.MouseMove += (sender, e) => {
				
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					Point newPoint = e.GetPosition((FrameworkElement)sender);

					if(Math.Abs(newPoint.X - _startPoint.X) >= SystemParameters.MinimumHorizontalDragDistance ||
					   Math.Abs(newPoint.Y - _startPoint.Y) >= SystemParameters.MinimumVerticalDragDistance) {

						// Set the source element
						FrameworkElement source = (FrameworkElement)sender;

						// If the source is a taskblock do the task block drag drop
						if(source is TaskBlockItem) {
							TaskBlockDragDrop((TaskBlockItem)source);
						
						} else {
							// Do a normal block drop, the source is a grid, so its parent is the actual view
							BlockDragDrop((FrameworkElement)source.Parent);
						}
						e.Handled = true;	
					}
				}
			};

		}

		private void BlockDragDrop(FrameworkElement source) {
			// Need to get the parent if its a loop, switch or sequence
			FrameworkElement parentView = getParentView(source);

			// Make sure there is no left over undesirable margin
			source.Margin = new Thickness(0,0,0,0);

			if(parentView is LoopControlBlockView) {
				((LoopControlBlockViewModel)parentView.DataContext).RemoveBlock(source);
			}
			else if(parentView is SwitchTabBlockView) {
				((SwitchTabBlockViewModel)parentView.DataContext).RemoveBlock(source);
			}
			else if(parentView is SequenceView) {
				((SequenceViewModel)parentView.DataContext).RemoveBlock(source);
			}

			DoDragDrop(source, source);
		}

		private void TaskBlockDragDrop(TaskBlockItem source) {
			FrameworkElement ctrl = null;

			if (source.Action.Equals("Move")) {
				ctrl = new MoveControlBlockView();
			}
			if (source.Action.Equals("Loop")) {
				ctrl = new LoopControlBlockView();
			}
			if (source.Action.Equals("Switch")) {
				//ctrl = new SwitchControlBlockView();
				ctrl = new SwitchTabBlockView();
			}
			if (source.Action.Equals("Wait")) {
				ctrl = new WaitControlBlockView();
			}

			// Do the drag drop actions
			DoDragDrop(source,ctrl);
		}

		private void DoDragDrop(FrameworkElement source, FrameworkElement data) {
			// Create the data object which will hold a brick in future
			DataObject dataObject = new DataObject();

			dataObject.SetData("Object", data);


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
			blockAdorner = new BlockDragAdorner(DragScope, data, 0.5);
			adornLayer = AdornerLayer.GetAdornerLayer(DragScope as Visual);
			adornLayer.Add(blockAdorner);

			// Do the drag drop
			DragDrop.DoDragDrop(source, dataObject, DragDropEffects.Move | DragDropEffects.Copy);

			// Remove the Adorner from the layer
			DragScope.AllowDrop = previousDrop;
			AdornerLayer.GetAdornerLayer(DragScope).Remove(blockAdorner);
			blockAdorner = null;

			// Remove the handlers for the drag scope
			DragScope.DragLeave -= dragleavehandler;
			DragScope.QueryContinueDrag -= queryhandler;
			DragScope.PreviewDragOver -= draghandler;
		}

		private FrameworkElement getParentView(FrameworkElement source) {
		FrameworkElement treeParent = source;
			while (treeParent != null) {
				if (treeParent == null) break;
				treeParent = (FrameworkElement)VisualTreeHelper.GetParent(treeParent);

				// Return any Switch or Loop as they are valid parents
				if (treeParent is SwitchTabBlockView || treeParent is LoopControlBlockView) {
					return treeParent;
				}

				// If the treeparent is WorkspaceView then return the SequenceView
				if (treeParent is WorkspaceView) {
					WorkspaceViewModel wsVM = (WorkspaceViewModel)((WorkspaceView)treeParent).DataContext;
					return wsVM.Sequence;
				}
			}
			return treeParent;
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
