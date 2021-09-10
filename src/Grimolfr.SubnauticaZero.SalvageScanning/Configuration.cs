using System.Collections.Generic;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace Grimolfr.SubnauticaZero.SalvageScanning
{
    [Menu(
        Main.ModName,
        MemberProcessing = MenuAttribute.Members.Implicit,
        LoadOn = MenuAttribute.LoadEvents.MenuOpened,
        SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    internal class Configuration
        : ConfigFile
    {
        public IDictionary<string, float> Weights =
            new Dictionary<string, float>
            {
            };
    }
}
