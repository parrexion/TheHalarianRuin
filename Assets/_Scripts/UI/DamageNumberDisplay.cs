using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumberDisplay : MonoBehaviour {

	public IntVariable removeBattleSide;
	public BoolVariable paused;
	public int damage;
	public float time = 1f;
	private float currentTime;
	private GUIStyle styleOutline;
	private GUIStyle style;
	private Rect rN;
	private Rect rS;
	Camera[] cam;
	private bool drawTop;
	private bool drawBottom;

	void Start() {
		cam = Camera.allCameras;

		drawTop = (removeBattleSide.value != 1);
		drawBottom = (removeBattleSide.value != 2);

		rN = new Rect(-32,-32,32,32);
		rS = new Rect(-32,-32,32,32);
		SetupPositions();

		style = new GUIStyle();
		style.fontSize = (int)(25 * Screen.height / 512f);
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.white;

		styleOutline = new GUIStyle();
		styleOutline.fontSize = (int)(30 * Screen.height / 512f);
		styleOutline.alignment = TextAnchor.MiddleCenter;
		styleOutline.normal.textColor = Color.black;

	}

	void Update() {

		if (paused.value)
			return;

		currentTime += Time.deltaTime;
		if (currentTime > time)
			Destroy(gameObject,time);
	}

	void SetupPositions() {

		float size = 16*Screen.height/512;

		if (drawBottom) {
			rN = new Rect(cam[2].WorldToScreenPoint(transform.position),new Vector2(size,size));
			rN.yMin = Screen.height-rN.yMin;
			rN.yMax = Screen.height-rN.yMax;
			rN.xMin -= size*0.5f;
			rN.xMax -= size*0.5f;
		}

		if (drawTop) {
			if (drawBottom){
				rS = new Rect(cam[3].WorldToScreenPoint(transform.position),new Vector2(size,size));
			}
			else
				rS = new Rect(cam[2].WorldToScreenPoint(transform.position),new Vector2(size,size));

			rS.yMin = Screen.height-rS.yMin;
			rS.yMax = Screen.height-rS.yMax;
			rS.xMin -= size*0.5f;
			rS.xMax -= size*0.5f;

			rS.y -= size;
		}
	}

	void OnGUI() {

		if (paused.value)
			return;

		GUI.Label(rN,damage.ToString(),styleOutline);
		GUI.Label(rS,damage.ToString(),styleOutline);
		GUI.Label(rN,damage.ToString(),style);
		GUI.Label(rS,damage.ToString(),style);
	}
}
