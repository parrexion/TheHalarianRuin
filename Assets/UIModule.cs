using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModule : MonoBehaviour {

	public Image uiImage;
	public CanvasRenderer m_Renderer;
	public Material baseMaterial;
	public BoolVariable paused;

	[Header("Shader values")]
	public float fillAmount;
	public bool isActive;

	private Material newMaterial;
	private bool initialized = false;
	private bool show;


	private void Update() {
		SetVisible();
	}

	/// <summary>
	/// Sets if the module is visible or not.
	/// </summary>
	/// <param name="state"></param>
	public void SetVisible() {
		uiImage.enabled = show && !paused.value;
	}

	/// <summary>
	/// Sets the icons the module will use.
	/// </summary>
	/// <param name="activeIcon"></param>
	/// <param name="chargingIcon"></param>
	public void SetIcons(Sprite activeIcon, Sprite chargingIcon) {
		newMaterial = new Material(baseMaterial);

		if (activeIcon == null || chargingIcon == null) {
			uiImage.enabled = false;
			show = false;
			gameObject.SetActive(false);
			return;
		}

		newMaterial.SetTexture("_ActiveTex", activeIcon.texture);
		newMaterial.SetTexture("_InactiveTex", chargingIcon.texture);
		show = true;

		StartCoroutine(WaitForRenderer());
	}

	/// <summary>
	/// Waits for the renderer to initialize.
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForRenderer() {
		while (m_Renderer.materialCount < 1)
			yield return null;

		m_Renderer.SetMaterial(newMaterial, 0);
		initialized = true;
		yield break;
	}

	public void SetValue(float value, bool active) {
		fillAmount = value;
		isActive = active;

		newMaterial.SetFloat("_charged", fillAmount);
		newMaterial.SetFloat("_IsActive", (isActive) ? 1 : 0);
		if (initialized && m_Renderer.materialCount > 0 && !paused.value)
			m_Renderer.SetMaterial(newMaterial, 0);
	}
	
}
