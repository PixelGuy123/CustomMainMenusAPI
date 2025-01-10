using System.Collections.Generic;
using UnityEngine;

namespace CustomMainMenusAPI
{
	/// <summary>
	/// This is the class from the api, which holds the data necessary for the main menu changes. Use its static methods to create an instance of this object.
	/// </summary>
	public class MainMenuObject
	{
		internal readonly string localizedName;
		internal readonly Sprite mainMenuImage;
		internal readonly SoundObject audSpeech; 
		internal readonly string midiName;

		internal static List<MainMenuObject> availableObjects = [];

		internal MainMenuObject(string localizedName, Sprite newMenu, SoundObject audSpeech, string newMidi)
		{
			this.localizedName = localizedName;
			mainMenuImage = newMenu;
			this.audSpeech = audSpeech;
			midiName = newMidi;
		}
		/// <summary>
		/// This method will create an instance of the class and automatically register it to the api.
		/// </summary>
		/// <param name="localizedName">The *localized* name of the menu to be displayed in the options.</param>
		/// <param name="newMenu">The new image that the menu will use.</param>
		/// <param name="audSpeech">The speech that the menu will have (like BBCR does).</param>
		/// <param name="newMidi">The new song that the menu will have (must be midi).</param>
		public static void CreateMenuObject(string localizedName, Sprite newMenu, SoundObject audSpeech, string newMidi) =>
			availableObjects.Add(new(localizedName, newMenu, audSpeech, newMidi));
		

		/// <summary>
		/// This method will create an instance of the class and automatically register it to the api.
		/// </summary>
		/// <param name="localizedName">The *localized* name of the menu to be displayed in the options.</param>
		/// <param name="newMenu">The new image that the menu will use.</param>
		/// <param name="audSpeech">The speech that the menu will have (like BBCR does).</param>
		public static void CreateMenuObject(string localizedName, Sprite newMenu, SoundObject audSpeech) =>
			CreateMenuObject(localizedName, newMenu, audSpeech, string.Empty);
		/// <summary>
		/// This method will create an instance of the class and automatically register it to the api.
		/// </summary>
		/// <param name="localizedName">The *localized* name of the menu to be displayed in the options.</param>
		/// <param name="newMenu">The new image that the menu will use.</param>
		/// <param name="newMidi">The new song that the menu will have (must be midi).</param>
		public static void CreateMenuObject(string localizedName, Sprite newMenu, string newMidi) =>
			CreateMenuObject(localizedName, newMenu, null, newMidi);
		/// <summary>
		/// This method will create an instance of the class and automatically register it to the api.
		/// </summary>
		/// <param name="localizedName">The *localized* name of the menu to be displayed in the options.</param>
		/// <param name="newMenu">The new image that the menu will use.</param>
		public static void CreateMenuObject(string localizedName, Sprite newMenu) =>
			CreateMenuObject(localizedName, newMenu, string.Empty);
		
		
	}
}
