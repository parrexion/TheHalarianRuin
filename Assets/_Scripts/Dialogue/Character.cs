using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour {

	public ScrObjEntryReference character;
	public IntVariable poseIndex;

	private int moveIndex;
	private Vector3 movePosition;

	[SerializeField] private SpriteRenderer characterSprite = null;
	[SerializeField] private SpriteRenderer poseSprite = null;
	

	// Use this for initialization
	void Start () {
		moveIndex = -1;
		UpdateCharacter();
	}

	public void UpdateCharacter() {
		if (character.value == null){
			characterSprite.enabled = false;
			poseSprite.enabled = false;
		}
		else {
			characterSprite.enabled = true;
			poseSprite.enabled = true;
			CharacterEntry ce = (CharacterEntry)character.value;
			characterSprite.sprite = ce.defaultColor;
			poseSprite.sprite = ce.poses[poseIndex.value];
		}
	}

	public void SetMoveDirection(Vector3 movePosition, int moveIndex) {
		this.movePosition = movePosition;
		this.moveIndex = moveIndex;
	}

	public void MoveCharacter(float moveSpeed) {
		if (moveIndex != -1) {
			StartCoroutine(Animation(movePosition, moveSpeed));
		}
	}

	IEnumerator Animation(Vector3 movePosition, float moveSpeed) {
		Vector3 startPosition = transform.position;
		Debug.Log("start     " + startPosition.ToString());
		Debug.Log("end     " + movePosition.ToString());
		Debug.Log("char     " + character.value.ToString());
		float dist = 0;
		while (dist <= moveSpeed) {
			dist += Time.deltaTime;
			transform.position = Vector3.Lerp(movePosition, startPosition, dist / moveSpeed);
			yield return null;
		}
		transform.position = startPosition;
		moveIndex = -1;
		yield break;
	}

}
