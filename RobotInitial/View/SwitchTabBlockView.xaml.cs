using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RobotInitial.ViewModel;

namespace RobotInitial.View
{
	/// <summary>
	/// Interaction logic for SwitchTabBlockView.xaml
	/// </summary>
	public partial class SwitchTabBlockView : UserControl
	{
		public SwitchTabBlockView()
		{
			InitializeComponent();
		}

		// Taking a short cut since i really cant be bothered with this MVVM Crap right now
		private void Ellipse_TopButtonDown(object sender, MouseButtonEventArgs e)
		{
			((SwitchTabBlockViewModel)this.DataContext).TopButtonAction();
		}

		// Taking a short cut since i really cant be bothered with this MVVM Crap right now
		private void Ellipse_BottomButtonDown(object sender, MouseButtonEventArgs e)
		{
			((SwitchTabBlockViewModel)this.DataContext).BottomButtonAction();
		}
	}
}
