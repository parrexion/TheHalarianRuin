using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGridUI : MonoBehaviour {

	public SoldierGrid grid;
	public IntVariable removeBattleSide;

	public Texture2D card;
	public Texture2D balanceMeter;
	private Texture2D target;

	public float balance_xpos = 0.5f;
	public float balance_ypos = 0.05f;
	public float balance_width = 0.1f;
	public float balance_height = 0.1f;

	public float grid_xpos = 0.5f;
	public float grid_ypos = 0.05f;
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
		//cards = Resources.LoadAll<Sprite>("_Sprites/Players/spirit_icons_strip17");
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

		balance_ypos = borderAdd + balance_ypos * resize;
		balance_height *= resize;
	}

	/// <summary>
	/// Draws the UI for the sprit character.
	/// </summary>
	void OnGUI() {

		DrawSpiritGrid();
		DrawBalanceMeter();
	}

	/// <summary>
	/// Draws the spirit grid.
	/// </summary>
	void DrawSpiritGrid() {
		//Not active
		if (grid.paused.value || removeBattleSide.value == 1)
			return;

		//No attack
		if (grid.attackDirection == Constants.Direction.NEUTRAL)
			return;

		Rect r = new Rect();
		Rect cut = new Rect();
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
				cut.Set(grid.grid[j,i]/(float)Constants.SPIRIT_IMAGES,0,1f/Constants.SPIRIT_IMAGES,1);
				
				//Draws the current position on the branch
				if (j == grid.branch && i == grid.pos)
					GUI.DrawTexture(r,target);

				GUI.DrawTextureWithTexCoords(r,card,cut);
			}
		}
	}

	/// <summary>
	/// Draws the current spirit balance.
	/// </summary>
	void DrawBalanceMeter() {
		Rect r = new Rect();
		Rect cut = new Rect();
		r.Set(Screen.width*balance_xpos-Screen.width*balance_width*0.5f,Screen.height*balance_ypos,Screen.width*balance_width,Screen.height*balance_height);
		cut.Set((4.0f+grid.balance)/(float)Constants.BALANCE_IMAGES, 0, 1.0f/Constants.BALANCE_IMAGES, 1);
		GUI.DrawTextureWithTexCoords(r,balanceMeter,cut);
	}

}
