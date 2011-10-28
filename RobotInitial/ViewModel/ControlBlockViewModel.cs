using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace RobotInitial.ViewModel
{
	// This class provides the ability to access certain
	// Standard properties expected of some Views,
	// Most importantly, the on drop functionality required
	// by the LoopControlBlockView and SwitchControlBlockView
	abstract class ControlBlockViewModel : ViewModelBase, INotifyPropertyChanged 
	{
		public String Type { get; set; }
		private double _topOffset = 0;
		private Thickness _offsetMargin = new Thickness(0, 0, 0, 0);
		public Thickness OffsetMargin
		{
			get
			{
				return _offsetMargin;
			}
			set
			{
				_offsetMargin = value;
			}
		}

		public Visibility AnimationVisible { get; set; }
		
		public void SetOffsetMargin(FrameworkElement element) {
			_topOffset -= 200;
			OffsetMargin = new Thickness(0,_topOffset,0,0);
		}

		public ControlBlockViewModel() {
			AnimationVisible = Visibility.Hidden;
		}

		public virtual void SetAnimationVisibility(Visibility v) {
			AnimationVisible = v;
			NotifyPropertyChanged("AnimationVisible");
		}

		public virtual Visibility GetAnimationVisibility() {
			return AnimationVisible;
		}

		public void StartSelectedAnimation(FrameworkElement element) {			
			ControlBlockViewModel viewModel = (ControlBlockViewModel)element.DataContext;
			if (viewModel.GetAnimationVisibility() == Visibility.Hidden) {
				Storyboard story = (Storyboard)element.FindResource("BlockSelected");
				viewModel.SetAnimationVisibility(Visibility.Visible);
				story.Begin(element, true);
			}
		}

		public void StopSelectedAnimation(FrameworkElement element) {
			ControlBlockViewModel viewModel = (ControlBlockViewModel)element.DataContext;
			if (viewModel.GetAnimationVisibility() == Visibility.Visible) {
				Storyboard story = (Storyboard)element.FindResource("BlockSelected");
				viewModel.SetAnimationVisibility(Visibility.Hidden);
				story.Stop(element);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notifies the property changed.
		/// </summary>
		/// <param name="property">The property.</param>
		private void NotifyPropertyChanged(string property) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(property));
			}
		}
	}
}
