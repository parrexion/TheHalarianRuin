using UnityEngine;
using System.Collections;

public class ClickManager : MonoBehaviour {

    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "ClickTrigger") {
                Debug.Log(hit.collider.gameObject.name);
                hit.collider.GetComponent<PopupTrigger>().Clicked();
            }
        }
    }

}