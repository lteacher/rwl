using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows;
using RobotInitial.ViewModel;
using RobotInitial.View;

namespace RobotInitial.Behaviours {
	class ScrollViewerCentering : Behavior<ScrollViewer> {
		
		public ScrollViewerCentering()
		{
			// Insert code required on object creation below this point.
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.SizeChanged +=new SizeChangedEventHandler(AssociatedObject_SizeChanged);
		}

		private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e) {
			WorkspaceView workspaceView = (WorkspaceView)AssociatedObject.Parent;
			WorkspaceViewModel workspaceViewModel = (WorkspaceViewModel)workspaceView.DataContext;
			double offset = workspaceViewModel.SequenceY;
			AssociatedObject.ScrollToVerticalOffset(offset/2+40);
		}

	}
}
