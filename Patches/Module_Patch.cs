using HarmonyLib;
using Kitchen.Layouts;
using Kitchen.Layouts.Modules;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    static class Module_Patch
    {
        [HarmonyPatch(typeof(Module<LayoutBlueprint>), "PerformUpdate")]
        [HarmonyPrefix]
        static void PerformUpdate_Prefix(ref Module<LayoutBlueprint> __instance)
        {
            Main.LogInfo($"\t\t{__instance.GetType()}");
        }
    }
}
