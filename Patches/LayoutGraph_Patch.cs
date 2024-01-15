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
            Module_Patch.Start(2);
            //Random_Patch.Start(3);
            LayoutBlueprint_Patch.Start(4);
            HashSet_Patch.Start(5);
        }

        [HarmonyPatch(typeof(LayoutGraph), nameof(LayoutGraph.Build))]
        [HarmonyPostfix]
        static void Build_Postfix()
        {
            Module_Patch.Reset();
            //Random_Patch.Reset();
            LayoutBlueprint_Patch.Reset();
            HashSet_Patch.Reset();
        }
    }
}
