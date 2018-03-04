using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeAwayScript : MonoBehaviour {

	public float fadeDuration = 0.3f;
	public BoolVariable paused;
	
	[Header("Music")]
	public SfxList sfxList;
	public AudioVariable sfxClip;
	public UnityEvent playSfx;

	private SpriteRenderer rend;
	private Color alphaColor;
	private float colorValue;

	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer>();

		alphaColor = rend.color;
		colorValue = 1.0f;
		sfxClip.value = sfxList.RandomClip();
		playSfx.Invoke();
	}
	
	// Update is called once per frame
	void Update () {
		if (paused.value)
			return;
			
		colorValue -= fadeDuration*Time.deltaTime;
		if (colorValue <= 0)
			Destroy(gameObject);
		else {
			alphaColor = rend.color;
			alphaColor.a = colorValue;
			rend.color = alphaColor;
		}
	}
}
