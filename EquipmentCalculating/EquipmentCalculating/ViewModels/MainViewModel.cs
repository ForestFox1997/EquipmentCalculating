using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EquipmentCalculating.Models;
using EquipmentCalculating.Models.Entities;
using System.Linq;
using System.Windows.Input;
using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace EquipmentCalculating.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly MainDataModel _dataModel;

        public MainViewModel()
        {
            _dataModel = new MainDataModel();

            Categories = new BindingList<Category>(_dataModel.Categories);

            Equipments = new ObservableCollection<Equipment>(_dataModel.Equipments);

            EquipmentList = new ListCollectionView(Equipments);
        }

        #region Команды
        public ICommand AddEquipmentCommand => new RelayCommand(AddEquipment);
        public ICommand RemoveEquipmentCommand => new RelayCommand(RemoveEquipment);
        public ICommand AddCategoryCommand => new RelayCommand(AddCategory);
        public ICommand RemoveCategoryCommand => new RelayCommand(RemoveCategory);
        public ICommand ActivateFilter => new RelayCommand(ApplyFilter);
        public ICommand DeactivateFilter => new RelayCommand(DisableFilter);
        public ICommand GenerateReportCommand => new RelayCommand(GenerateReport);
        public ICommand ActualizeCommand => new RelayCommand(SaveCurrentRecord);
        #endregion Команды

        public ListCollectionView EquipmentList { get; set; }

        ObservableCollection<Equipment> Equipments { get; set; }
        public Equipment SelectedEquipment { get; set; }

        public BindingList<Category> Categories { get; set; }
        public Category SelectedCategory { get; set; }

        public string LocationFilter { get; set; }

        public string NewCategory { get; set; }

        public Category CategoryFilter { get; set; }

        public void SaveCurrentRecord()
        {
            _dataModel.SaveEquipment(SelectedEquipment);
        }

        public void AddEquipment()
        {
            Equipments.Add(new Equipment() { Category = GetLastCategory() });
        }

        public void RemoveEquipment()
        {
            if (SelectedEquipment != null)
            {
                _dataModel.RemoveEquipment(SelectedEquipment);
                Equipments.Remove(SelectedEquipment);
            }
        }

        public void AddCategory()
        {
            bool categoryExist =
                Categories.Any(c => string.Equals(c.Name, NewCategory, StringComparison.CurrentCultureIgnoreCase));

            if (!categoryExist)
            {
                var newCategory = _dataModel.AddCategory(NewCategory);
                Categories.Add(newCategory);
            }
        }

        public void RemoveCategory()
        {
            if (SelectedCategory != null)
            {
                _dataModel.RemoveCategory(SelectedCategory);
                Categories.Remove(SelectedCategory);
            }
        }

        public void ApplyFilter()
        {
            EquipmentList.Filter = Filter;
        }

        public void DisableFilter()
        {
            EquipmentList.Filter = null;
        }

        private bool Filter(object obj)
        {
            var equipment = obj as Equipment;

            if (CategoryFilter != null && equipment.Category.Name != CategoryFilter.Name)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(LocationFilter) &&
                (string.IsNullOrEmpty(equipment.Location.Name) || !equipment.Location.Name.Contains(LocationFilter)))
            {
                return false;
            }

            return true;
        }

        private Category GetLastCategory()
        {
            return Categories.Last();
        }

        private void GenerateReport()
        {

        }
    }
}
