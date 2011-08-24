using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RobotInitial.Command;
using RobotInitial.Properties;
using RobotInitial.Model;
using System.Windows.Input;
using System.Windows;

namespace RobotInitial.ViewModel
{
    class BrickViewModel : ViewModelBase
    {

        string _fillColor;

        public String FillColor
        {
            get { return _fillColor; }
        }

        public BrickViewModel(string fillColor)
        {
            _fillColor = "Red";
        }

    }
}
