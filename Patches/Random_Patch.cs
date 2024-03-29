﻿using HarmonyLib;
using System;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    internal static class Random_Patch
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

        [HarmonyPatch(typeof(UnityEngine.Random), "value", MethodType.Getter)]
        [HarmonyPostfix]
        static void Get_Value_Postfix(ref float __result)
        {
            if (_shouldLogValue)
                Main.LogInfo($"{new string('\t', _indentLevel)}Random.value = {__result}");
        }

        [HarmonyPatch(typeof(UnityEngine.Random), "Range", new Type[] { typeof(int), typeof(int) })]
        [HarmonyPostfix]
        static void Range_Postfix(ref int __result, int minInclusive, int maxExclusive)
        {
            if (_shouldLogValue)
                Main.LogInfo($"{new string('\t', _indentLevel)}Random.Range({minInclusive}, {maxExclusive}) = {__result}");
        }
    }
}
