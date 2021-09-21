using System;
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
                {TechType.Titanium, 1.0},
                {TechType.Copper, 1.0},
                {TechType.Quartz, 1.0},
                {TechType.Salt, 1.0},
                {TechType.Lead, 1.0},
                {TechType.Silver, 1.0},
                {TechType.Sulphur, 1.0},
                {TechType.Gold, 1.0},
                {TechType.Lithium, 1.0},
                {TechType.AluminumOxide, 1.0},
                {TechType.Diamond, 1.0},
                {TechType.UraniniteCrystal, 1.0},
                {TechType.Magnetite, 1.0},
                {TechType.Nickel, 1.0},
                {TechType.Kyanite, 1.0},
            };

        private static readonly IDictionary<TechType, double> BasicMaterials =
            new Dictionary<TechType, double>
            {
                {TechType.FiberMesh, 1.0},
                {TechType.Lubricant, 1.0},
                {TechType.Silicone, 1.0},
                {TechType.Glass, 1.0},
                {TechType.EnameledGlass, 1.0},
                {TechType.TitaniumIngot, 1.0},
                {TechType.PlasteelIngot, 1.0},
            };

        private static readonly IDictionary<TechType, double> AdvancedMaterials =
            new Dictionary<TechType, double>
            {
                {TechType.Aerogel, 1.0},
                {TechType.AramidFibers, 1.0},
                {TechType.HydrochloricAcid, 1.0},
                {TechType.Benzene, 1.0},
                {TechType.Polyaniline, 1.0},
                {TechType.PrecursorIonCrystal, 1.0},
            };

        private static readonly IDictionary<TechType, double> Electronics =
            new Dictionary<TechType, double>
            {
                {TechType.CopperWire, 1.0},
                {TechType.Battery, 1.0},
                {TechType.WiringKit, 1.0},
                {TechType.PowerCell, 1.0},
                {TechType.ComputerChip, 1.0},
                {TechType.AdvancedWiringKit, 1.0},
                {TechType.ReactorRod, 1.0},
                {TechType.PrecursorIonBattery, 1.0},
                {TechType.PrecursorIonPowerCell, 1.0},
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
                            Weight = cList.Any() ? cList.Min(c => c.Value) : w.Value
                        })
                .Where(a => a.TechType != TechType.None && a.Weight is > 0.0 and <= 10.0)
                .ToDictionary(a => a.TechType, a => a.Weight);

        private static IDictionary<TechType, double> ConfigWeights =>
            Main.Config.SalvageProbabilities
                .GroupBy(
                    x => x.Key,
                    (tt, x) => new {TechType = tt.ToEnum<TechType>(true) ?? TechType.None, Weight = x.Min(w => w.Value)},
                    StringComparer.OrdinalIgnoreCase)
                .ToDictionary(x => x.TechType, x => x.Weight);

        private static IEnumerable<KeyValuePair<TechType, double>> ConfiguredOperationModeSalvageTypes() =>
            Main.Config.OperationMode switch
            {
                OperationMode.Any => Minerals.Concat(BasicMaterials).Concat(AdvancedMaterials).Concat(Electronics),
                OperationMode.Basic => Minerals.Concat(BasicMaterials),
                OperationMode.Advanced => AdvancedMaterials.Concat(Electronics),
                _ => Minerals.Concat(BasicMaterials)
            };
    }
}
