using UnityEngine;
using System.Collections;

/// <summary>
/// Class which contains the information from the mouse movements from the player.
/// </summary>
public class MouseInformation {
    
	public Vector2 mousePosition;
	public Vector2 position1;
	public Vector2 position2 ;//{ get; private set; }

    public float distX;
    public float distY;

	/// <summary>
	/// Rotation in radians from position1 to position2
	/// </summary>
	public float rotationInternal;
	/// <summary>
	/// 
	/// </summary>
	public float rotationPlayer;

	public float holdDuration;
	public bool holding;
	public bool clicked;

    public Vector2 playerPosition;
    

	/// <summary>
	/// Sets the position for the second mouse position as well as calculating the distances 
	/// in the information.
	/// </summary>
	/// <param name="position"></param>
	public void setPosition2(Vector2 position) {
		setPosition2(position.x, position.y);
	}

	/// <summary>
	/// Sets the position for the second mouse position as well as calculating the distances 
	/// in the information.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
    public void setPosition2(float x, float y) {
        position2.Set(x, y);

        distX = position2.x - position1.x;
        distY = position2.y - position1.y;

		rotationPlayer = Mathf.Atan2(
            position2.y - playerPosition.y,
            position2.x - playerPosition.x);

		rotationInternal = Mathf.Atan2(
			position2.y - position1.y,
			position2.x - position1.x);
	}

	/// <summary>
	/// The distance from the player to the first mouse position.
	/// </summary>
	/// <returns></returns>
	public float GetPlayerPos1Distance(){
		return Vector2.Distance (playerPosition, position1);
	}
    
	/// <summary>
	/// The distance from the player to the second mouse position
	/// </summary>
	/// <returns></returns>
	public float GetPlayerPos2Distance(){
		return Vector2.Distance (playerPosition, position2);
	}

	/// <summary>
	/// The distance between the first and second mouse position.
	/// </summary>
	/// <returns></returns>
	public float GetInternalDistance(){
		return Vector2.Distance (position1, position2);
	}


	public void PrintInfo() {
		Debug.Log("Info: " + JsonUtility.ToJson(this));
	}
}
