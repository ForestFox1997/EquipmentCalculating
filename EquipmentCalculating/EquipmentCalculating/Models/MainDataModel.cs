using System.Collections.Generic;
using System.Linq;
using EquipmentCalculating.Data;
using EquipmentCalculating.Models.Entities;

namespace EquipmentCalculating.Models
{
    public class MainDataModel
    {
        private Repository _repository;

        public MainDataModel()
        {
            _repository = new Repository();

            Categories = _repository.GetCategories();

            if (Categories.Count == 0)
            {
                Categories = new List<Category>() { new Category { Id = 0, Name = string.Empty} };
            }

            Equipments = _repository.GetEquipments();

            foreach (var equipment in Equipments)
            {
                if (Categories.Any(c => c.Id == equipment.Category.Id))
                {
                    equipment.Category = Categories.FirstOrDefault(c => c.Id == equipment.Category.Id);
                }
            }
        }

        public IList<Equipment> Equipments { get; set; }

        public IList<Category> Categories { get; set; }

        public Category AddCategory(string name)
        {
            return _repository.AddCategory(new Category { Name = name });
        }

        public void RemoveCategory(Category category)
        {
            _repository.RemoveCategory(category);
        }

        public void SaveEquipment(Equipment equipment)
        {
            _repository.SaveEquipment(equipment);
        }

        public void RemoveEquipment(Equipment equipment)
        {
            _repository.RemoveEquipment(equipment);
        }
    }
}
