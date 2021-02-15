
using System.Reflection;
using BepInEx;
using BepInEx.IL2CPP;
using HarmonyLib;

namespace DictatorRole
{
    [BepInPlugin("b6f8476e-dd5b-127f-96bb-33c129da114c", "Dictator Role Mod", "1.0.0")]
    [BepInProcess("Among Us.exe")]

    public class DictatorRoleMod : BasePlugin
    {
        public override void Load()
        {
            Log.LogInfo($"{"Role Swap Mod"} v{"1.0.0"} loaded.");

            #region ---------- Enable Harmony Patching ----------
            var harmony = new Harmony("b6f8476e-dd5b-127f-96bb-33c129da114c");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            #endregion
        }
    }
}
