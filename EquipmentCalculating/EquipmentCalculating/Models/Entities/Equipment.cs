namespace EquipmentCalculating.Models.Entities
{
    public class Equipment
    {
        internal long? Id { get; set; }

        public string SerialNumber { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; } = new Category();

        public bool GoodCondition { get; set; }

        public Location Location { get; set; } = new Location();
    }
}
