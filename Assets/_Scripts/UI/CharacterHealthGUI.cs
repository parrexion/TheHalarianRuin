using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class which handles converting the damage taken into a health bar
/// and renders it to the screen.
/// </summary>
public class CharacterHealthGUI : MonoBehaviour {
    
	private enum Side {TOP,BOTTOM};

	public BoolVariable pause;
	public BoolVariable playerImmortal;
	public IntVariable playerMaxHp;
	public FloatVariable androidDamageTaken;
	public FloatVariable soldierDamageTaken;

	public float bar_xpos = 0.04f;
	public float bar_ypos = 0.25f;
	public float bar_width = 0.1f;
	public float bar_height = 0.5f;
	public float bar_borderx = 0.05f;
	public float bar_bordery = 0.05f;

	public Text winText;

	[HideInInspector] public Rect healthRect;
	[HideInInspector] public Rect emptyRect;
	[HideInInspector] public Texture2D healthTexture;
	[HideInInspector] public Texture2D emptyTexture;
	

	// Use this for initialization
	void Start () {
		CalculateRatioDifference();

		androidDamageTaken.value = 0;
		soldierDamageTaken.value = 0;

		healthRect = new Rect(Screen.width * bar_xpos, Screen.height * bar_ypos, Screen.width * bar_width, Screen.height * bar_height);
		emptyRect = new Rect(Screen.width*(bar_xpos-bar_borderx),Screen.height*(bar_ypos-bar_bordery),Screen.width*(bar_width+2*bar_borderx),Screen.height*(bar_height+2*bar_bordery));

        healthTexture = new Texture2D(1,1);
        healthTexture.SetPixel(0,0,Color.green);
        healthTexture.Apply();
        emptyTexture = new Texture2D(1,1);
        emptyTexture.SetPixel(0,0,Color.black);
        emptyTexture.Apply();
	}

	/// <summary>
	/// Updates the size of the UI to fit the current screen resolution.
	/// </summary>
	void CalculateRatioDifference() {
		float p2 = (float)Constants.SCREEN_HEIGHT * (float)Screen.width/(float)Constants.SCREEN_WIDTH;
		float borderAdd = ((float)Screen.height - p2) * 0.5f / Screen.height;
		float resize = (1 - 2*borderAdd);

		bar_ypos = borderAdd + bar_ypos * resize;
		bar_height *= resize;
		bar_bordery *= resize;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateHealth();
	}

	/// <summary>
	/// Renders the health bar.
	/// </summary>
    void OnGUI() {
		if (pause.value || playerImmortal.value)
			return;

        GUI.DrawTexture(emptyRect,emptyTexture);
        GUI.DrawTexture(healthRect,healthTexture);
    }

	/// <summary>
	/// Updates the value of the health bar.
	/// </summary>
    void UpdateHealth() {
		float ratioTop = soldierDamageTaken.value / (float)playerMaxHp.value;
		float ratioBottom = 1 - (androidDamageTaken.value / (float)playerMaxHp.value);
        
		healthRect.yMin = Screen.height * (ratioTop * bar_height + bar_ypos);
		healthRect.yMax = Mathf.Max(Screen.height * (ratioBottom * bar_height + bar_ypos), healthRect.yMin);
    }

}
