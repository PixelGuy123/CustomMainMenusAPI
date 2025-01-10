using BepInEx;
using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using System.Linq;
using UnityEngine;
using System.IO;
using TMPro;

namespace CustomMainMenusAPI
{
    [BepInPlugin(mod_guid, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    internal class CustomMainMenusPlugin : BaseUnityPlugin
    {
		const string mod_guid = "pixelguy.pixelmodding.baldiplus.custommainmenusapi";
		public static int mainMenuObjIndex = 0;
		internal static AssetManager assetMan = new();
		public static CustomMainMenusPlugin i;

		private void Awake()
        {
			i = this;

			AssetLoader.LocalizationFromFunction((_) => new()
			{
				{"Opt_MainMenus_Desc" , "Switch the available Main Menus in here!"},
				{"Opt_MainMenus_DefaultMenu" , "BB+ Main Menu"}
			});

			Harmony h = new(mod_guid);
			h.PatchAll();

			// Menus Config setup
			CustomOptionsCore.OnMenuInitialize += (optInstance, handler) => handler.AddCategory<MainMenusOptionsCat>("Menus Config");

			ModdedSaveSystem.AddSaveLoadAction(this, (isSave, path) => // Save system
			{
				path = Path.Combine(path, "moddedMenusData.dat");

				if (isSave)
				{
					using BinaryWriter writer = new(File.OpenWrite(path));
					writer.Write(mainMenuObjIndex);

					return;
				}

				if (File.Exists(path))
				{
					using BinaryReader reader = new(File.OpenRead(path));
					mainMenuObjIndex = reader.ReadInt32();
				}
				else
					mainMenuObjIndex = 1; // 1 to default to a modded main menu (if available)

				mainMenuObjIndex = Mathf.Clamp(mainMenuObjIndex, 0, System.Math.Max(0, MainMenuObject.availableObjects.Count - 1));
			});

			// Adds the BB+ main menu to the system
			LoadingEvents.RegisterOnAssetsLoaded(Info, () =>
			{
				var spr = Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.GetInstanceID() > 0f && x.name == "TempMenu_Low");
				var obj = new MainMenuObject("Opt_MainMenus_DefaultMenu", spr, null, string.Empty);

				if (MainMenuObject.availableObjects.Count == 0) // Forcefully insert the object as the first item in the list
					MainMenuObject.availableObjects.Add(obj);
				else
					MainMenuObject.availableObjects.Insert(0, obj);

				// Test
				//spr = Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.GetInstanceID() > 0f && x.name == "ClipBoard_Full");
				//MainMenuObject.CreateMenuObject("Oh hi! Welcome to my super schoolhouse!", spr);

				for (int i = 0; i <= 3; i++)
				{
					string name = "MenuArrowSheet_" + i;
					assetMan.Add(name, Resources.FindObjectsOfTypeAll<Sprite>().First(x => x.GetInstanceID() > 0 && x.name == name));
				}
				

				
			}, false);
        }
    }

	internal class MainMenusOptionsCat : CustomOptionsCategory
	{
		int imaginaryIndex = CustomMainMenusPlugin.mainMenuObjIndex;

		void OnEnable()
		{
			imaginaryIndex = CustomMainMenusPlugin.mainMenuObjIndex;
			if (displayText)
				displayText.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(MainMenuObject.availableObjects[imaginaryIndex].localizedName);
			if (indexText)
				indexText.text = (imaginaryIndex + 1).ToString("D3");
		}
		public override void Build()
		{
			CreateText("MainMenuOptDesc", "Opt_MainMenus_Desc", Vector3.down * 100f, MTM101BaldAPI.UI.BaldiFonts.ComicSans24, TextAlignmentOptions.Top, new(235f, 75f), Color.black);
			displayText = CreateText("DisplayNameForMenu", MainMenuObject.availableObjects[imaginaryIndex].localizedName, Vector3.zero, MTM101BaldAPI.UI.BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new(300f, 125f), Color.black);
			indexText = CreateText("IndexDisplay", imaginaryIndex.ToString("D3"), Vector3.down * 45f, MTM101BaldAPI.UI.BaldiFonts.ComicSans24, TextAlignmentOptions.Center, new(64f, 32f), Color.black, false);

			CreateButton(() => // Right arrow
			{
				imaginaryIndex++;
				imaginaryIndex %= MainMenuObject.availableObjects.Count;

				displayText.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(MainMenuObject.availableObjects[imaginaryIndex].localizedName);
				indexText.text = (imaginaryIndex + 1).ToString("D3");

			}, CustomMainMenusPlugin.assetMan.Get<Sprite>("MenuArrowSheet_3"), CustomMainMenusPlugin.assetMan.Get<Sprite>("MenuArrowSheet_1"), "RightTogglerButton", new(45f, -45f), Vector2.one * 32f);

			CreateButton(() => // left arrow
			{
				imaginaryIndex--;
				if (imaginaryIndex < 0)
					imaginaryIndex = MainMenuObject.availableObjects.Count - 1;

				displayText.text = Singleton<LocalizationManager>.Instance.GetLocalizedText(MainMenuObject.availableObjects[imaginaryIndex].localizedName);
				indexText.text = (imaginaryIndex + 1).ToString("D3");

			}, CustomMainMenusPlugin.assetMan.Get<Sprite>("MenuArrowSheet_2"), CustomMainMenusPlugin.assetMan.Get<Sprite>("MenuArrowSheet_0"), "LeftTogglerButton", new(-45f, -45f), Vector2.one * 32f);

			CreateApplyButton(() =>
			{
				CustomMainMenusPlugin.mainMenuObjIndex = imaginaryIndex;
				MainMenuPatch.UpdateMenuTexture(CustomMainMenusPlugin.mainMenuObjIndex);
				ModdedSaveSystem.CallSaveLoadAction(CustomMainMenusPlugin.i, true, ModdedSaveSystem.GetCurrentSaveFolder(CustomMainMenusPlugin.i));
			});
		}

		TextMeshProUGUI displayText, indexText;
	}
}
