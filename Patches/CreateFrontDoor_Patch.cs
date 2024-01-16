using HarmonyLib;
using Kitchen.Layouts;
using Kitchen.Layouts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    static class CreateFrontDoor_Patch
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

        [HarmonyPatch(typeof(CreateFrontDoor), "ActOn")]
        [HarmonyPrefix]
        static void ActOn_Patch(LayoutBlueprint blueprint, ref bool ___ForceFirstHalf, ref RoomType ___Type)
        {
            if (!_shouldLogValue)
                return;

            Bounds bounds = blueprint.GetBounds();
            float min_y = bounds.min.y;
            RoomType type = ___Type;
            bool forceFirstHalf = ___ForceFirstHalf;
            List<KeyValuePair<LayoutPosition, Room>> list = blueprint.Tiles
                .Where((KeyValuePair<LayoutPosition, Room> t) => Math.Abs((float)t.Key.y - min_y) < 0.05f && t.Value.Type == type && (!forceFirstHalf || (float)t.Key.x < bounds.center.x))
                .ToList();
            Main.LogInfo($"{new String('\t', _indentLevel)}Candidate Front Door Tile Positions: {String.Join(", ", list.Select(tile => $"({tile.Key.x}, {tile.Key.y})"))}");
        }
    }
}
