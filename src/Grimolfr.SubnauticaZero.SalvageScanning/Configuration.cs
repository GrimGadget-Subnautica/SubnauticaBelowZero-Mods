using System.Collections.Generic;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace Grimolfr.SubnauticaZero.SalvageScanning
{
    [Menu(
        Main.ModName,
        MemberProcessing = MenuAttribute.Members.Implicit,
        LoadOn = MenuAttribute.LoadEvents.MenuOpened)]
    internal class Configuration
        : ConfigFile
    {
        [Toggle(Label = "Verbose Debug Logging", Tooltip = "Log extra data when QMods debug logging is enabled.", Order = 1)]
        public bool EnableVerboseLogging = false;

        [Choice(
            Label = "Salvage Operation Mode",
            Tooltip = "Selects the operational mode of the salvage functionality.  "
                + "'Any' will return any possible salvage result from the Basic or Advanced setting.  "
                + "'Basic' will only return non-biological raw materials and basic materials."
                + "'Advanced' will only return advanced resources and electronics, with a possibility of receiving nothing at all.  ",
            Order = 101)]
        public OperationMode OperationMode = OperationMode.Any;

        public readonly IDictionary<string, double> SalvageProbabilities =
            new Dictionary<string, double>
            {
                {"titanium", 2.5},
                {"copper", 0.5},

                {"Lubricant", 0.125},
                {"HydrochloricAcid", 0.125},
                {"Benzene", 0.125},
                {"Polyaniline", 0.125},

                {"PrecursorIonCrystal", 0.0625},
                {"PrecursorIonBattery", 0},
                {"PrecursorIonPowerCell", 0},
            };
    }
}
