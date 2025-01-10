using System.Collections;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using PixelInternalAPI.Extensions;

namespace CustomMainMenusAPI
{
	[HarmonyPatch(typeof(MainMenu), "Start")]
	internal static class MainMenuPatch // Intentionally public to be changed later with Endless floors
	{

		internal static void UpdateMenuTexture(int idx)
		{
			menu.transform.Find("Image").GetComponent<Image>().sprite = ActiveObject.mainMenuImage;
			lastIndex = idx;
		}
		private static void Postfix(MainMenu __instance)
		{
			// Main Menu itself
			menu = __instance;

			if (MainMenuObject.availableObjects.Count == 0)
				return;

			// .. Make it get the sprite to display
			UpdateMenuTexture(CustomMainMenusPlugin.mainMenuObjIndex);
			if (ActiveObject.audSpeech)
			{
				var welcomer = new GameObject(Singleton<LocalizationManager>.Instance.GetLocalizedText(ActiveObject.localizedName) + "_Welcomer").AddComponent<Welcomer>();
				var newSrc = welcomer.gameObject.CreateAudioManager(55f, 66f).MakeAudioManagerNonPositional();
				newSrc.ignoreListenerPause = true;
				newSrc.audioDevice.playOnAwake = false;
				welcomer.PlayAudio(ActiveObject.audSpeech, newSrc, __instance);
			}

			if (!string.IsNullOrEmpty(ActiveObject.midiName))
				__instance.transform.GetComponentInChildren<MusicPlayer>().track = ActiveObject.midiName;
		}

		internal static MainMenuObject ActiveObject => MainMenuObject.availableObjects[CustomMainMenusPlugin.mainMenuObjIndex];

		static int lastIndex = -1;

		static MainMenu menu;
	}
}
