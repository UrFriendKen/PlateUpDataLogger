using HarmonyLib;
using Kitchen.Layouts;
using System;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    static class LayoutFailureException_Patch
    {
        [HarmonyPatch(typeof(LayoutFailureException), MethodType.Constructor, new Type[] { typeof(string) })]
        [HarmonyPrefix]
        static void Ctor_Prefix(string message)
        {
            Main.LogError(message);
        }
    }
}
