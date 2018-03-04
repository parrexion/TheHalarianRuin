using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component which updates the phone's clock to reflect the current one.
/// </summary>
public class PhoneMenuController : MonoBehaviour {

	public Text locationText;

	public Text clockText;
	public Text dateText;
	private System.DateTime currentDate;

	public Button equipButton;
	public BoolVariable equipButtonAvailable;

	public IntVariable currentArea;
	public StringVariable dialogueUUID;


	void Awake() {
		equipButton.interactable = equipButtonAvailable.value;
	}

	/// <summary>
	/// Updates the phone's clock to current time.
	/// </summary>
	void Update() {
		SetCurrentLocationText();
		currentDate = System.DateTime.Now;
		SetCurrentTimeDate();
		// if (Input.GetKeyDown(KeyCode.Escape)) {
		// 	AppHelper.Quit();
		// }
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
