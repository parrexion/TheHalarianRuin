using UnityEngine;

/// <summary>
/// Moves a projectile in a straight line.
/// </summary>
public class MoveStraightLine : MoveScript {

    /// <summary>
    /// Calculates the straight line the projectile should move in.
    /// </summary>
    protected override void CalculateMovement() {
        if (paused.value)
			movement = new Vector2(0,0);
		else {
			movement = new Vector2(
				speed.x*direction.x,
				speed.y*direction.y
            );
		}
    }

    /// <summary>
    /// Sets the speed and rotation in the straight line it should move.
    /// </summary>
    /// <param name="baseSpeed"></param>
    /// <param name="rotation"></param>
    public override void setSpeed(Vector2 baseSpeed, float rotation) {
        speed = baseSpeed;
        direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation));
    }

}