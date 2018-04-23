using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Component which updates the phone's clock to reflect the current one.
/// </summary>
public class PhoneMenuController : MonoBehaviour {

	public StringVariable dialogueUUID; // DEBUG

	[Header("InventoryScreen stuff")]
	public IntVariable inventoryScreenIndex;
	public UnityEvent mapChangeEvent;

	[Header("Clock")]
	public Text clockText;
	public Text dateText;
	private System.DateTime currentDate;

	[Header("Location")]
	public Text locationText;
	public AreaIntVariable currentArea;

	[Header("Buttons")]
	public Button statusButton;
	public Button equipButton;
	public Button moduleButton;
	public Button saveButton;

	[Header("Button enablers")]
	public BoolVariable statusAvailable;
	public BoolVariable equipAvailable;
	public BoolVariable moduleAvailable;
	public BoolVariable saveAvailable;


	void Awake() {
		statusButton.interactable = statusAvailable.value;
		equipButton.interactable = equipAvailable.value;
		moduleButton.interactable = moduleAvailable.value;
		saveButton.interactable = saveAvailable.value;
	}

	/// <summary>
	/// Updates the phone's clock to current time.
	/// </summary>
	void Update() {
		SetCurrentLocationText();
		currentDate = System.DateTime.Now;
		SetCurrentTimeDate();
	}

	/// <summary>
	/// Sets the selected inventory screen and moves to the sceen.
	/// </summary>
	/// <param name="screenIndex"></param>
	public void GoToInventory(int screenIndex) {
		inventoryScreenIndex.value = screenIndex;
		currentArea.value = (int)Constants.SCENE_INDEXES.INVENTORY;
		mapChangeEvent.Invoke();
	}

	/// <summary>
	/// Updates the name of the current location.
	/// </summary>
	void SetCurrentLocationText(){
		if (currentArea.value == (int)Constants.SCENE_INDEXES.DIALOGUE){
			locationText.text = dialogueUUID.value;
		}
		else {
			locationText.text = ((Constants.SCENE_INDEXES)currentArea.value).ToString();
		}
	}

	/// <summary>
	/// Updates the current time and weekday.
	/// </summary>
	void SetCurrentTimeDate() {
		if (currentDate.Minute < 10)
			clockText.text = currentDate.Hour+":0"+currentDate.Minute;
		else
			clockText.text = currentDate.Hour+":"+currentDate.Minute;

		switch (currentDate.DayOfWeek) {
		case System.DayOfWeek.Monday:
			dateText.text = "Mon";
			break;
		case System.DayOfWeek.Tuesday:
			dateText.text = "Tue";
			break;
		case System.DayOfWeek.Wednesday:
			dateText.text = "Wed";
			break;
		case System.DayOfWeek.Thursday:
			dateText.text = "Thu";
			break;
		case System.DayOfWeek.Friday:
			dateText.text = "Fri";
			break;
		case System.DayOfWeek.Saturday:
			dateText.text = "Sat";
			break;
		case System.DayOfWeek.Sunday:
			dateText.text = "Sun";
			break;
		default:
			break;
		}
	}
}
