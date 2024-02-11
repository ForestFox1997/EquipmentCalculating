namespace EquipmentCalculating.Models.Entities
{
    public class Category
    {
        internal long Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
