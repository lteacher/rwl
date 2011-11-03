using System.Windows;
using System.Threading;
using LynxTest2.Communications;
using RobotInitial.Lynx_Server;
using System;
using RobotInitial.ViewModel;

namespace RobotInitial.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindowView : Window
	{
		private delegate void NoArgDelegate();
		private delegate void OneArgDelegate(string arg);

		public MainWindowView()
		{
			this.InitializeComponent();
            _cons_MainWindow();
			
		}
	}
}