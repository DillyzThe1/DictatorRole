using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using DillyzRolesAPI.Roles;
using HarmonyLib;
using Reactor;
using Reactor.Networking;
using UnityEngine;
using DillyzRolesAPI.Options;

namespace DictatorReworked
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class DictatorReworked : BasePlugin
    {
        public const string Id = "gg.reactor.dictatorreworked";

        public Harmony Harmony { get; } = new Harmony(Id);

        public static RoleGenerator dictator = new RoleGenerator();
        public static CustomNumberOption dictatorVoteTimes = new CustomNumberOption();
        public static CustomNumberOption dictatorVotePower = new CustomNumberOption();
        public static CustomBoolOption dictatorEnabled = new CustomBoolOption();
        //public static CustomNumberOption dictatorCount = new CustomNumberOption();

        public override void Load()
        {
            Harmony.PatchAll();

            RegisterInIl2CppAttribute.Register();
            RegisterCustomRpcAttribute.Register(this);

            staticvars.noColorYoinking = true;
            //staticvars.shamelessDiscordPromo = true; experimental feature i was testing

            dictator.NameOfRole = "Dictator";
            dictator.RoleColor = new Color(0.79f, 0.47f, 0.15f, 1f);
            dictator.IntroText = "Dictate the votes to the crews power!";
            dictator.EjectionText = "was The Dictator, apparently...";
            //dictator.isEnabled = true; // used for custom settings
            dictator.canVent = false;
            dictator.Awake();

            dictatorVoteTimes.hostOptionsName = "Dictatable Votes";
            dictatorVoteTimes.defValue = 2;
            dictatorVoteTimes.minValue = 1;
            dictatorVoteTimes.maxValue = 10;
            dictatorVoteTimes.Awake();

            dictatorVotePower.hostOptionsName = "Dictator Vote Power";
            dictatorVotePower.defValue = 8;
            dictatorVotePower.minValue = 2;
            dictatorVotePower.maxValue = 100;
            dictatorVotePower.Awake();

            dictatorEnabled.hostOptionsName = "Have Dictator";
            dictatorEnabled.defValue = true;
            dictatorEnabled.Awake();

            /*dictatorCount.hostOptionsName = "Dictator Count";
            dictatorCount.defValue = 1;
            dictatorCount.minValue = 0;
            dictatorCount.maxValue = 10;
            dictatorCount.Awake();*/ // read rpcSetInfectedPatch

            NewRole.pingText.Add("Dictator mod V2.0.0");
            NewRole.modsText.Add("Dictator Mod <#F6FF00>2.0.0</color> by <#3AA3D9>DillyzThe1</color>.");
        }
    }
}
