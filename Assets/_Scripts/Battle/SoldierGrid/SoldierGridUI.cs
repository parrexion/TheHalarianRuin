using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGridUI : MonoBehaviour {

	public SoldierGrid grid;
	public IntVariable removeBattleSide;

	public Texture2D[] cards;
	private Texture2D target;

	[Header("Grid positioning")]
	public float grid_xpos = 0.25f;
	public float grid_ypos = 0.53f;
	public float grid_size = 0.1f;
	private float grid_square;


	// Use this for initialization
	void Start () {
		LoadSprites();
		CalculateRatioDifference();
	}

	/// <summary>
	/// Loads/Generates the textures for the spirit grid.
	/// </summary>
	private void LoadSprites() {
		target = new Texture2D(1,1);
		target.SetPixel(0,0,Color.red);
		target.Apply();
	}

	/// <summary>
	/// Updates the size of the UI to fit the current screen resolution.
	/// </summary>
	void CalculateRatioDifference() {
		float p2 = (float)Constants.SCREEN_HEIGHT * (float)Screen.width/(float)Constants.SCREEN_WIDTH;
		float borderAdd = ((float)Screen.height - p2) * 0.5f / Screen.height;
		float resize = (1 - 2*borderAdd);

		grid_size *= resize;
		grid_square = grid_size * 1.2f;
		grid_ypos = grid_ypos * resize + borderAdd;
	}

	/// <summary>
	/// Draws the UI for the sprit character.
	/// </summary>
	void OnGUI() {

		//Not active
		if (grid.paused.value || removeBattleSide.value == 1)
			return;

		DrawSpiritGrid();
	}

	/// <summary>
	/// Draws the spirit grid.
	/// </summary>
	void DrawSpiritGrid() {

		//No attack
		if (grid.attackDirection == Constants.Direction.NEUTRAL)
			return;

		Rect r = new Rect();
		float gridSize = Screen.height*grid_size;
		float gridSquare = Screen.height*grid_square;
		float gridDir = (grid.attackDirection == Constants.Direction.RIGHT) ? 1f : -1f;
		float gridOffset = Constants.GRID_WIDTH * 0.5f - 1;

		//Draws the grid
		for (int i = 0; i < Constants.GRID_WIDTH; i++) {
			for (int j = 0; j < Constants.GRID_BRANCH; j++) {
				if (grid.grid[j,i] == 0)
					continue;

				r.Set(Screen.width*grid_xpos+((i-gridOffset)*gridSquare*gridDir),Screen.height*grid_ypos+j*gridSquare,gridSize,gridSize);
				
				//Draws the current position on the branch
				if (j == grid.branch && i == grid.pos)
					GUI.DrawTexture(r,target);

				if (grid.grid[j,i] == SoldierGrid.GridIndex.END) {
					GUI.DrawTexture(r,grid.endPoints[j].tex);	
				}
				else {
					GUI.DrawTexture(r,cards[(int)grid.grid[j,i]]);
				}
			}
		}
	}



}
