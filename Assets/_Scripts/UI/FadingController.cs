using UnityEngine;
using System.Collections;

public class FadingController : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public FloatVariable fadeSpeed;

    private float alpha = 1.0f;
    private int fadeDir = -1;


    void OnAwake() {
        alpha = 1;
        BeginFade(-1);
    }

     void OnGUI() {
        // Update alpha value
        alpha += fadeDir * fadeSpeed.value * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public void BeginFade (int direction) {
        fadeDir = direction;
    }
}