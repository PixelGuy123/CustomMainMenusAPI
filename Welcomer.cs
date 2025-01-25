using UnityEngine;
using System.Collections;

namespace CustomMainMenusAPI
{
	internal class Welcomer : MonoBehaviour
	{
		internal void PlayAudio(SoundObject audio, AudioManager src, MainMenu menu) =>
			StartCoroutine(WaitForAudioPlay(menu.audioSource, src, audio, menu.gameObject));
		

		IEnumerator WaitForAudioPlay(AudioSource menuAud, AudioManager source, SoundObject audio, GameObject menuReference)
		{
			yield return null;
			yield return new WaitForEndOfFrame();
			yield return new WaitForSecondsRealtime((float)Singleton<MusicManager>.Instance.MidiPlayer.MPTK_Duration.TotalSeconds - 2f); // Small delay to actually wait for the jingle

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
