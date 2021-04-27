﻿using HarmonyLib;
using VersionShower = BOCOFLHKCOJ;

namespace DictatorRole
{
    [HarmonyPatch(typeof(VersionShower), "Start")]
    public static class VersionShowerPatch
    {
        public static void Postfix(VersionShower __instance)
        {
            AELDHKGBIFD text = __instance.text;
            text.Text += "\nDictator Role 1.0.1 by DillyzThe1";
        }
    }
}