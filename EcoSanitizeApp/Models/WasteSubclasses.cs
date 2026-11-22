namespace EcoSanitizeApp.Models
{
    public class OrganicWaste : WasteItemBase
    {
        public OrganicWaste(int id, string itemName, double weightKg, string originZone)
            : base(id, itemName, weightKg, originZone)
        {
        }

        public override string GetBinColor() => "Green (Wet Waste)";
        public override string GetHandlingInstructions() => "Keep covered to control odors and moisture. Do not mix with non-biodegradables.";
        public override string GetTargetFacility() => "Municipal Composting Plant / Biogas Digester";
        public override int GetHazardRating() => 1;
    }

    public class RecyclableWaste : WasteItemBase
    {
        public RecyclableWaste(int id, string itemName, double weightKg, string originZone)
            : base(id, itemName, weightKg, originZone)
        {
        }

        public override string GetBinColor() => "Blue (Dry Recyclable)";
        public override string GetHandlingInstructions() => "Rinse containers, flatten paper and cardboard, keep dry.";
        public override string GetTargetFacility() => "Material Recovery Facility (MRF)";
        public override int GetHazardRating() => 2;
    }

    public class HazardousWaste : WasteItemBase
    {
        public HazardousWaste(int id, string itemName, double weightKg, string originZone)
            : base(id, itemName, weightKg, originZone)
        {
        }

        public override string GetBinColor() => "Red (Hazardous)";
        public override string GetHandlingInstructions() => "Store in sealed chemical containers. Protective gloves and eye gear required.";
        public override string GetTargetFacility() => "Hazardous Waste Treatment and Disposal Facility";
        public override int GetHazardRating() => 8;
    }

    public class SanitaryWaste : WasteItemBase
    {
        public SanitaryWaste(int id, string itemName, double weightKg, string originZone)
            : base(id, itemName, weightKg, originZone)
        {
        }

        public override string GetBinColor() => "Yellow (Sanitary)";
        public override string GetHandlingInstructions() => "Double-bag in marked biohazard bags. Keep segregated from municipal streams.";
        public override string GetTargetFacility() => "High-Temperature Medical Incinerator";
        public override int GetHazardRating() => 7;
    }
}
