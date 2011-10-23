using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using RobotInitial.View;
using RobotInitial.ViewModel;

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
			ControlBlockViewModel sourceViewModel = (ControlBlockViewModel)dropSource.DataContext;

			if(sourceViewModel.Type == "Move") {
				MoveControlBlockView childBlock = new MoveControlBlockView();

				sequenceViewModel.AddBlock(dropTarget, childBlock, e.GetPosition(dropTarget).X);
			}
			else if (sourceViewModel.Type == "Wait")
			{
				WaitControlBlockView childBlock = new WaitControlBlockView();
				sequenceViewModel.AddBlock(dropTarget, childBlock, e.GetPosition(dropTarget).X);
			}
			else if (sourceViewModel.Type == "Loop")
			{
				LoopControlBlockView childBlock = new LoopControlBlockView();

				sequenceViewModel.AddBlock(dropTarget, childBlock, e.GetPosition(dropTarget).X);
			}
			else if (sourceViewModel.Type == "Switch")
			{
				SwitchTabBlockView childBlock = new SwitchTabBlockView();

				sequenceViewModel.AddBlock(dropTarget, childBlock, e.GetPosition(dropTarget).X);
			}

			e.Handled = true;
			

			
		}
	}
}
