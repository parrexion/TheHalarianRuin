using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Collider which absorbs and destroys projectiles. Used for walls and boundaries.
/// </summary>
public class HurtableAbsorbScript : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D otherCollider){
		Projectile projectile = otherCollider.gameObject.GetComponent<Projectile>();
		if (projectile != null) {
			Destroy(projectile.gameObject);
			//Debug.Log("Found you!");
		}
		else
			Debug.Log ("Null");
	}

}
