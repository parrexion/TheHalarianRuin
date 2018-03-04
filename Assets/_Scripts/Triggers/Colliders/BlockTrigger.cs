using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTrigger : OWTrigger {

	[Header("Invisible Block")]
	public GameObject otherObjects;
	public GameObject[] blocks;


	/// <summary>
	/// Sets up all blocks and hides them from sight.
	/// </summary>
	protected override void Startup(){
		for (int i = 0; i < blocks.Length; i++) {
			blocks[i].SetActive(active);
			if (active)
				blocks[i].GetComponent<SpriteRenderer>().enabled = (active && visible);
		}
		otherObjects.SetActive(active);
	}


    public override void Trigger() {}
}
