using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace Grimolfr.SubnauticaZero.ExteriorPlantPots
{
    [Menu(Main.ModName,
        MemberProcessing = MenuAttribute.Members.Explicit,
        LoadOn = MenuAttribute.LoadEvents.MenuOpened,
        SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    internal class Configuration
        : ConfigFile
    {
        [Toggle(
            Label = "Require Exterior Growbed",
            Tooltip = "If checked, exterior pots become available along with the exterior growbed.  "
                + "Otherwise they become available along with their base counterparts.",
            Order = 100)]
        public bool RequireExteriorGrowbed = false;
    }
}
