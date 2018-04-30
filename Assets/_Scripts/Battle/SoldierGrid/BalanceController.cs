using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller class for displaying and handling the balance meter.
/// </summary>
public class BalanceController : MonoBehaviour {

	public Texture2D[] icons;

	[Header("Control values")]
	public BoolVariable paused;
	public BoolVariable useSlowTime;
	public BoolVariable slowSoldierSide;
	public FloatVariable slowAmount;
	public IntVariable removeBattleSide;

    [Header("Value")]
    public BoolVariable gridUseable;
    public FloatVariable balanceValue;
    public FloatVariable maxBalanceValue;

	[Header("Cooldown")]
	public float cooldownDelay = 2.0f;
	public float cooldownValue = 4;
	private float currentCooldownWait = 0;

	[Header("Bar Image")]
	public Image valueBkg;
	public Image valueImage;

	[Header("Attacks")]
	public int damage = 15;
	// private int hits = 1;
	

	private void Start() {
		currentCooldownWait = 0;
		balanceValue.value = 0;

		valueBkg.enabled = false;
		valueImage.enabled = false;

		if (removeBattleSide.value == 1) {
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Draws the current spirit balance.
	/// </summary>
	void DrawBalanceMeter() {
		float filled = balanceValue.value / maxBalanceValue.value;
		valueImage.fillAmount = filled;
		
		valueBkg.enabled = !paused.value;
		valueImage.enabled = !paused.value;
	}

	void Update() {
		float deltaTime = (useSlowTime.value && slowSoldierSide.value) ? (Time.deltaTime * slowAmount.value) : Time.deltaTime;
		currentCooldownWait += deltaTime;

		if (currentCooldownWait >= cooldownDelay) {
			balanceValue.value = Mathf.Max(0, balanceValue.value - cooldownValue);
			currentCooldownWait -= 0.25f;
		}
		DrawBalanceMeter();
	}

	/// <summary>
	/// Triggered with every normal step in the grid.
	/// </summary>
	public void TriggerNormal() {
		balanceValue.value++;
		currentCooldownWait = 0;
	}

	/// <summary>
	/// Gets a random end point for branches.
	/// </summary>
	/// <returns></returns>
    public BalanceObject GetEndpoint() {

		int[] selection = new int[0];
		float heatPercent = balanceValue.value / maxBalanceValue.value;
		if (heatPercent < 0.2f) {
			selection = new int[]{ 1, 2, 2, 3, 3 };
		}
		else if (heatPercent < 0.5f) {
			selection = new int[]{ 0, 1, 1, 2, 2, 3, 3 };
		}
		else {
			selection = new int[]{ 0, 1, 1, 2, 2, 3 };
		}

		int index = Random.Range(0, selection.Length);

		BalanceObject bo = new BalanceObject();
		bo.tex = icons[selection[index]];
		bo.typeID = selection[index];
		return bo;
    }

	/// <summary>
	/// Generates the end attack's damage and hits depending on the current heat value.
	/// </summary>
	/// <returns></returns>
	public SoldierEndAttack GetEndAttack() {
		float heatValue = balanceValue.value / maxBalanceValue.value;
		SoldierEndAttack attack = new SoldierEndAttack();
		attack.type = SoldierEndAttack.Type.DAMAGE;
		if (heatValue < 0.25f) {
			attack.hits = 1;
			attack.value = damage;
		}
		else if (heatValue < 0.5f) {
			attack.hits = 2;
			attack.value = damage;
		}
		else if (heatValue < 0.75f) {
			attack.hits = 3;
			attack.value = damage;
		}
		else {
			attack.hits = 2;
			attack.value = damage;
		}
		return attack;
	}

	/// <summary>
	/// Triggers the effect of the end point for the branch.
	/// </summary>
	/// <param name="typeID"></param>
    public void TriggerEnd(int typeID) {
		currentCooldownWait = 0;

		if (typeID == 1) {
			balanceValue.value += 15;
			damage = 10;
		}
		else if (typeID == 2) {
			balanceValue.value += 20;
			damage = 20;
		}
		else if (typeID == 3) {
			balanceValue.value += 25;
			damage = 30;
		}
		else if (typeID == 0) {
			balanceValue.value -= 10;
			damage = 5;
		}
		else {
			Debug.LogError("Unknown endpoint type:  " + typeID);
		}

		if (balanceValue.value >= maxBalanceValue.value) {
			StartCoroutine(DestroyedCombo());
		}
    }

	/// <summary>
	/// Locks the grid and displays the destroyed combo animation.
	/// </summary>
	/// <returns></returns>
	private IEnumerator DestroyedCombo(){
		float tempBalance = 0;
		gridUseable.value = false;
		for (int i = 0; i < 8; i++) {
			yield return new WaitForSeconds(0.3f);
			float temp = tempBalance;
			tempBalance = balanceValue.value;
			balanceValue.value = temp;
		}
		balanceValue.value = 0;
		gridUseable.value = true;
	}
}