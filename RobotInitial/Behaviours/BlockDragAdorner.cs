using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using RobotInitial.Controls;
using System.Windows.Controls;
using RobotInitial.View;
using RobotInitial.ViewModel;

namespace RobotInitial 
{
    class BlockDragAdorner : Adorner
    {
        protected UIElement _child;
        protected UIElement _owner;
        protected double XCenter;
        protected double YCenter;

        public BlockDragAdorner(UIElement owner) : base(owner) { }

        public BlockDragAdorner(UIElement owner, FrameworkElement adornElement, double opacity)
            : base(owner)
        {
            System.Diagnostics.Debug.Assert(owner != null);
            System.Diagnostics.Debug.Assert(adornElement != null); 

            _owner = owner;

			ControlBlockViewModel viewModel = (ControlBlockViewModel)adornElement.DataContext;

			if(viewModel.Type == "Move") _child = new MoveControlBlockView();
			if(viewModel.Type == "Loop") _child = new LoopControlBlockView();
			if(viewModel.Type == "Wait") _child = new WaitControlBlockView();
			//if (viewModel.Type == "Switch") _child = new SwitchControlBlockView();
			if (viewModel.Type == "Switch") _child = new SwitchTabBlockView();

			_child.Opacity = opacity;

			// Set the centre points
            XCenter = 0; // r.Width / 2;
            YCenter = 0; // r.Height / 2;

			//_child = ctrl;    
        }


        private double _leftOffset = 0;
        public double LeftOffset
        {
            get { return _leftOffset; }
            set
            {
                _leftOffset = value; // - XCenter;
                UpdatePosition();
            }
        }

        private double _topOffset = 0;
        public double TopOffset
        {
            get { return _topOffset; }
            set
            {
                _topOffset = value; // -YCenter;

                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            AdornerLayer adorner = (AdornerLayer)this.Parent;
            if (adorner != null)
            {
                adorner.Update(this.AdornedElement);
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _child;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            _child.Measure(finalSize);
            return _child.DesiredSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {

            _child.Arrange(new Rect(_child.DesiredSize));
            return finalSize;
        }

        public double scale = 1.0;
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();

            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));
            return result;
        }
    }
}
