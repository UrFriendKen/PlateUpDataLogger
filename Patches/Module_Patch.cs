using HarmonyLib;
using Kitchen.Layouts;
using Kitchen.Layouts.Modules;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    static class Module_Patch
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

        [HarmonyPatch(typeof(Module<LayoutBlueprint>), "PerformUpdate")]
        [HarmonyPrefix]
        static void PerformUpdate_Prefix(ref Module<LayoutBlueprint> __instance)
        {
            if (!_shouldLogValue)
                return;

            Main.LogInfo($"{new string('\t', _indentLevel)}{__instance.GetType()}");
        }
    }
}
