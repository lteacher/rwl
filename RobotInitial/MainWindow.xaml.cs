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
using Microsoft.Win32;

namespace RobotInitial
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public FrameworkElement DragScope { get; set; }
		private BrickDragAdorner brickAdorner = null;
		private AdornerLayer adornLayer;
		private bool _dragHasLeftScope = false;

		public MainWindow()
		{
			this.InitializeComponent();
		}

		private void BrickDrag_brickDropped(object sender, System.Windows.DragEventArgs e)
		{
			// Block the event from being handled if handled already in the panel
			if(e.Handled) return;

			Panel panel = (Panel)sender;

			// AGAIN WE ASSUME THAT THIS IS A RECTANGLE
			Rectangle element = (Rectangle)e.Data.GetData("Object");

			if(panel != null && element != null) {
				double x = e.GetPosition(panel).X;
				double y = e.GetPosition(panel).Y;
				panel.Children.Add(element);

				// Note we are dropping from the centre of the brick
				element.SetValue(Canvas.LeftProperty, x - (element.Width / 2));
				element.SetValue(Canvas.TopProperty, y - (element.Height / 2));
					
				// set the value to return to the DoDragDrop call
				e.Effects = DragDropEffects.Copy;
			}
		}

		private void BrickDrag_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			// Capture the mouse, use to create drag drop
			//((UIElement)sender).CaptureMouse();
		}

		private void BrickDrag_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if(e.LeftButton == MouseButtonState.Pressed) {
				// Set the brick button
				UIElement brickButton = (UIElement)sender;

				// Create the data object which will hold a brick in future
				DataObject data = new DataObject();

				// TEMPORARY
				if (sender is Rectangle)
				{
					Rectangle rect = new Rectangle();
					BrushConverter converter = new BrushConverter();
					rect.Fill = ((Rectangle)brickButton).Fill;
					rect.Stroke = ((Rectangle)brickButton).Stroke;
					rect.Width = 80;
					rect.Height = 50;
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
		}

		private void BrickDrag_GiveFeedback(object sender, System.Windows.GiveFeedbackEventArgs e)
		{

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
		private void openFileDialog(object sender, System.Windows.RoutedEventArgs e)
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "RWL Files | *.rwl";
			openFile.ShowDialog();
		}
		
		private void saveFileDialog(object sender, System.Windows.RoutedEventArgs e)
		{
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = "RWL File | *.rwl";
			saveFile.ShowDialog();
		}

		
	}
}