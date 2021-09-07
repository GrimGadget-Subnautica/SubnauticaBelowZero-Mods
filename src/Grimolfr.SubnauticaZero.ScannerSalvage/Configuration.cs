using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace Grimolfr.SubnauticaZero.ScannerSalvage
{
    [Menu(
        Main.ModName,
        MemberProcessing = MenuAttribute.Members.Explicit,
        LoadOn = MenuAttribute.LoadEvents.MenuOpened,
        SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    internal class Configuration
        : ConfigFile
    {
        [Toggle(Label = "Enable", Tooltip = "Check to enable this mod.", Order = 0)]
        public bool IsEnabled = true;
    }
}
