using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Main controller for the battles which handles most of the initialization
/// and keeps track of objects in the battle.
/// </summary>
public class BattleController : MonoBehaviour {

	[Header("Controllers")]
	public BackgroundChanger backchange;
	public EnemyController enemyController;
	public AndroidController playerController;
	public SoldierGridController soldierController;
	public CharacterHealthGUI characterHealth;
	public BattleClock battleClock;
	public Text winText;

	[Header("Battle information")]
	public ScrObjLibraryVariable battleLibrary;
	public StringVariable battleUuid;
	public AreaInfoValues areaInfo;

	[Header("Variables")]
	public BoolVariable paused;
	public BoolVariable invincible;
	public IntVariable removeBattleSide;
	public BoolVariable alwaysEscapable;
	public BoolVariable useSlowTime;

	[Header("Delay Times")]
	public FloatVariable startupTime;
	public FloatVariable battleEndDelay;

	[Header("Audio")]
	public SfxEntry diedClip;
	public AudioVariable currentMusic;
	public AudioVariable currentSfx;
	public UnityEvent playMusicEvent;
	public UnityEvent playSfxEvent;

	[Header("Events")]
	public IntVariable currentArea;
	public UnityEvent pauseEvent;
	public UnityEvent buttonClickedEvent;
	public UnityEvent changeMapEvent;

	private BattleEntry be;
	private int state = 0;
	private bool tutorial = false;
	private bool escape = false;
	private float currentTime = 0f;
	private bool initiated = false;


	// Use this for initialization
	void Awake () {
		be = (BattleEntry)battleLibrary.GetEntry(battleUuid.value);

		tutorial = (be.backgroundHintLeft != null || be.backgroundHintRight != null);
		escape = tutorial;
		invincible.value = true;
		removeBattleSide.value = (int)be.removeSide;
		useSlowTime.value = be.useSlowTime;
		battleClock.gameObject.SetActive(useSlowTime.value && !tutorial);

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
	}
	
	// Update is called once per frame
	void Update () {
		if (!initiated)
			return;

		if (state != -1)
			currentTime += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (escape) {
				escape = false;
				StartBattle();
			}
			else if (state == 2) {
				PauseGame();
			}
		}

		if (state == 0 && currentTime >= startupTime.value-1.0f) {
			AlmostStart();
			state = 1;
		}
		else if (state == 1 && currentTime >= startupTime.value) {
			BattleStart();
			state = 2;
			currentTime = 0;
		}

		if (state == 2 && enemyController.CheckAllEnemiesDead()) {
			state = 3;
			soldierController.grid.CancelGrid();
			WonBattle();
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

	/// <summary>
	/// Called when the battle is won.
	/// </summary>
	public void WonBattle(){
		paused.value = true;
		winText.text = "YOU WIN";

		ScoreScreenValues values = GetComponent<ScoreScreenValues>();
		values.wonBattleState.value = "win";
		values.time.value = currentTime;
		values.noEnemies.value = enemyController.numberOfEnemies;
		// values.enemiesDefeated = enemyController.GetEnemiesDefeated();
		values.exp.value = enemyController.GetTotalExp();
		values.money.value = enemyController.GetTotalMoney();
		// values.treasures = enemyController.GetTreasures();

		StartCoroutine(GoToScoreScreen(battleEndDelay.value));
	}

	/// <summary>
	/// Called when the player escapes the battle.
	/// </summary>
	public void EscapeBattleButton(){
		paused.value = true;
		buttonClickedEvent.Invoke();
		winText.text = "ESCAPED!";

		ScoreScreenValues values = GetComponent<ScoreScreenValues>();
		values.wonBattleState.value = "escape";
		values.time.value = currentTime;
		values.noEnemies.value = enemyController.numberOfEnemies;

		StartCoroutine(GoToScoreScreen(battleEndDelay.value));
	}

	/// <summary>
	/// Called when it's game over.
	/// </summary>
	public void GameOverTrigger() {
		paused.value = true;
		winText.text = "YOU DIED";

		currentMusic.value = null;
		playMusicEvent.Invoke();
		currentSfx.value = diedClip.clip;
		playSfxEvent.Invoke();

		ScoreScreenValues values = GetComponent<ScoreScreenValues>();
		values.wonBattleState.value = "lose";
		values.time.value = currentTime;

		StartCoroutine(GoToScoreScreen(battleEndDelay.value));
	}

	/// <summary>
	/// Enumerator which waits a while and then moves on to the score screen.
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	IEnumerator GoToScoreScreen(float time) {
		yield return new WaitForSeconds(time);
		currentArea.value = (int)Constants.SCENE_INDEXES.SCORE;
		changeMapEvent.Invoke();
		yield break;
	}

}
