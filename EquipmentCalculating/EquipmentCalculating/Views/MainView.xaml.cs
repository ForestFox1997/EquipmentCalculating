using EquipmentCalculating.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EquipmentCalculating.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EquipmentListView_CurrentCellChanged(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void ValueInControlIsChanged(object sender, TextChangedEventArgs e)
        {
            SaveRecord();
        }

        private void SaveRecord()
        {
            var dataContext = DataContext as MainViewModel;
            dataContext.ActualizeCommand.Execute(null);
        }

        private void CheckBox_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SaveRecord();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveRecord();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SaveRecord();
        }
    }
}
