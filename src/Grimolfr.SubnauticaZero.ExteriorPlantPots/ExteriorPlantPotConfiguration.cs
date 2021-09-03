using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;

namespace Grimolfr.SubnauticaZero.ExteriorPlantPots
{
    [Menu("Exterior Plant Pots", LoadOn = MenuAttribute.LoadEvents.MenuOpened, SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    internal class ExteriorPlantPotConfiguration
        : ConfigFile
    {
        private const float _MaxRgbColorValue = 255f;

        [Slider(
            "Exterior Pot Tint - Red", 0, 255, DefaultValue = 127,
            Tooltip = "Red component of custom tint RGB color.",
            Order = 101)]
        public byte Red = 127;

        [Slider(
            "Exterior Pot Tint - Green", 0, 255, DefaultValue = 255,
            Tooltip = "Green component of custom tint RGB color.",
            Order = 102)]
        public byte Green = 255;

        [Slider(
            "Exterior Pot Tint - Blue", 0, 255, DefaultValue = 212,
            Tooltip = "Blue component of custom tint RGB color.",
            Order = 103)]
        public byte Blue = 212;

        [IgnoreMember]
        internal Color CustomTint =>
            new Color(ConvertColorComponentValue(Red), ConvertColorComponentValue(Green), ConvertColorComponentValue(Blue));

        [IgnoreMember]
        private static float ConvertColorComponentValue(byte componentValue)
        {
            return componentValue / _MaxRgbColorValue;
        }
    }
}
