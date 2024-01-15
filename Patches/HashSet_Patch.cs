using HarmonyLib;
using Kitchen.Layouts;
using System.Collections.Generic;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    internal static class HashSet_Patch
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

        [HarmonyPatch(typeof(HashSet<Room>), "Add")]
        [HarmonyPrefix]
        static void Add_Prefix()
        {
            if (!_shouldLogValue)
                return;

            Main.LogInfo($"{new string('\t', _indentLevel)}HashSet<Room>.Add(Room value)");
            _shouldLogHashCode = true;
        }

        [HarmonyPatch(typeof(HashSet<Room>), "Add")]
        [HarmonyPostfix]
        static void Add_Postfix(ref bool __result)
        {
            if (_shouldLogHashCode)
            {
                string indent = new string('\t', _indentLevel + 1);
                Main.LogInfo($"{indent}IsAdded = {__result}");
            }

            _shouldLogHashCode = false;
        }

        private static bool _shouldLogHashCode = false;

        [HarmonyPatch(typeof(HashSet<Room>), "InternalGetHashCode")]
        [HarmonyPostfix]
        static void InternalGetHashCode_Postfix(Room item, ref int __result)
        {
            if (!_shouldLogHashCode)
                return;

            string indent = new string('\t', _indentLevel + 1);
            Main.LogInfo($"{indent}Room = {item.ID} ({item.Type})");
            Main.LogInfo($"{indent}HashCode = {__result}");
        }
    }
}
