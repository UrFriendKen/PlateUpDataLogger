using HarmonyLib;
using Kitchen.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

namespace KitchenDataLogger.Patches
{
    [HarmonyPatch]
    internal static class LayoutBlueprint_Patch
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

        [HarmonyPatch(typeof(LayoutBlueprint), "AdjacentRooms", new Type[] { typeof(Room) })]
        [HarmonyPrefix]
        static void AdjacentRooms_Prefix(Room start)
        {
            if (!_shouldLogValue)
                return;

            string indent = new string('\t', _indentLevel);
            string infoIndent = new string('\t', _indentLevel + 1);
            Main.LogInfo($"{indent}LayoutBlueprint.AdjacentRooms(Room start)");
            Main.LogInfo($"{infoIndent}start.ID = {start.ID}");
            Main.LogInfo($"{infoIndent}start.Type = {start.Type}");
        }

        [HarmonyPatch(typeof(LayoutBlueprint), "AdjacentRooms", new Type[] { typeof(Room) })]
        [HarmonyPostfix]
        static void AdjacentRooms_Postfix(Room start, ref LayoutBlueprint __instance, ref HashSet<Room> __result, ref Dictionary<LayoutPosition, Room> ___Tiles)
        {
            if (!_shouldLogValue)
                return;

            string indent = new string('\t', _indentLevel);
            string infoIndent = new string('\t', _indentLevel + 1);
            string loopIndent = new string('\t', _indentLevel + 2);
            
            Main.LogInfo($"{infoIndent}Layout Tile Positions Loop Order: {String.Join(", ", __instance.Tiles.Select(tile => $"({tile.Key.x}, {tile.Key.y})"))}");

            Main.LogInfo($"{infoIndent}AdjacentRooms returned hashset to enumerable");
            int hashSetToEnumerableIndex = 0;
            foreach (Room room in __result)
            {
                Main.LogInfo($"{loopIndent}{hashSetToEnumerableIndex++}: {room.ID} ({room.Type})");
            }
            if (hashSetToEnumerableIndex == 0)
            {
                Main.LogInfo($"{loopIndent}Empty");
            }


            Main.LogInfo($"{infoIndent}Filter unassigned rooms using LINQ");
            Room[] array = (from e in __result
                            where e.Type == RoomType.Unassigned
                            select e).ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Main.LogInfo($"{loopIndent}{i}: {array[i].ID} ({array[i].Type})");
            }
            if (array.Length == 0)
            {
                Main.LogInfo($"{loopIndent}Empty");
            }
        }


        //[HarmonyPatch(typeof(LayoutBlueprint), "SetRoom")]
        //[HarmonyPrefix]
        //static void SetRoom_Prefix(ref Dictionary<LayoutPosition, Room> ___Tiles, Room current, Room next)
        //{
        //    if (!_shouldLogValue)
        //        return;

        //    string indent = new string('\t', _indentLevel);
        //    string infoIndent = new string('\t', _indentLevel + 1);
        //    string loopIndent = new string('\t', _indentLevel + 2);
        //    Main.LogInfo($"{indent}LayoutBlueprint.SetRoom(Room, Room)");
        //    LayoutPosition[] array = ___Tiles.Keys.ToArray();

        //    Main.LogInfo($"{infoIndent}current.ID = {current.ID}");
        //    Main.LogInfo($"{infoIndent}current.Type = {current.ID}");
        //    Main.LogInfo($"{infoIndent}next.ID = {next.ID}");
        //    Main.LogInfo($"{infoIndent}next.Type = {next.ID}");

        //    Main.LogInfo($"{infoIndent}Positions Changed");
        //    foreach (LayoutPosition key in array)
        //    {
        //        if (___Tiles[key].ID == current.ID)
        //        {
        //            Main.LogInfo($"{loopIndent}LayoutPosition = ({key.x}, {key.y})");
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(LayoutBlueprint), "SetRoom")]
        //[HarmonyPostfix]
        //static void SetRoom_Postfix(ref Dictionary<LayoutPosition, Room> ___Tiles, Room next)
        //{
        //    if (!_shouldLogValue)
        //        return;

        //    string infoIndent = new string('\t', _indentLevel + 1);
        //    string loopIndent = new string('\t', _indentLevel + 2);

        //    int width = ___Tiles.Select(tile => tile.Key.x).Max() + 1;
        //    var orderedRooms = ___Tiles.OrderByDescending(tile => tile.Key.y).ThenBy(tile => tile.Key.x).Select(tile => tile.Value).ToList();
        //    Main.LogInfo($"{infoIndent}Room result");
        //    for (int j = 0; j < ___Tiles.Count / (float)width; j++)
        //    {
        //        string row = string.Empty;
        //        for (int i = 0; i < width; i++)
        //        {
        //            row += orderedRooms[i + j * width].ID == next.ID ? " ■" : " □";
        //        }
        //        Main.LogInfo($"{loopIndent}{row}");
        //    }
        //}
    }
}
