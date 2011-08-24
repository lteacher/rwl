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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RobotInitial.ViewModel;

namespace RobotInitial.View
{
	/// <summary>
	/// Interaction logic for BlockView.xaml
	/// </summary>
	public partial class BlockView : UserControl
	{
		// Im Breaking the MVVM Rules already... very irritating to implement
		// But i just want to have this property OUTSIDE!! of the user control!!
		public String BlockFill { 
			get { 
				return ((BlockViewModel)DataContext).FillColor;
				}

			set {
				((BlockViewModel)DataContext).FillColor = value;
			} 
		}

		public BlockView()
		{
			this.InitializeComponent();
			DataContext = new BlockViewModel();
		}
	}
}