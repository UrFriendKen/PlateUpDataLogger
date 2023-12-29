using HarmonyLib;
using Kitchen.Layouts;
using System.Collections.Generic;
using System.Linq;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    internal static class LayoutBlueprint_Patch
    {
        static bool _shouldLogValue = false;

        static int _indentLevel = 0;

        public static void Start(int indentLevel = 0)
        {
            _shouldLogValue = true;
            _indentLevel = indentLevel;
        }

        public static void Reset()
        {
            _shouldLogValue = false;
        }

        [HarmonyPatch(typeof(LayoutBlueprint), "SetRoom")]
        [HarmonyPrefix]
        static void SetRoom_Prefix(ref Dictionary<LayoutPosition, Room> ___Tiles, Room current)
        {
            if (!_shouldLogValue)
                return;
            Main.LogInfo($"{new string('\t', _indentLevel)}LayoutBlueprint.SetRoom(Room, Room)");
            LayoutPosition[] array = ___Tiles.Keys.ToArray();

            string innerIndent = new string('\t', _indentLevel + 1);
            foreach (LayoutPosition key in array)
            {
                if (___Tiles[key].ID == current.ID)
                {
                    Main.LogInfo($"{innerIndent}LayoutPosition = ({key.x}, {key.y})");
                }
            }
        }
    }
}
