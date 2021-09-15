using System.Collections.Generic;
using System.Linq;
using Grimware;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Salvage
{
    internal static class Salvageable
    {
        private static readonly IDictionary<TechType, double> Minerals =
            new Dictionary<TechType, double>
            {
                {TechType.Titanium, 1},
                {TechType.Copper, 0.75},
                {TechType.Quartz, 0.75},
                {TechType.Salt, 0.75},
                {TechType.Lead, 0.6875},
                {TechType.Silver, 0.5625},
                {TechType.Sulphur, 0.5625},
                {TechType.Gold, 0.5},
                {TechType.Lithium, 0.5},
                {TechType.AluminumOxide, 0.375},
                {TechType.Diamond, 0.3125},
                {TechType.UraniniteCrystal, 0.25},
                {TechType.Magnetite, 0.125},
                {TechType.Nickel, 0.0625},
                {TechType.Kyanite, 0.03125},
            };

        private static readonly IDictionary<TechType, double> BasicMaterials =
            new Dictionary<TechType, double>
            {
                {TechType.FiberMesh, 0.75},
                {TechType.Lubricant, 0.75},
                {TechType.Silicone, 0.75},
                {TechType.Glass, 0.6875},
                {TechType.EnameledGlass, 0.5},
                {TechType.TitaniumIngot, 0.5},
                {TechType.PlasteelIngot, 0.375},
            };

        private static readonly IDictionary<TechType, double> AdvancedMaterials =
            new Dictionary<TechType, double>
            {
                {TechType.Aerogel, 0.375},
                {TechType.HydrochloricAcid, 0.375},
                {TechType.AramidFibers, 0.25},
                {TechType.Benzene, 0.25},
                {TechType.Polyaniline, 0.125},
                {TechType.PrecursorIonCrystal, 0},
            };

        private static readonly IDictionary<TechType, double> Electronics =
            new Dictionary<TechType, double>
            {
                {TechType.CopperWire, 0.6875},
                {TechType.Battery, 0.5625},
                {TechType.WiringKit, 0.5625},
                {TechType.PowerCell, 0.5},
                {TechType.ComputerChip, 0.5},
                {TechType.AdvancedWiringKit, 0.4375},
                {TechType.ReactorRod, 0.015625},
                {TechType.PrecursorIonBattery, 0},
                {TechType.PrecursorIonPowerCell, 0},
            };

        public static IDictionary<TechType, double> Weights =>
            ConfiguredOperationModeSalvageTypes()
                .GroupJoin(
                    ConfigWeights,
                    w => w.Key,
                    cw => cw.Key,
                    (w, cList) =>
                        new
                        {
                            TechType = w.Key,
                            Weight =
                                (!cList.Any() ? (double?)null : cList.Min(c => c.Value))
                                ?? w.Value
                        })
                .Where(a => a.TechType != TechType.None && a.Weight is > 0.0 and <= 1.0)
                .ToDictionary(a => a.TechType, a => a.Weight);

        private static IDictionary<TechType, double> ConfigWeights =>
            Main.Config.SalvageProbabilities
                .Select(kvp => new {TechType = kvp.Key.ToEnum<TechType>(), Weight = kvp.Value})
                .Where(a => a.TechType != null && a.TechType != TechType.None & a.Weight is > 0.0 and <= 1.0)
                .ToDictionary(a => a.TechType.Value, a => a.Weight);

        private static IEnumerable<KeyValuePair<TechType, double>> ConfiguredOperationModeSalvageTypes() =>
            Main.Config.OperationMode switch
            {
                OperationMode.Any => Minerals.Concat(BasicMaterials).Concat(AdvancedMaterials).Concat(Electronics),
                OperationMode.Basic => Minerals.Concat(BasicMaterials),
                OperationMode.Advanced => AdvancedMaterials.Concat(Electronics),
                _ => Minerals.Concat(BasicMaterials).Concat(AdvancedMaterials).Concat(Electronics)
            };
    }
}
