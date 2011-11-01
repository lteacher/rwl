using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;

using RobotInitial.Command;
using RobotInitial.Properties;
using RobotInitial.Model;
using RobotInitial.View;
using System.Windows;
using System.Net;
using System.Xml;
using System.IO;
using System.Net.Sockets;
using LynxTest2.Communications;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;

namespace RobotInitial.ViewModel
{
	class MainWindowViewModel : ClosableViewModel, INotifyPropertyChanged 
    {

        #region Fields

        #region Commands

        RelayCommand _newWorkspaceCommand;
        RelayCommand _openWorkspaceCommand;
        RelayCommand _saveWorkspaceCommand;
        RelayCommand _saveAsWorkspaceCommand;
        RelayCommand _closeWorkspaceCommand;

        RelayCommand _undoCommand;
        RelayCommand _redoCommand;

        #endregion // Commands
     
		//ObservableCollection<WorkspaceViewModel> _workspaces;
		//ObservableCollection<TaskBlockTabViewModel> _brickTabs;

		private bool connected = false;
		public bool AddressesEnabled { get; set; }
		private delegate void NoArgDelegate();
		private delegate void OneArgDelegate(string arg);
		public Visibility ConnectButtonVisibility { get; set; }
		public Visibility DisconnectButtonVisibility { get; set; }
		public Collection<string> _robotNames = new Collection<string>();
		public Collection<IPEndPoint> _robotEndpoints = new Collection<IPEndPoint>();
		public Collection<string> RobotNames {
			get { return _robotNames; }
		}

		// The workspace views that can show in the tab control
		ObservableCollection<WorkspaceView> _workspaces = new ObservableCollection<WorkspaceView>();
        public ObservableCollection<WorkspaceView> Workspaces {
			get { return _workspaces; }
		}

		// The brick tabs on the left side, theres really just one tab for now
		ObservableCollection<TaskBlockTabView> _brickTabs = new ObservableCollection<TaskBlockTabView>();
		public ObservableCollection<TaskBlockTabView> BrickTabs {
			get { return _brickTabs; }
		}

		// The selected index of whatever workspace is active, used to get the currently selected index... since i know it will work
		public int SelectedIndex { get; set; }

		// Selected address
		public int SelectedAddress { get; set; }

		// Set the current text of the address box
		public string CurrentAddressText { get; set; }

		// Now the current/active workspace is easy to get, its the item in the collection from the selected index
		public WorkspaceView GetCurrentWorkspace() {
			return Workspaces[SelectedIndex];
		}

		public WorkspaceViewModel ActiveWorkspaceViewModel {
			get {
				return (WorkspaceViewModel)GetCurrentWorkspace().DataContext;
			}
		}

        bool _undoEnabled = false;
        bool _redoEnabled = false;

        #endregion // Fields

        public MainWindowViewModel()
        {
            base.DisplayName = Resources.applicationDisplayName;
			// Create a new workspace
			CreateNewWorkspace();

			// Just add a bricktabs probably there wont ever be any more tabs
			BrickTabs.Add(new TaskBlockTabView());

			// Initialise the connect and disconnect buttons
			ConnectButtonVisibility = Visibility.Visible;
			DisconnectButtonVisibility = Visibility.Hidden;

			////=== TESTING ONLY, START A LOCAL LYNX SERVER!
			//Thread ServerThread;
			//Lynx_Server.Lynx_Server server = new Lynx_Server.Lynx_Server();
			//ServerThread = new Thread(server.start);
			//ServerThread.Start();
			//Console.WriteLine("SERVER IS RUNNING");
			////====================================================

			// Update the RobotNames address list
			updateRobotAddressList();
		}
			
		public void updateRobotAddressList() {
			CurrentAddressText = "Updating Robot List";
			AddressesEnabled = false;
			NotifyPropertyChanged("AddressesEnabled");
			NotifyPropertyChanged("CurrentAddressText");
			NoArgDelegate addressUpdate = new NoArgDelegate(checkAddressConnections);
			addressUpdate.BeginInvoke(null, null);
		}

		private void checkAddressConnections() {
			string robotsXML = Resources.Robots;

			using (XmlReader reader = XmlReader.Create(new StringReader(robotsXML))) {
				
				bool IPAdded = false;
				while (reader.Read()) {
					if(reader.NodeType == XmlNodeType.Element) {
						// If the element name is IPAddress
						if (reader.Name == "IPAddress") {
							// Move to the nodes value
							string value = reader.ReadElementContentAsString();

							// Check the value isnt empty
							if(value != "") {
								try {
									IPHostEntry IpToDomainName = Dns.GetHostEntry(value);
									IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(value), Network.DefaultPort);
									if (Network.Instance.robotConnectionAvail(endpoint)) {
										// Add the EndPoint
										_robotEndpoints.Add(endpoint);

										// Set the added flag
										IPAdded = true;
									}
								}
								catch (SocketException exc) {
									Console.WriteLine("EXCEPTION ({1}): {0}", exc, value);
								}
							}
						}

						// If the element name is HostName
						if (reader.Name == "HostName") {
							// Skip the HostName if the IP is already added
							if(IPAdded) continue;

							// Move to the nodes value
							string value = reader.ReadElementContentAsString();

							// Check the value isnt empty
							if (value != "") {
								try {
									// NOTE: Here im using the first address, there could be more than one!!
									IPHostEntry IpToDomainName = Dns.GetHostEntry(value);
									IPEndPoint endpoint = new IPEndPoint(IpToDomainName.AddressList[0], Network.DefaultPort);
									if (Network.Instance.robotConnectionAvail(endpoint)) {
										// Add the EndPoint
										_robotEndpoints.Add(endpoint);

										// Set the added flag
										IPAdded = true;
									}
								}
								catch (SocketException exc) {
									Console.WriteLine("EXCEPTION ({1}): {0}", exc, value);
								}
							}
						}

						// If the element name is DisplayName
						if (reader.Name == "DisplayName") {
							// If the IP is added
							if(IPAdded) {
								// Move to the nodes value
								string value = reader.ReadElementContentAsString();

								// Add the displayName
								RobotNames.Add(value);

								// Set the flag back to false
								IPAdded = false;
							}
						}
					}
				}
			}

			// Update the properties back on the main UI thread
			Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				 new NoArgDelegate(robotNamesUpdated));
		}

		// Update a bunch of properties
		private void robotNamesUpdated() {
			AddressesEnabled = true;
			CurrentAddressText = RobotNames.Count>0 ? RobotNames[0]: "No Default Robots Available (Enter an IPAddress)";
			NotifyPropertyChanged("AddressesEnabled");
			NotifyPropertyChanged("RobotNames");
			NotifyPropertyChanged("CurrentAddressText");
		}

		//#region Command Definitions

		//#region NewWorkspaceCommand

		public ICommand NewWorkspaceCommand {
			get {
				if (_newWorkspaceCommand == null) {
					_newWorkspaceCommand = new RelayCommand(param => this.CreateNewWorkspace());
				}
				return _newWorkspaceCommand;
			}
		}

		private void CreateNewWorkspace() {
			WorkspaceView workspace = new WorkspaceView();
			((WorkspaceViewModel)workspace.DataContext).DisplayName = Resources.untitledFileName;
			Workspaces.Add(workspace);
			// Shamelessly reference the MainWindowView and its start stop control, sorry MVVM
			((MainWindowView)Application.Current.MainWindow).StartStopControl.MainGrid.SetValue(UIElement.VisibilityProperty, Visibility.Visible);
		}

		//#endregion // NewWorkspaceCommand

		//#region OpenWorkspaceCommand

		//public ICommand OpenWorkspaceCommand
		//{
		//    get
		//    {
		//        if (_openWorkspaceCommand == null)
		//        {
		//            _openWorkspaceCommand = new RelayCommand(param => this.OpenWorkspace());
		//        }
		//        return _openWorkspaceCommand;
		//    }
		//}

		//#endregion // OpenWorkspaceCommand

		//#region SaveWorkspaceCommand

		//public ICommand SaveWorkspaceCommand
		//{
		//    get
		//    {
		//        if (_saveWorkspaceCommand == null)
		//        {
		//            _saveWorkspaceCommand = new RelayCommand(param => this.SaveWorkspace(false));
		//        }
		//        return _saveWorkspaceCommand;
		//    }
		//}

		//#endregion // SaveWorkspaceCommand

		//#region SaveAsWorkspaceCommand

		//public ICommand SaveAsWorkspaceCommand
		//{
		//    get
		//    {
		//        if (_saveAsWorkspaceCommand == null)
		//        {
		//            _saveAsWorkspaceCommand = new RelayCommand(param => this.SaveWorkspace(true));
		//        }
		//        return _saveAsWorkspaceCommand;
		//    }
		//}

		//#endregion // SaveWorkspaceAsCommand

		//#region CloseWorkspaceCommand

		public ICommand CloseWorkspaceCommand {
			get {
				if (_closeWorkspaceCommand == null) {
					_closeWorkspaceCommand = new RelayCommand(param => this.CloseWorkspace());
				}
				return _closeWorkspaceCommand;
			}
		}

		//#endregion // CloseWorkspaceCommand

		//#region UndoCommand

		//public ICommand UndoCommand
		//{
		//    get
		//    {
		//        if (_undoCommand == null)
		//        {
		//            _undoCommand = new RelayCommand(param => this.GetCurrentWorkspace().Undo());
		//        }
		//        return _undoCommand;
		//    }
		//}

		//#endregion // UndoCommand

		//#region RedoCommand
        
		//public ICommand RedoCommand
		//{
		//    get
		//    {
		//        if (_redoCommand == null)
		//        {
		//            _redoCommand = new RelayCommand(param => this.GetCurrentWorkspace().Redo());
		//        }
		//        return _redoCommand;
		//    }
		//}
        
		//#endregion // RedoCommand

		//#endregion // Command Definitions

//        #region Property Bindings

//        public bool IsRedoEnabled
//        {
//            get { return GetCurrentWorkspace().IsRedoEnabled; }
//        }

//        public bool IsUndoEnabled
//        {
//            get { return GetCurrentWorkspace().IsUndoEnabled; }
//        }

//        #endregion // Property Bindings

//        #region Collection Definitions

//        #region Workspaces

//        public ObservableCollection<WorkspaceViewModel> Workspaces
//        {
//            get
//            {
//                if (_workspaces == null)
//                {
//                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
//                    _workspaces.CollectionChanged += OnWorkspacesChanged;
//                }

//                return _workspaces;
//            }
//        }

//        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            if (e.NewItems != null && e.NewItems.Count != 0)
//            {
//                foreach (WorkspaceViewModel workspace in e.NewItems)
//                {
//                    workspace.RequestClose += this.OnWorkspaceRequestClose;
//                }

//            }

//            if (e.OldItems != null && e.OldItems.Count != 0)
//            {
//                foreach (WorkspaceViewModel workspace in e.OldItems)
//                {
//                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
//                }
//            }
//        }

//        void OnWorkspacePropertyChanged(object sender, EventArgs e)
//        {
//            Debug.WriteLine("Property Changed: " + e);
//            if (e.ToString().Equals("IsUndoEnabled"))
//            {
//                OnPropertyChanged("IsUndoEnabled");
//            }
//            else if (e.ToString().Equals("IsRedoEnabled"))
//            {
//                OnPropertyChanged("IsRedoEnabled");
//            }

//        }

//        void OnWorkspaceRequestClose(object sender, EventArgs e)
//        {
//            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
//            workspace.Dispose();
//            this.Workspaces.Remove(workspace);
//        }

//#endregion // Workspaces

//        #region BrickTabs

//        public ObservableCollection<TaskBlockTabViewModel> BrickTabs
//        {
//            get
//            {
//                if (_brickTabs == null)
//                {
//                    _brickTabs = new ObservableCollection<TaskBlockTabViewModel>();
//                    _brickTabs.CollectionChanged += OnBricksTabChanged;
//                }

//                return _brickTabs;

//            }
//        }

//        void OnBricksTabChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            if (e.NewItems != null && e.NewItems.Count != 0)
//            {
//                foreach (TaskBlockTabViewModel brickTab in e.NewItems)
//                {
//                    brickTab.RequestClose += this.OnBrickTabRequestClose;
//                }

//            }

//            if (e.OldItems != null && e.OldItems.Count != 0)
//            {
//                foreach (TaskBlockTabViewModel brickTab in e.OldItems)
//                {
//                    brickTab.RequestClose -= this.OnBrickTabRequestClose;
//                }
//            }
//        }

//        void OnBrickTabRequestClose(object sender, EventArgs e)
//        {
//            TaskBlockTabViewModel brickTab = sender as TaskBlockTabViewModel;
//            brickTab.Dispose();
//            this.BrickTabs.Remove(brickTab);
//        }

//        #endregion // BrickTabs

//        #endregion // Collection Definitions

//        #region Private Helper Methods

//        void Initialise()
//        {
//            TaskBlockTabViewModel btmodel = new TaskBlockTabViewModel();
//            this.BrickTabs.Add(btmodel);

//            // Create a new workspace on startup
//            CreateNewWorkspace();
//        }

//        void CreateNewWorkspace()
//        {
//            Workspace workspaceModel = Workspace.CreateNewWorkspace();
//            WorkspaceViewModel workspace = new WorkspaceViewModel();
//            workspace.DisplayName = Resources.untitledFileName;
//            this.Workspaces.Add(workspace);
//            //workspace.PropertyChanged += new PropertyChangedEventHandler(OnWorkspacePropertyChanged);
//            this.SetActiveWorkspace(workspace);
//        }

//        void SetActiveWorkspace(WorkspaceViewModel workspace)
//        {
//            Debug.Assert(Workspaces.Contains(workspace));

//            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
//            if (collectionView != null)
//            {
//                collectionView.MoveCurrentTo(workspace);
//                NotifyPropertyChanged("Workspaces");
//            }
//        }

//        void OpenWorkspace()
//        {
//            // Show Dialog (this is a non-trivial task in MVVM)
//            // Get filename from dialog
//            Workspace workspaceModel = Workspace.LoadWorkspace("rawr.rwl");
//            //WorkspaceViewModel workspace = new WorkspaceViewModel(workspaceModel);
//            WorkspaceViewModel workspace = new WorkspaceViewModel();
//            this.Workspaces.Add(workspace);
//            this.SetActiveWorkspace(workspace);
//        }

//        void SaveWorkspace(bool newFile)
//        {
//            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
//            if (collectionView != null)
//            {
//                WorkspaceViewModel viewmodel = collectionView.CurrentItem as WorkspaceViewModel;
//                if (viewmodel != null)
//                {
//                    if (viewmodel.IsUntitled || newFile)
//                    {
//                        // Show Dialog (this is a non-trivial task in MVVM)
//                        // Get filename from dialog
//                    }

//                    // Write Workspace Model to disk
//                    // Mark Workspace as saved.
//                }
                    
//            }
//        }

		void CloseWorkspace() {
			// Save Workspace.
			Workspaces.RemoveAt(SelectedIndex);
			if(Workspaces.Count == 0) {
				SelectedIndex = 0;

				// Closing all pages resets the properties to blank!
				((PropertiesTabViewModel)((MainWindowView)Application.Current.MainWindow).PropertiesView.DataContext).setBlankProperties();

				// Shamelessly reference the MainWindowView and its start stop control, sorry MVVM
				((MainWindowView)Application.Current.MainWindow).StartStopControl.MainGrid.SetValue(UIElement.VisibilityProperty, Visibility.Hidden);
				NotifyPropertyChanged("SelectedIndex");
				return;
			}

			// If theres still workspaces open get the now selected one and update the properties
			if (ActiveWorkspaceViewModel.SelectedBlock != null) {
				((PropertiesTabViewModel)((MainWindowView)Application.Current.MainWindow).PropertiesView.DataContext).setPropertiesView(
										((ControlBlockViewModel)ActiveWorkspaceViewModel.SelectedBlock.DataContext).PropertiesView);
			}

			
		}

//        WorkspaceViewModel GetCurrentWorkspace()
//        {
//            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
//            return collectionView.CurrentItem as WorkspaceViewModel;
//        }


//        #endregion // Private Helper Methods

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
