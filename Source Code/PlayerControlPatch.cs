using System.Collections.Generic;
using HarmonyLib;
using Hazel;
using UnhollowerBaseLib;
using PlayerControl = FFGALNAPKCD;
using GameDataPlayerInfo = EGLJNOMOGNP.DCJMABDDJCF;
using AmongUsClient = FMLLKEACGIO;
using GameOptionsData = KMOGFLPJLLK;
using System;

namespace DictatorRole
{
    [HarmonyPatch]
    public static class PlayerControlPatch
    {
        public static PlayerControl Dictator;

        [HarmonyPatch(typeof(PlayerControl), "HandleRpc")]
        public static void Postfix(byte HKHMBLJFLMC, MessageReader ALMCIJKELCP)
        {
            switch (HKHMBLJFLMC)
            {
                case 41:
                    byte DictatorId = ALMCIJKELCP.ReadByte();
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                    {
                        if (DictatorId == player.PlayerId)
                        {
                            Dictator = player;
                        }
                    }
                    break;
            }
        }

        public static bool IsDictator(PlayerControl player)
        {
            if (Dictator == null) return false;
            return player.PlayerId == PlayerControlPatch.Dictator.PlayerId;
        }
        public static List<FFGALNAPKCD> getCrewMates(Il2CppReferenceArray<EGLJNOMOGNP.DCJMABDDJCF> infection)
        {
            List<FFGALNAPKCD> CrewmateIds = new List<FFGALNAPKCD>();
            foreach (FFGALNAPKCD player in FFGALNAPKCD.AllPlayerControls)
            {
                bool isInfected = false;
                foreach (EGLJNOMOGNP.DCJMABDDJCF infected in infection)
                {
                    if (player.PlayerId == infected.LAOEJKHLKAI.PlayerId)
                    {
                        isInfected = true;
                        break;
                    }

                }
                if (!isInfected)
                {
                    CrewmateIds.Add(player);
                }
            }
            return CrewmateIds;
        }
        public static PlayerControl getPlayerById(byte id)
        {
            foreach (PlayerControl player in PlayerControl.AllPlayerControls)
            {
                if (player.PlayerId == id)
                {
                    return player;
                }
            }
            return null;
        }
        [HarmonyPatch(typeof(PlayerControl), "RpcSetInfected")]
        public static void Postfix(Il2CppReferenceArray<GameDataPlayerInfo> JPGEIBIBJPJ)
        {
            MessageWriter writer = FMLLKEACGIO.Instance.StartRpcImmediately(FFGALNAPKCD.LocalPlayer.NetId, (byte)CustomRPC.SetDictator, Hazel.SendOption.None, -1);
            List<FFGALNAPKCD> crewmates = getCrewMates(JPGEIBIBJPJ);
            System.Random r = new System.Random();
            Dictator = crewmates[r.Next(0, crewmates.Count)];
            byte DictatorId = Dictator.PlayerId;
            writer.Write(DictatorId);
            FMLLKEACGIO.Instance.FinishRpcImmediately(writer);
        }
    }
}