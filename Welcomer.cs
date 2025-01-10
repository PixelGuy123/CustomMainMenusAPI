using UnityEngine;
using System.Collections;

namespace CustomMainMenusAPI
{
	internal class Welcomer : MonoBehaviour
	{
		bool played = false;
		internal void PlayAudio(SoundObject audio, AudioManager src, MainMenu menu)
		{
			if (played)
				return;
			

			played = true;
			StartCoroutine(WaitForAudioPlay(menu.audioSource, src, audio, menu.gameObject));
		}

		IEnumerator WaitForAudioPlay(AudioSource menuAud, AudioManager source, SoundObject audio, GameObject menuReference)
		{
			yield return null;
			while (Singleton<MusicManager>.Instance.MidiPlaying)
				yield return null;

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
