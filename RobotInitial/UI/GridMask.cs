using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Diagnostics;

namespace RobotInitial
{
	class GridMask : FrameworkElement
	{
		private VisualCollection children;
		private UIElement parent;
		public int RowWidth {get; set;}
		public int ColWidth {get; set;}

        private SizeChangedEventHandler handler;

		public GridMask() {
			// Default properties unless reset
			RowWidth = 50;
			ColWidth = 50;		

			// Instantiate the collection
			children = new VisualCollection(this);

			// Add the event handler for MouseLeftButtonUp.
			this.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(MyVisualHost_MouseLeftButtonUp);

            handler = new SizeChangedEventHandler(parent_SizeChanged);
		}

        // Update parent whenever it changes.
        // Update the SizeChanged listener to handle invalidation.
        protected override void  OnVisualParentChanged(DependencyObject oldParent)
        {
 	        base.OnVisualParentChanged(oldParent);
            parent = (UIElement) Parent;

            if (parent is Canvas)
                (parent as Canvas).SizeChanged += handler;

            if (oldParent is Canvas)
                (oldParent as Canvas).SizeChanged -= handler;
        }

        void parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InvalidateVisual();
        }

		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);

            if (parent == null)
                return;


			//Draw the horizontal lines  
			Point a = new Point(0, 0);
			Point b = new Point(parent.RenderSize.Width, 0);

			Pen linePen = new Pen(Brushes.Black, 0.3);
			while (a.Y <= parent.RenderSize.Height)
			{
				drawingContext.DrawLine(linePen, a, b);
				a.Offset(0, RowWidth);
				b.Offset(0, RowWidth);	
			}

			//Draw the vertical lines  
			a = new Point(0, 0);
			b = new Point(0, parent.RenderSize.Height);
			while (a.X <= parent.RenderSize.Width)
			{
				drawingContext.DrawLine(linePen, a, b);
				a.Offset(ColWidth, 0);
				b.Offset(ColWidth, 0);
			}
		}

		// Provide a required override for the VisualChildrenCount property.
		protected override int VisualChildrenCount
		{
		    get { return children.Count; }
		}

		// Provide a required override for the GetVisualChild method.
		protected override Visual GetVisualChild(int index)
		{
			if (index < 0 || index >= children.Count)
			{
				throw new ArgumentOutOfRangeException();
			}

			return children[index];
		}

		// Capture the mouse event and hit test the coordinate point value against
		// the child visual objects.
		void MyVisualHost_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			// Retreive the coordinates of the mouse button event.
			System.Windows.Point pt = e.GetPosition((UIElement)sender);

			// Initiate the hit test by setting up a hit test result callback method.
			VisualTreeHelper.HitTest(this, null, new HitTestResultCallback(myCallback), new PointHitTestParameters(pt));
		}

		// If a child visual object is hit, toggle its opacity to visually indicate a hit.
		public HitTestResultBehavior myCallback(HitTestResult result)
		{
			if (result.VisualHit.GetType() == typeof(DrawingVisual))
			{
				if (((DrawingVisual)result.VisualHit).Opacity == 1.0)
				{
					((DrawingVisual)result.VisualHit).Opacity = 0.4;
				}
				else
				{
					((DrawingVisual)result.VisualHit).Opacity = 1.0;
				}
			}

			// Stop the hit test enumeration of objects in the visual tree.
			return HitTestResultBehavior.Stop;
		}
	}

}
