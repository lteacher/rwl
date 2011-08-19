using System;
using System.ComponentModel;
using System.Diagnostics;

namespace RobotInitial.ViewModel
{
    // Based on the demo application from the MVVM tutorial
    // at http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    //
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {

        #region Constructor
        protected ViewModelBase()
        {
        }

        #endregion // Constructor

        #region DisplayName
        public virtual string DisplayName { get; protected set; }

        #endregion // DisplayName

        #region Debug Helper Methods

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                {
                    throw new Exception(msg);
                }
                else
                {
                    Debug.Fail(msg);
                }
            }
        }

        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debug Helper Methods

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion // INotifyPropertyChanged Members

        #region IDisposable Members

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        {
        }

#if DEBUG

        ~ViewModelBase()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
            Debug.WriteLine(msg);
        }
#endif

        #endregion // IDisposable Members

    }
}
