using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RobotInitial.ViewModel
{
    class BrickTabViewModel : ClosableViewModel
    {

        ObservableCollection<BrickViewModel> _bricks;

        public BrickTabViewModel()
        {
            base.DisplayName = "C";
            Initialize();
        }

        public ObservableCollection<BrickViewModel> Bricks
        {
            get
            {
                if (_bricks == null)
                {
                    _bricks = new ObservableCollection<BrickViewModel>();
                }

                return _bricks;
            }

        }

        void Initialize()
        {
			//Bricks.Add(new BrickViewModel("#FF00FF33"));
			//Bricks.Add(new BrickViewModel("Red"));
			//Bricks.Add(new BrickViewModel("#FF0061FF"));
			//Bricks.Add(new BrickViewModel("#FFFFEA00"));

        }
    }
}
