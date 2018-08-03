using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName="Other ScrObj Variables/Audio Queue Variable")]
public class AudioQueueVariable : ScriptableObject {
	public Queue<AudioClip> value = new Queue<AudioClip>();
}
