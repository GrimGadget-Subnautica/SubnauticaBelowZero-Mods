using System;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Salvage
{
    public struct SalvageableItem
    {
        public TechType TechType { get; set; }
        public int Rarity { get; set; }
        public int Amount { get; set; }
        public int MaxDepth { get; set; }

        public double Weight => 1.0 / (Rarity * 2^Math.Max(4 - MaxDepth, 0));
    }
}
