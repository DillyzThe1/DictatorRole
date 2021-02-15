using HarmonyLib;
using UnityEngine;
using IntroCutscene_CoBegin_d__10 = PENEIDJGGAF.CKACLKCOJFO;
using PlayerControl = FFGALNAPKCD;

namespace DictatorRole
{
    [HarmonyPatch]
    public static class IntroCutscenePatch
    {
        [HarmonyPatch(typeof(IntroCutscene_CoBegin_d__10), "MoveNext")]
        public static void Postfix(IntroCutscene_CoBegin_d__10 __instance)
        {
            if (PlayerControlPatch.IsDictator(PlayerControl.LocalPlayer))
            {
                __instance.field_Public_PENEIDJGGAF_0.Title.Text = "Dictator";
                __instance.field_Public_PENEIDJGGAF_0.Title.Color = new Color(0.79f, 0.47f, 0.15f, 1f);
                __instance.field_Public_PENEIDJGGAF_0.ImpostorText.Text = "Your vote is worth " + CustomGameOptions.DictatorVotePoints + " whole votes";
                __instance.field_Public_PENEIDJGGAF_0.BackgroundBar.material.color = new Color(0.79f, 0.47f, 0.15f, 1f);
            }
        }
    }
}
