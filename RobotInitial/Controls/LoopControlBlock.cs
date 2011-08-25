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
	///     <MyNamespace:CustomControl1/>
	///
	/// </summary>
	public class LoopControlBlock : ControlBlock
	{
		static LoopControlBlock()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(LoopControlBlock), new FrameworkPropertyMetadata(typeof(LoopControlBlock)));
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			base.Action = "Loop";
		}
	}
}
