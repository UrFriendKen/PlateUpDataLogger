using HarmonyLib;
using Kitchen.Layouts;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    static class LayoutGraph_Patch
    {
        [HarmonyPatch(typeof(LayoutGraph), nameof(LayoutGraph.Build))]
        [HarmonyPrefix]
        static void Build_Prefix(int seed)
        {
            if (seed == 0) return;
            Main.LogInfo($"\tBuilding: {seed}");
        }
    }
}
