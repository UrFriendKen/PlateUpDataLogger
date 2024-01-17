using HarmonyLib;
using Kitchen;
using KitchenData;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    static class CreateLayoutHelper_Patch
    {
        [HarmonyPatch(typeof(CreateLayoutHelper), nameof(CreateLayoutHelper.ConstructLayout))]
        [HarmonyPrefix]
        static void ConstructLayout_Prefix(LayoutProfile profile, RestaurantSetting setting, int seed)
        {
            Main.LogInfo($"LayoutProfile: {profile.name}");
            //Main.LogInfo($"RestaurantSetting: {setting.name}");
            Main.LogInfo($"Generated random number: {seed}");
        }
    }
}
