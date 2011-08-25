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

namespace RobotInitial.Controls
{
	/// <summary>
	///
	///     <MyNamespace:ControlBlock/>
	///
	/// </summary>
	public class ControlBlock : Control
	{
		static ControlBlock()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlBlock), new FrameworkPropertyMetadata(typeof(ControlBlock)));
		}

		static readonly DependencyProperty ActionProperty = DependencyProperty.Register("Action", typeof(String), typeof(ControlBlock));

		public String Action
		{
			get
			{
				return (String)GetValue(ActionProperty);
			}
			set
			{
				SetValue(ActionProperty, value);
			}
		}
	}
}
