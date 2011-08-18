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
		public MainWindow()
		{
			this.InitializeComponent();
		}

		private void BrickDrag_brickDropped(object sender, System.Windows.DragEventArgs e)
		{
			// Block the event from being handled if handled already in the panel
			if(e.Handled) return;

			Panel panel = (Panel)sender;
			UIElement element = (UIElement)e.Data.GetData("Object");

			if(panel != null && element != null) {
				panel.Children.Add(element);
				element.SetValue(Canvas.LeftProperty,e.GetPosition(panel).X);
				element.SetValue(Canvas.TopProperty, e.GetPosition(panel).Y);
					
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
			// Set the brick button
			UIElement brickButton = (UIElement)sender;

			// Create the data object which will hold a brick in future
			DataObject data = new DataObject();

			// TEMPORARY
			if(sender is Rectangle) {
				Rectangle rect = new Rectangle();
				BrushConverter converter = new BrushConverter();
				rect.Fill = ((Rectangle)brickButton).Fill;
				rect.Width = 80;
				rect.Height = 50;
				data.SetData("Object",rect);
			}

			// Do the drag drop
			DragDrop.DoDragDrop(brickButton, data, DragDropEffects.Move | DragDropEffects.Copy);
		}

		private void BrickDrag_GiveFeedback(object sender, System.Windows.GiveFeedbackEventArgs e)
		{

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