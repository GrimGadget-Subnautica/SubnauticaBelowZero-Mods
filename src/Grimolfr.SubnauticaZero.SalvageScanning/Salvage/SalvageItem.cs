using Newtonsoft.Json;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Salvage
{
    internal class SalvageableItem
    {
        public SalvageableItem(Material material, SalvageableItem parent)
        {
            TechType = material.TechType;
            Parent = parent;
        }

        [JsonIgnore]
        public SalvageableItem Parent { get; }

        public TechType TechType { get; }

        public double Weight { get; set; } = 0.0;
    }
}
