using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnhollowerBaseLib;
using UnityEngine;
using Reactor.Extensions;
using DillyzRolesAPI.Roles;
using Reactor.Networking;
using TMPro;

namespace DictatorReworked
{
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CalculateVotes))]
    class VotingPatch
    {
        public static bool isDictating = false;
        public static int dictatesLeft = 2;
        public static bool Prefix(MeetingHud __instance, ref Il2CppStructArray<byte> __result)
        {
            byte[] array = new byte[11];
            foreach (PlayerVoteArea player in __instance.playerStates)
            {
                if (player.didVote)
                {
                    int num = player.votedFor + 1;
                    if (num >= 0 && num < array.Length)
                    {
                        byte[] array2 = array;
                        int num2 = num;
                        byte votePower = (DictatorReworked.dictator.containedPlayerIds.Contains((byte)player.TargetPlayerId) && isDictating) ? (byte)Mathf.RoundToInt(DictatorReworked.dictatorVotePower.value) : (byte)1;
                        array2[num2] += votePower;
                    }
                }
            }
            __result = array;
            return false;
        }
    }
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    class meetstartpatch
    {
        public static void Prefix(MeetingHud __instance)
        {
            foreach (PlayerVoteArea playervotearea in __instance.playerStates)
            {
                GameObject dictateButton = new GameObject();
                dictateButton = GameObject.Instantiate(playervotearea.ConfirmButton.gameObject);
                dictateButton.transform.position = playervotearea.ConfirmButton.transform.position + new Vector3(-1f, 0f, 0f);
                dictateButton.transform.localPosition += new Vector3(0.34f, 0f, 0f);
                dictateButton.transform.parent = playervotearea.Buttons.transform;
                AssetLoader.BundleLoad();
                dictateButton.GetComponent<SpriteRenderer>().sprite = AssetLoader.dictatorsprite;
                dictateButton.name = "DictateButton";
                if (!DictatorReworked.dictator.containedPlayerIds.Contains(PlayerControl.LocalPlayer.PlayerId) || VotingPatch.dictatesLeft < 1)
                    dictateButton.gameObject.SetActive(false);
                else
                    dictateButton.gameObject.SetActive(true); // i broke everything AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            }
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
    class meetuppatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            if (DictatorReworked.dictator.containedPlayerIds.Contains(PlayerControl.LocalPlayer.PlayerId))
                if (VotingPatch.dictatesLeft >= 1)
                    __instance.TitleText.text = "You can control votes <#3AA3D9>" + VotingPatch.dictatesLeft.ToString() + (VotingPatch.dictatesLeft > 1 ? "</color> more times." : "</color> more time.");
                else
                    __instance.TitleText.text = "You <#FF0000>cannot</color> control votes.";
        }
    }

    [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Awake))]
    class resetStuff
    {
        public static void Postfix()
        {
            VotingPatch.dictatesLeft = Mathf.RoundToInt(DictatorReworked.dictatorVoteTimes.value);
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetInfected))]
    class setInfectedPatch
    {
        public static void Postfix()
        {
            DictatorReworked.dictator.isEnabled = DictatorReworked.dictatorEnabled.value;
        }
    }

    /*[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetInfected))]
    class rpcSetInfectedPatch
    {
        public static void Prefix()
        {
            DictatorReworked.dictator.roleCount = Mathf.RoundToInt(DictatorReworked.dictatorCount.value);
        }
    }*/

    [HarmonyPatch(typeof(PassiveButton), nameof(PassiveButton.ReceiveClickDown))]
    class clickbuttonpatch
    {
        public static void Postfix(PassiveButton __instance)
        {
            if (DictatorReworked.dictator.containedPlayerIds.Contains(PlayerControl.LocalPlayer.PlayerId) && ShipStatus.Instance != null && __instance.gameObject != null)
                if (asdfghjkl(__instance))
                    switch (__instance.gameObject.name)
                    {
                        case "DictateButton":
                            VotingPatch.isDictating = true;
                            Rpc<SetDictatedVote>.Instance.Send((true, 0));
                            VotingPatch.dictatesLeft--;
                            break;
                        case "ConfirmButton":
                            VotingPatch.isDictating = false;
                            Rpc<SetDictatedVote>.Instance.Send((false, 0));
                            break;
                        case "CancelButton":
                            VotingPatch.isDictating = false;
                            Rpc<SetDictatedVote>.Instance.Send((false, 0));
                            break;
                        case "button_skipVoting":
                            VotingPatch.isDictating = false;
                            Rpc<SetDictatedVote>.Instance.Send((false, 0));
                            break;
                    }
        }
        public static bool asdfghjkl(PassiveButton __instance)
        {
            foreach (string str in new List<string> { "DictateButton", "ConfirmButton", "CancelButton", "button_skipVoting" })
                if (__instance.gameObject.name == str)
                    return true;
            return false;
        }
    }

    public static class AssetLoader
    {
        public static AssetBundle bundle;

        public static Sprite dictatorsprite;
        public static void BundleLoad()
        {
            if (bundle == null)
            {
                byte[] array = Properties.Resources.dictate;
                bundle = AssetBundle.LoadFromMemory(array);
            }
            dictatorsprite = bundle.LoadAsset<Sprite>("dictate").DontUnload();
        }
    }
}
