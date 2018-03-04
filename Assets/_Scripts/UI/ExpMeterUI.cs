using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	float currentAnimationTime;

	[Header("Bar Image")]
	public Image valueImage;
	

	// Use this for initialization
	void Start () {
		StartCoroutine(FillExpBar());
	}

	IEnumerator FillExpBar() {

		currentAnimationTime = 0;
		float startExp = totalExp.value - gainedExp.value;
		float endExp = totalExp.value;
		ExpLevel expLevel = new ExpLevel((int)startExp);
		float filled = expLevel.PercentToNext();
		valueImage.fillAmount = filled;
		Debug.Log("Filled is now: " + filled);

		Debug.Log("Waiting for start delay");
		yield return new WaitForSeconds(startDelay);
		float value = startExp;

		do {
			currentAnimationTime += Time.deltaTime / animationDuration;
			value = Mathf.Lerp(startExp,endExp,currentAnimationTime);
			bool levelUp = expLevel.SetExp((int)value);
			valueImage.fillAmount = expLevel.PercentToNext();
			// Debug.Log("Amount is now: " + value);
			if (levelUp){
				// Debug.Log("#######Levelup!");
				valueImage.color = levelupColor;
				valueImage.fillAmount = 1;
				yield return new WaitForSeconds(1);
				valueImage.fillAmount = 0;
				valueImage.color = normalColor;
			}
			yield return null;
		} while (value < endExp);

		yield break;
	}
}
