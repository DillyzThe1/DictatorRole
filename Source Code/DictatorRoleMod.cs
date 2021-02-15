
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;

namespace DictatorRole
{
    [BepInPlugin("98e06bc0-55dc-4b69-b0d9-a2d4aedf9662", "Dictator Role Mod", "1.0.1")]
    [BepInProcess("Among Us.exe")]

    public class DictatorRoleMod : BasePlugin
    {
        public override void Load()
        {
            Log.LogInfo($"{"Dictator Role Mod"} v{"1.0.1"} loaded.");

            #region ---------- Enable Harmony Patching ----------
            var harmony = new Harmony("98e06bc0-55dc-4b69-b0d9-a2d4aedf9662");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            #endregion
        }
    }
}
