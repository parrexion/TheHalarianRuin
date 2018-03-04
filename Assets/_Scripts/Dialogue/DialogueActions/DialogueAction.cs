using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class DialogueAction : ScriptableObject {

	public abstract bool Act(DialogueScene scene, DialogueJsonItem data);
}
