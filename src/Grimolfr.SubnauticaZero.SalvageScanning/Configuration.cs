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
        [Choice(
            Label = "Salvage Operation Mode",
            Tooltip = "Selects the operational mode of the salvage functionality.  "
                + "'Any' will return any possible salvage result from the Basic or Advanced setting.  "
                + "'Basic' will only return non-organic raw materials and basic materials."
                + "'Advanced' will only return advanced resources and electronics, but with a possibility of receiving nothing at all.  ")]
        public OperationMode OperationMode = OperationMode.Basic;

        public IDictionary<string, double> SalvageProbabilities =
            new Dictionary<string, double>
            {
                {"PrecursorIonCrystal", 0.125},
                {"PrecursorIonBattery", 0.125},
                {"PrecursorIonPowerCell", 0.125},
            };

    }
}
