using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using RobotInitial.View;
using RobotInitial.ViewModel;
using RobotInitial.Controls;

namespace RobotInitial.Behaviours
{
	class DropToSequence : Behavior<UIElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.Drop += new DragEventHandler(Item_Dropped);
		}

		private void Item_Dropped(object sender, System.Windows.DragEventArgs e) {
			FrameworkElement dropTarget = (FrameworkElement)sender;
			SequenceViewModel sequenceViewModel = (SequenceViewModel)dropTarget.DataContext;

			FrameworkElement dropSource = (FrameworkElement)e.Data.GetData("Object");

			sequenceViewModel.AddBlock(dropTarget, dropSource, e.GetPosition(dropTarget).X);

			e.Handled = true;
				
		}
	}
}
