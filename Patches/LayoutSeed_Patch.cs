using HarmonyLib;
using Kitchen;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    static class LayoutSeed_Patch
    {
        [HarmonyPatch(typeof(LayoutSeed), nameof(LayoutSeed.GenerateMap))]
        [HarmonyPrefix]
        static void GenerateMap_Prefix(LayoutSeed __instance)
        {
            Main.LogInfo($"{__instance.FixedSeed.StrValue}: {__instance.FixedSeed.IntValue}");
        }
    }
}
