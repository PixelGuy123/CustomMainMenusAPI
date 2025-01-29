using UnityEngine;
using System.Collections;

namespace CustomMainMenusAPI
{
	internal class Welcomer : MonoBehaviour
	{
		internal void PlayAudio(MainMenuObject mainMenuObject, AudioManager src, MainMenu menu) =>
			StartCoroutine(WaitForAudioPlay(mainMenuObject, menu.audioSource, src, mainMenuObject.audSpeech, menu.gameObject));
		

		IEnumerator WaitForAudioPlay(MainMenuObject mainMenuObject, AudioSource menuAud, AudioManager source, SoundObject audio, GameObject menuReference)
		{
			while (Singleton<MusicManager>.Instance.MidiPlayer.MPTK_MidiName != mainMenuObject.midiName)
				yield return null;

			float delay = (float)Singleton<MusicManager>.Instance.MidiPlayer.MPTK_Duration.TotalSeconds - 2f;
			while (delay > 0f)
			{
				delay -= Time.unscaledDeltaTime;
				yield return null;
			}

			source.QueueAudio(audio);

			while (source.AnyAudioIsPlaying)
			{
				if (!menuReference.activeSelf || menuAud.isPlaying)
					source.FlushQueue(true);
				yield return null;
			}

			yield break;
		}

	}
}
