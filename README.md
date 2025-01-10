# CustomMainMenusAPI
If you're curious on how to use this api, it's as simple as using a single method from a single class!
Here's the guid of the mod: `pixelguy.pixelmodding.baldiplus.custommainmenusapi`.

Anyways, this mod has a simple structure around a class called `MainMenuObject` in th `CustomMainMenusAPI` namespace. To create a custom main menu from it, you simply must call `MainMenuObject.CreateMenuObject()`. All the information of the parameters are documented in the .xml file.

Here's an example of usage (using the MTM101BaldDevAPI):
```
LoadingEvents.RegisterOnAssetsLoaded(Info, () =>
{
  // .. operation to get the Sprite, SoundObject or midi.
  MainMenuObject.CreateMenuObject("localizedNameForMyMenu", mySprite, mySpeech, myMidi);
}
```
