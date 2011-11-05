using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RobotInitial.Services
{
    public interface IDialogService
    {
		string FileName { get; }
		bool OpenFileDialog();
        //string? ShowOpenFileDialog();
        //string? ShowSaveFileDialog();
        //string? ShowMessageDialog(string text, string title, MessageBoxButton button, MessageBoxImage image);
    }
}
