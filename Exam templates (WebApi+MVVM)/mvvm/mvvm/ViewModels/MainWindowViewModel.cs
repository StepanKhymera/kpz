using mvvm.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows.Input;

namespace mvvm.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
            
        }

        public ObservableCollection<Product> products { get; set; }

        DataContext _dataContext;
        public MainWindowViewModel(DataContext db)
        {
            _dataContext = db;
            db.Products.Load();
            products = db.Products.Local;
           
        }

        private DelegateCommand addNewCommand;

        public ICommand AddNewCommand
        {
            get
            {
                if (addNewCommand == null)
                {
                    addNewCommand = new DelegateCommand(AddItem);
                }
                return addNewCommand;
            }
        }

        private void AddItem()
        {
            products.Add(new Product());            
        }

        private DelegateCommand saveCommand;

        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new DelegateCommand(Save);
                }
                return saveCommand;
            }
        }

        private void Save()
        {
            _dataContext.SaveChanges();
        }
    }
}
