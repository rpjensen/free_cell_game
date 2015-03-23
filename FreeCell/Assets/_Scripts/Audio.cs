using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Audio : MonoBehaviour {

	// NewGame is a "card fanning" noise
	// MoveCard array is an array of various card noises
	// AudioSource is the audio source we're modifying (should be THIS' audio source)
	public AudioClip snd_NewGame;
	public List<AudioClip> snd_MoveCard;
	public AudioSource src_AudioSource;

	private bool bool_enable;

	public void Awake() {
		bool_enable = true;
	}

	// picks a random card move sound out of the ones avaliable
	public void MoveCard() {
		if (bool_enable) {
				snd_MoveCard.TrimExcess ();
				src_AudioSource.PlayOneShot (snd_MoveCard [Random.Range (0, snd_MoveCard.Count)]);
		}
	}

	// plays fanning noise
	public void NewGame() {
		if (bool_enable)
			src_AudioSource.PlayOneShot (snd_NewGame);
	}

	public void AudioDisable() {
		bool_enable = false;
	}

	public void AudioEnable() {
		bool_enable = true;
	}

	public void AudioToggle() {
		bool_enable = !bool_enable;
	}

}
