using System.Collections.Generic;
using System.Linq;
using Grimware;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Salvage
{
    internal static class Salvageable
    {
        public static IEnumerable<(TechType TechType, int Rarity)> AllTypes =>
            Minerals
                .Concat(BasicMaterials)
                .Concat(AdvancedMaterials)
                .Concat(ElectronicsMaterials)
                .Distinct(EqualityComparer.Create<(TechType TechType, int Rarity), TechType>(x => x.TechType));

        public static readonly IEnumerable<(TechType TechType, int Rarity)> Minerals =
            new[]
            {
                (TechType.Titanium, 1),
                (TechType.Copper, 2),
                (TechType.Quartz, 2),
                (TechType.Salt, 4),
                (TechType.Silver, 8),
                (TechType.Lead, 16),
                (TechType.Sulphur, 32),
                (TechType.Gold, 32),
                (TechType.Lithium, 64),
                (TechType.AluminumOxide, 128),
                (TechType.Diamond, 128),
                (TechType.UraniniteCrystal, 128),
                (TechType.Magnetite, 256),
                (TechType.Nickel, 256),
                (TechType.Kyanite, 512),
            };

        public static readonly IEnumerable<(TechType TechType, int Rarity)> BasicMaterials =
            new[]
            {
                (TechType.FiberMesh, 2),
                (TechType.Silicone, 2),
                (TechType.Lubricant, 4),
                (TechType.Glass, 8),
                (TechType.TitaniumIngot, 16),
                (TechType.EnameledGlass, 32),
                (TechType.PlasteelIngot, 128),
            };

        public static readonly IEnumerable<(TechType TechType, int Rarity)> AdvancedMaterials =
            new[]
            {
                (TechType.Aerogel, 64),
                (TechType.AramidFibers, 128),
                (TechType.HydrochloricAcid, 128),
                (TechType.Benzene, 256),
                (TechType.Polyaniline, 256),
                (TechType.PrecursorIonCrystal, 512),
            };

        public static readonly IEnumerable<(TechType TechType, int Rarity)> ElectronicsMaterials =
            new[]
            {
                (TechType.CopperWire, 4),
                (TechType.Battery, 4),
                (TechType.WiringKit, 16),
                (TechType.PowerCell, 16),
                (TechType.ComputerChip, 64),
                (TechType.AdvancedWiringKit, 256),
                (TechType.PrecursorIonBattery, 1024),
                (TechType.PrecursorIonPowerCell, 2048),
                (TechType.ReactorRod, 2048),
            };
    }
}
