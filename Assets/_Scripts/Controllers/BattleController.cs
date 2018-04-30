using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

/// <summary>
/// Main controller for the battles which handles most of the initialization
/// and keeps track of objects in the battle.
/// </summary>
public class BattleController : MonoBehaviour {

	public ScrObjLibraryVariable battleLibrary;
	public StringVariable battleUuid;
	public IntVariable currentArea;
	public AreaInfoValues areaInfo;
	private BattleEntry be;
	private BackgroundChanger backchange;

	private bool initiated = false;
	public BoolVariable paused;
	public BoolVariable invincible;
	public IntVariable removeBattleSide;
	public BoolVariable alwaysEscapable;
	public BoolVariable useSlowTime;

	public Text winText;
	public EnemyController enemyController;
	public AndroidController playerController;
	public SoldierGridController soldierController;
	public CharacterHealthGUI characterHealth;
	public BattleClock battleClock;
	public AudioClip pauseClip;

	public float startupTime = 3.0f;
	public int state = 0;
	public bool tutorial = false;
	public bool escape = false;
	public float currentTime = 0f;

	[Header("Events")]
	public UnityEvent pauseEvent;
	public UnityEvent saveGameEvent;


	// Use this for initialization
	void Awake () {
		be = (BattleEntry)battleLibrary.GetEntry(battleUuid.value);

		tutorial = (be.backgroundHintLeft != null || be.backgroundHintRight != null);
		escape = tutorial;
		invincible.value = true;
		removeBattleSide.value = (int)be.removeSide;
		useSlowTime.value = be.useSlowTime;
		battleClock.gameObject.SetActive(useSlowTime.value && !tutorial);

		backchange = GameObject.Find("Background Background").GetComponent<BackgroundChanger>();
		backchange.escapeBattleButton.interactable = be.escapeButtonEnabled;
#if UNITY_EDITOR
		backchange.escapeBattleButton.interactable = be.escapeButtonEnabled || alwaysEscapable.value;
#endif
		backchange.cameraAndroid.enabled = false;
		backchange.cameraSoldier.enabled = false;
		backchange.gridController.enabled = false;

		SetupBackgrounds();
		paused.value = true;
		StartCoroutine(CreateEnemies());
		// Debug.Log("Start");
	}

	/// <summary>
	/// Once the enemy controller is ready, create the enemies.
	/// </summary>
	/// <returns></returns>
	IEnumerator CreateEnemies(){
		while (!enemyController.initiated) {
			Debug.Log("Waiting");
			yield return null;
		}
		enemyController.CreateEnemies(be.removeSide != BattleEntry.RemoveSide.RIGHT, be.removeSide != BattleEntry.RemoveSide.LEFT);
		initiated = true;
		// Debug.Log("Initiated");
	}
	
	// Update is called once per frame
	void Update () {

		if (!initiated)
			return;

		if (state != -1)
			currentTime += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Escape)) {
			// Debug.Log("Press escape");
			if (escape) {
				escape = false;
				StartBattle();
				// Debug.Log("ESCAPED");
			}
			else if (state == 2) {
				PauseGame();
			}
		}

		if (state == 0 && currentTime >= startupTime-1.0f) {
			AlmostStart();
			state = 1;
		}
		else if (state == 1 && currentTime >= startupTime) {
			BattleStart();
			state = 2;
			currentTime = 0;
		}

		if (state == 2 && enemyController.CheckAllEnemiesDead()) {
			state = 3;
			winText.text = "YOU WIN!";
			EndBattle();
			StartCoroutine(WonBattle(3f));
		}
	}

	/// <summary>
	/// Sets up the backgrounds. 
	/// </summary>
	private void SetupBackgrounds() {
		if (tutorial && be.isTutorial) {
			if (be.backgroundHintRight != null) {
				backchange.tutorialAndroid.sprite = be.backgroundHintRight;
				tutorial = false;
			}
			if (be.backgroundHintLeft != null) {
				backchange.tutorialSoldier.sprite = be.backgroundHintLeft;
				tutorial = false;
			}
			state = -1;
			return;
		}

		invincible.value = be.playerInvincible;
		
		if (be.removeSide == BattleEntry.RemoveSide.RIGHT) {
			backchange.tutorialAndroid.sprite = be.backgroundRight;
		}
		else {
			backchange.tutorialAndroid.enabled = false;
			backchange.cameraAndroid.enabled = true;
			backchange.transformAndroid.sprite = be.backgroundRight;
		}
		if (be.removeSide == BattleEntry.RemoveSide.LEFT)
			backchange.tutorialSoldier.sprite = be.backgroundLeft;
		else {
			backchange.tutorialSoldier.enabled = false;
			backchange.cameraSoldier.enabled = true;
			backchange.transformSoldier.sprite = be.backgroundLeft;
			backchange.gridController.enabled = true;
		}
		
		winText.text = "GET READY!";
		battleClock.gameObject.SetActive(useSlowTime.value);
		state = 0;
	}

	/// <summary>
	/// Called when entering the battle scene.
	/// </summary>
	private void StartBattle() {
		SetupBackgrounds();
	}

	/// <summary>
	/// Called when the battle almost have started.
	/// </summary>
	private void AlmostStart() {
		winText.text = "FIGHT!";
	}
	
	/// <summary>
	/// Pauses or unpauses the game when called.
	/// </summary>
	public void PauseGame() {
		paused.value = !paused.value;
		pauseEvent.Invoke();
	}

	/// <summary>
	/// Called when the battle starts.
	/// </summary>
	private void BattleStart() {
		winText.text = "";
		paused.value = false;
	}

	public void EndBattle(){
		paused.value = true;
		soldierController.grid.CancelGrid();
	}

	public void EscapeBattleButton(float time){
		StartCoroutine(EscapedBattle(time));
	}

	public IEnumerator WonBattle(float time){
		ScoreScreenValues values = GetComponent<ScoreScreenValues>();
		values.wonBattleState.value = "win";
		values.time.value = currentTime;
		values.noEnemies.value = enemyController.numberOfEnemies;
		// values.enemiesDefeated = enemyController.GetEnemiesDefeated();
		values.exp.value = enemyController.GetTotalExp();
		values.money.value = enemyController.GetTotalMoney();
		// values.treasures = enemyController.GetTreasures();

		if (be.playerArea == Constants.OverworldArea.BATTLETOWER)
			saveGameEvent.Invoke();

		Debug.Log("Won");

		yield return new WaitForSeconds(time);
		currentArea.value = (int)Constants.SCENE_INDEXES.SCORE;
		SceneManager.LoadScene(currentArea.value);
	}

	public IEnumerator EscapedBattle(float time){
		ScoreScreenValues values = GetComponent<ScoreScreenValues>();
		values.wonBattleState.value = "escape";
		values.time.value = currentTime;
		values.noEnemies.value = enemyController.numberOfEnemies;

		Debug.Log("Escaped battle");

		winText.text = "ESCAPED!";
		yield return new WaitForSeconds(time);
		currentArea.value = (int)Constants.SCENE_INDEXES.SCORE;
		SceneManager.LoadScene(areaInfo.GetArea(currentArea.value).sceneID);
		yield return 0;
	}

	public void GameOverTrigger() {
		StartCoroutine(LostBattle(3f));
		paused.value = true;
	}

	public IEnumerator LostBattle(float time) {
		winText.text = "YOU DIED";

		ScoreScreenValues values = GetComponent<ScoreScreenValues>();
		values.wonBattleState.value = "lose";
		values.time.value = currentTime;

		yield return new WaitForSeconds(time);
		currentArea.value = (int)Constants.SCENE_INDEXES.SCORE;
		SceneManager.LoadScene(currentArea.value);
		yield return 0;
	}


	public static IEnumerator EndGame(float time){
		yield return new WaitForSeconds(time);
		AppHelper.Quit();
	}
}
