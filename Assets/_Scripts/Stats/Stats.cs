using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats {

	[SerializeField]
	public IntVariable baseValue;
	private List<int> addModifiers = new List<int>();
	private List<float> multiModifiers = new List<float>();
	private int hardcapMin = 0, hardcapMax = -1;

	public int GetValue(){
		int finalValue = baseValue.value;

		addModifiers.ForEach(x => finalValue += x);
		multiModifiers.ForEach(x => finalValue = (int)(finalValue * x));

		if (hardcapMax != -1)
			finalValue = Mathf.Min(hardcapMax,finalValue);
		finalValue = Mathf.Max(hardcapMin,finalValue);

		return finalValue;
	}

	public void DefineSettings(int[] values){
		if (values.Length < 2) {
			Debug.LogWarning("There are not enough values to define the settings!  Length: "+values.Length);
			return;
		}
		hardcapMin = values[0];
		hardcapMax = values[1];
	}

}
