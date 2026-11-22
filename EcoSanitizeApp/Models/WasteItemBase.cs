namespace EcoSanitizeApp.Models
{
    public abstract class WasteItemBase
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public double WeightKg { get; set; }
        public string OriginZone { get; set; }

        protected WasteItemBase(int id, string itemName, double weightKg, string originZone)
        {
            Id = id;
            ItemName = itemName;
            WeightKg = weightKg;
            OriginZone = originZone;
        }

        public abstract string GetBinColor();
        public abstract string GetHandlingInstructions();
        public abstract string GetTargetFacility();
        public abstract int GetHazardRating();
    }
}
