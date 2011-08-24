﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RobotInitial.Behaviours
{
	class DragWithAdornment : Behavior<UIElement>
	{
		public FrameworkElement DragScope { get; set; }
		private BrickDragAdorner brickAdorner = null;
		private AdornerLayer adornLayer;
		private bool _dragHasLeftScope = false;
		protected override void OnAttached()
		{

			AssociatedObject.MouseMove += (sender, e) => {
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					// Set the brick button
					UIElement brickButton = (UIElement)sender;

					// Create the data object which will hold a brick in future
					DataObject data = new DataObject();

					// TEMPORARY BRICK, LOVELY COLOURED BLOCKS
					if (sender is Rectangle)
					{
						Rectangle rect = new Rectangle();
						BrushConverter converter = new BrushConverter();
						rect.Fill = ((Rectangle)brickButton).Fill;
						rect.Stroke = ((Rectangle)brickButton).Stroke;
						rect.Width = 75;
						rect.Height = 75;
						rect.RadiusX = 4.0;
						rect.RadiusY = 4.0;

						data.SetData("Object", rect);


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
						brickAdorner = new BrickDragAdorner(DragScope, (UIElement)rect, 0.5);
						adornLayer = AdornerLayer.GetAdornerLayer(DragScope as Visual);
						adornLayer.Add(brickAdorner);

						// Do the drag drop
						DragDrop.DoDragDrop(brickButton, data, DragDropEffects.Move | DragDropEffects.Copy);

						// Remove the Adorner from the layer
						DragScope.AllowDrop = previousDrop;
						AdornerLayer.GetAdornerLayer(DragScope).Remove(brickAdorner);
						brickAdorner = null;

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
			if (brickAdorner != null)
			{
				brickAdorner.LeftOffset = args.GetPosition(DragScope).X /* - _startPoint.X */ ;
				brickAdorner.TopOffset = args.GetPosition(DragScope).Y /* - _startPoint.Y */ ;
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