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
using System.Collections.ObjectModel;
using System.Windows.Threading;
//using Microsoft.Expression.Interactivity.Core;

namespace RobotInitial.Behaviours
{
	public class BlockExpand : Behavior<UIElement>
	{
		private WorkspaceView workspace;
		private FrameworkElement parent;
		private Collection<SwitchTabBlockView> switchBlockViews;
		private double maxChildHeight;
		private int depth = 0;

		public BlockExpand()
		{
			
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			// Insert code that you would want run when the Behavior is attached to an object.
			AssociatedObject.Drop += new DragEventHandler(Item_Dropped);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			// Insert code that you would want run when the Behavior is removed from an object.
		}

		// Setup the workspace by finding it as the parent
		private void setWorkspaceAndParent(FrameworkElement dropTarget, FrameworkElement child)
		{
			// if the child to be added is bigger than the parent then
			// the 'Root' parent must refit to the workspace, this
			// means we need the root control so now we have to find it!
			FrameworkElement treeParent = (FrameworkElement)VisualTreeHelper.GetParent(dropTarget);
			switchBlockViews = new Collection<SwitchTabBlockView>();

			// Find the ultimate parent
			double tempChildHeight = child.DesiredSize.Height;
			maxChildHeight = tempChildHeight;
			depth = 0;
			while (treeParent != null)
			{
				treeParent = (FrameworkElement)VisualTreeHelper.GetParent(treeParent);
				if (treeParent == null) break;
				if (treeParent.GetType() == typeof(LoopControlBlockView) || treeParent.GetType() == typeof(SwitchControlBlockView) || treeParent.GetType() == typeof(SwitchTabBlockView))
				{
					depth++;
					if (tempChildHeight > maxChildHeight) maxChildHeight = tempChildHeight;
					tempChildHeight = treeParent.DesiredSize.Height;
					parent = treeParent;
				}

				// If a switch is found in the heirachy
				if (treeParent.GetType() == typeof(SwitchTabBlockView)) {
					// Add the switch view model to the collection for later size check
					switchBlockViews.Add((SwitchTabBlockView)treeParent);
				}

				if (treeParent.GetType() == typeof(WorkspaceView)) {
					workspace = (WorkspaceView)treeParent;
					break;
				}
			}
		}

		private void Item_Dropped(object sender, System.Windows.DragEventArgs e)
		{
			FrameworkElement dropTarget = (FrameworkElement)sender;
			ControlBlockViewModel targetViewModel = (ControlBlockViewModel)dropTarget.DataContext;

			FrameworkElement dropSource = (FrameworkElement)e.Data.GetData("Object");
			ControlBlockViewModel sourceViewModel = (ControlBlockViewModel)dropSource.DataContext;

			// Set the workspace and the parent
			setWorkspaceAndParent(dropTarget, dropSource);

			if (targetViewModel.GetType() == typeof(LoopControlBlockViewModel))
			{
				((LoopControlBlockViewModel)targetViewModel).AddChildBlock(dropTarget, dropSource, e.GetPosition(dropTarget).X);
			}
			else if (targetViewModel.GetType() == typeof(SwitchControlBlockViewModel))
			{
				// Set the parent for the child for checking the Y value
				FrameworkElement dropParent = (FrameworkElement)dropTarget.Parent;

				// Set the parent Y mouse value as relevant to the workspace or the current target
				double parentY = depth == 1 ? e.GetPosition(workspace).Y : e.GetPosition(dropTarget).Y;

				// Set the top value depending on the current depth since the top will be
				// different for a component on the workspace compared to one inside another component
				double top = depth == 1 ? (double)dropTarget.Parent.GetValue(Canvas.TopProperty) : 0;

				// If the mouse Y location is in the top or bottom half of the component when dropped
				if (parentY > top + (dropParent.RenderSize.Height / 2))
				{
					((SwitchControlBlockViewModel)targetViewModel).AddChildBlock(dropSource, SwitchControlBlockViewModel.ORIENTATION_BOTTOM);
				}
				else
				{
					((SwitchControlBlockViewModel)targetViewModel).AddChildBlock(dropSource, SwitchControlBlockViewModel.ORIENTATION_TOP);
				}
			}
			else if (targetViewModel.GetType() == typeof(SwitchTabBlockViewModel)) {
				((SwitchTabBlockViewModel)targetViewModel).AddChildBlock(dropTarget, dropSource, e.GetPosition(dropTarget).X);
			}

			// Expand the block
			if (dropSource is SwitchTabBlockView || dropSource is LoopControlBlockView) ExpandControlBlock(dropTarget, dropSource);

			e.Handled = true;
		}


		private void ExpandControlBlock(FrameworkElement dropTarget, FrameworkElement child) {
			// Setup the measured size
			child.Measure(new Size(child.MaxWidth, child.MaxHeight));

			// Setup the component height for comparison
			double parentHeight = 0;
			if ( ((FrameworkElement)dropTarget.Parent).GetType() == typeof(LoopControlBlockView) ||
			((FrameworkElement)dropTarget.Parent).GetType() == typeof(SwitchTabBlockView))
			{
				parentHeight = ((FrameworkElement)dropTarget.Parent).RenderSize.Height;
			} else if( ((FrameworkElement)dropTarget.Parent).GetType() == typeof(SwitchControlBlockView) ) {
				parentHeight = ((FrameworkElement)dropTarget.Parent).RenderSize.Height / 2;
			}

			// If the parent height is less than or the same as the child
			if (parentHeight <= child.DesiredSize.Height)
			{
				// Get the top point of the root parent
				double top = Canvas.GetTop(parent);

				// If the parent size minus the new child size will have less than
				// the minimum top+bottom buffer(50), meaning the child is bigger than the main parent
				if (!(parent.RenderSize.Height - child.DesiredSize.Height >= 50)) {
					double offset = ((child.DesiredSize.Height+(depth * 50)) - parent.RenderSize.Height)/2;
					top = top - offset;
				}

				// Otherwise if the new child is bigger or the same size as any other child so far!
				else if(child.DesiredSize.Height >= maxChildHeight) {
					// Make sure that when the max child expands it will have space!
					// e.g. the parent will fit the max size plus its buffer!
					if(child.DesiredSize.Height+(depth*50) > parent.RenderSize.Height) {
						double offset = ((child.DesiredSize.Height + (depth * 50)) - parent.RenderSize.Height) / 2;
						top = top - offset;
					}
				}
				else if (maxChildHeight + (50) >= parent.RenderSize.Height) {
					double offset = ((child.DesiredSize.Height + (depth * 50)) - parent.RenderSize.Height) / 2;
					top = top - offset;
				}

				// Set the top value
				parent.SetValue(Canvas.TopProperty, top);

				parent.Dispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate(Object state) {
					resizeSwitchBlocks();
					return null;
				}),null);
			}
		}

		private void resizeSwitchBlocks() {
			// Update the switches that were affected!
			foreach (SwitchTabBlockView switchView in switchBlockViews)
			{
				((SwitchTabBlockViewModel)switchView.DataContext).UpdateChildrenSizes();
			}
		}
	}
}