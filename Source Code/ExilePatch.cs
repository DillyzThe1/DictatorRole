using HarmonyLib;
using ExileController = CNNGMDOPELD;
using GameDataPlayerInfo = EGLJNOMOGNP.DCJMABDDJCF;
using PlayerControl = FFGALNAPKCD;

namespace DictatorRole
{
	[HarmonyPatch(typeof(ExileController), "Begin")]
	public static class ExilePatch
	{
		public static void Postfix([HarmonyArgument(0)] GameDataPlayerInfo exiled, ExileController __instance)
		{
			if (exiled.JKOMCOJCAID == PlayerControlPatch.Dictator.PlayerId && PlayerControl.GameOptions.HGOMOAAPHNJ)
			{
				__instance.EOFFAJKKDMI = exiled.EIGEKHDAKOH + " was The Dictator, apparently.";
			}
		}
	}
}