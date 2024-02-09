using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using EquipmentCalculating.Data;
using EquipmentCalculating.Models.Entities;

namespace EquipmentCalculating.Models
{
    public class MainDataModel
    {
        public MainDataModel()
        {
            // TODO !!! Заполнить из БД
            Repository.Get();
        }

        public IEnumerable<Equipment> Equipments { get; set; }

        public IEnumerable<Category> Categories { get; set; }


    }
}
