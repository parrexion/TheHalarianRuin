using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExpMeterUI : MonoBehaviour {

	[Header("EXP values")]
	public IntVariable totalExp;
	public IntVariable gainedExp;

	[Header("Animation values")]
	public Color normalColor;
	public Color levelupColor;
	public float startDelay = 1f;
	public float animationDuration = 2f;
	public GameObject levelupText;
	float currentAnimationTime;

	[Header("Bar Image")]
	public Image valueImage;

	[Header("Sounds")]
	public float sfxDelayTime = 0.25f;
	public SfxEntry gainExpSfx;
	public SfxEntry levelupSfx;
	public AudioVariable currentSfx;
	public UnityEvent playSfxEvent;
	

	// Use this for initialization
	void Start () {
		levelupText.SetActive(false);
		StartCoroutine(FillExpBar());
	}

	IEnumerator FillExpBar() {

		currentAnimationTime = 0;
		float startExp = totalExp.value - gainedExp.value;
		float endExp = totalExp.value;
		ExpLevel expLevel = new ExpLevel((int)startExp);
		float filled = expLevel.PercentToNext();
		valueImage.fillAmount = filled;

		yield return new WaitForSeconds(startDelay);
		float value = startExp;
		float currentDelay = sfxDelayTime;

		do {
			currentAnimationTime += Time.deltaTime / animationDuration;
			value = Mathf.Lerp(startExp,endExp,currentAnimationTime);
			bool levelUp = expLevel.SetExp((int)value);
			valueImage.fillAmount = expLevel.PercentToNext();
			
			currentDelay += Time.deltaTime / animationDuration;
			
			if (levelUp){
				levelupText.SetActive(true);
				valueImage.color = levelupColor;
				valueImage.fillAmount = 1;
				currentSfx.value = levelupSfx.clip;
				playSfxEvent.Invoke();
				yield return new WaitForSeconds(1);
				valueImage.fillAmount = 0;
				valueImage.color = normalColor;
				levelupText.SetActive(false);
			}
			else if (currentDelay > sfxDelayTime){
				currentSfx.value = gainExpSfx.clip;
				currentDelay = 0;
				playSfxEvent.Invoke();
			}

			yield return null;
		} while (value < endExp);

		yield break;
	}
}
