namespace EquipmentCalculating.Models.Entities
{
    public class Category
    {
        internal int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
