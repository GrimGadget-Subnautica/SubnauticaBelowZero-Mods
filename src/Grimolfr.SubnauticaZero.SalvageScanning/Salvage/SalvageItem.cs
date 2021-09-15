using Newtonsoft.Json;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Salvage
{
    internal class SalvageableItem
    {
        public SalvageableItem(Material material, SalvageableItem parent)
        {
            TechType = material.TechType;
            Parent = parent;

            Weight = 1.0 / material.CountAllRawMaterials();
        }

        [JsonIgnore]
        public SalvageableItem Parent { get; }

        public TechType TechType { get; }

        public double Weight { get; set; }
    }
}
