using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGrid : MonoBehaviour {

	public enum GridIndex {NOTHING, LEFT, RIGHT, UP, DOWN, S_LEFT, S_RIGHT, G_LEFT, G_RIGHT, G_UP, G_DOWN, END}

	public BoolVariable paused;
	public BalanceController balance;
	public GridIndex[,] grid;
	public BalanceObject[] endPoints;
	public int branch;
	public int pos;
	public Constants.Direction attackDirection;
	public Constants.Direction lastDirection;

	// Use this for initialization
	void Start () {
		grid = new GridIndex[Constants.GRID_BRANCH,Constants.GRID_WIDTH];
		endPoints = new BalanceObject[Constants.GRID_BRANCH];

		branch = 2;
		pos = 0;
		attackDirection = Constants.Direction.NEUTRAL;
		lastDirection = Constants.Direction.NEUTRAL;
	}

	private void GenerateGrid(Constants.Direction dir) {

		for (int i = 0; i < Constants.GRID_WIDTH; i++) {
			for (int j = 0; j < Constants.GRID_BRANCH; j++) {
				grid[j,i] = GridIndex.NOTHING;
			}
		}

		NormalGrid(dir,2,2,1,2);
	}

	/*
	 * Generates the soldier grid with the following values
	 * 
	 * Dir					Which direction the grid is travelling in (left / right)
	 * diff_low				lowest number of arrows before the split
	 * diff_high			highest number of arrows before the split
	 * branch_width_low		lowest number of arrows after the split
	 * branch_width_high	highest number of arrows after the split
	 */
	private void NormalGrid(Constants.Direction dir,int diff_low, int diff_high, int branch_width_low, int branch_width_high) {
		int a, b, c, diff;

		pos = 0;
		branch = 2;

		diff = Random.Range(diff_low, diff_high+1);
		a = 1+diff+Random.Range(branch_width_low, branch_width_high);
		b = 1+diff+Random.Range(branch_width_low, branch_width_high+1);
		c = 1+diff+Random.Range(branch_width_low, branch_width_high);

		GridIndex arrow = GridIndex.RIGHT;
		if (dir == Constants.Direction.LEFT)
			arrow = GridIndex.LEFT;

		for (int i = 0; i < Constants.GRID_WIDTH; i++) {

			if (i < diff) {
				grid[2, i] = arrow;
			}
			else if (i == diff) {
				grid[1, i] = GridIndex.UP;
				grid[2, i] = arrow;
				grid[3, i] = GridIndex.DOWN;
			}
			else {
				if (i == b) {
					endPoints[2] = balance.GetEndpoint();
					grid[2, i] = GridIndex.END;
				}
				else if (i < b)
					grid[2, i] = arrow;

				if (i == a){
					endPoints[1] = balance.GetEndpoint();
					grid[1, i] = GridIndex.END;
				}
				else if (i < a)
					grid[1, i] = arrow;

				if (i == c) {
					endPoints[3] = balance.GetEndpoint();
					grid[3, i] = GridIndex.END;
				}
				else if (i < c)
					grid[3, i] = arrow;
			}
		}
		grid[2,0] = (arrow == GridIndex.LEFT) ? GridIndex.S_LEFT : GridIndex.S_RIGHT;
	}	

	public bool MoveGrid(Constants.Direction dir){

		if (dir == Constants.Direction.RIGHT) {

			if (attackDirection == Constants.Direction.NEUTRAL) {
				attackDirection = Constants.Direction.RIGHT;
				GenerateGrid(attackDirection);
			}

			if (attackDirection != Constants.Direction.RIGHT) {
				return false;
			}

			if (grid[branch,pos+1] == GridIndex.NOTHING) {
				return false;
			}
			else if (grid[branch,pos+1] == GridIndex.RIGHT) {
				if (grid[branch-1,pos] == GridIndex.UP)
					CancelBranches(branch);
				else if (grid[branch+1,pos] == GridIndex.DOWN)
					CancelBranches(branch);

				grid[branch,pos] = (grid[branch,pos] == GridIndex.S_LEFT) ? GridIndex.G_LEFT : GridIndex.G_RIGHT;
				grid[branch,pos+1] = (grid[branch,pos+1] == GridIndex.LEFT) ? GridIndex.S_LEFT : GridIndex.S_RIGHT;
				pos++;
				return false;
			}
			else {
				EndReached(branch);
				return true;
			}
		}
		else if (dir == Constants.Direction.LEFT) {

			if (attackDirection == Constants.Direction.NEUTRAL) {
				attackDirection = Constants.Direction.LEFT;
				GenerateGrid(attackDirection);
			}

			if (attackDirection != Constants.Direction.LEFT) {
				return false;
			}

			if (grid[branch,pos+1] == 0) {
				return false;
			}
			else if (grid[branch,pos+1] == GridIndex.LEFT) {
				if (grid[branch-1,pos] == GridIndex.UP)
					CancelBranches(branch);
				else if (grid[branch+1,pos] == GridIndex.DOWN)
					CancelBranches(branch);

				grid[branch,pos] = (grid[branch,pos] == GridIndex.S_LEFT) ? GridIndex.G_LEFT : GridIndex.G_RIGHT;
				grid[branch,pos+1] = (grid[branch,pos+1] == GridIndex.LEFT) ? GridIndex.S_LEFT : GridIndex.S_RIGHT;
				pos++;
				return false;
			}
			else {
				EndReached(branch);
				return true;
			}
		}
		else if (dir == Constants.Direction.UP) {
			if (grid[branch-1,pos] == GridIndex.UP) {
				grid[branch,pos] = (grid[branch,pos] == GridIndex.S_LEFT) ? GridIndex.G_LEFT : GridIndex.G_RIGHT;
				grid[branch-1,pos] = GridIndex.G_UP;
				grid[branch-1,pos+1] = (grid[branch,pos+1] == GridIndex.LEFT) ? GridIndex.S_LEFT : GridIndex.S_RIGHT;
				pos++;
				branch--;
				CancelBranches(branch);
				return true;
			}
			else {
				return false;
			}
		}
		else if (dir == Constants.Direction.DOWN) {
			if (grid[branch+1,pos] == GridIndex.DOWN) {
				grid[branch,pos] = (grid[branch,pos] == GridIndex.S_LEFT) ? GridIndex.G_LEFT : GridIndex.G_RIGHT;
				grid[branch+1,pos] = GridIndex.G_DOWN;
				grid[branch+1,pos+1] = (grid[branch,pos+1] == GridIndex.LEFT) ? GridIndex.S_LEFT : GridIndex.S_RIGHT;
				pos++;
				branch++;
				CancelBranches(branch);
				return true;
			}
			else {
				return false;
			}
		}

		Debug.LogWarning("Wrong Argument");
		return false;
	}


	public void CancelGrid(){
		attackDirection = Constants.Direction.NEUTRAL;
		branch = 2;
		pos = 0;
	}

	/// <summary>
	/// Called when the end of a branch is reached.
	/// </summary>
	/// <param name="branchIndex"></param>
	private void EndReached(int branchIndex) {
		lastDirection = attackDirection;
		CancelGrid();

		balance.TriggerEnd(endPoints[branchIndex].typeID);
	}

	/// <summary>
	/// Greys out the other branches when the player selectes the branch to continue down.
	/// </summary>
	/// <param name="currentBranch"></param>
	private void CancelBranches(int currentBranch) {
		for (int i = 0; i < Constants.GRID_BRANCH; i++) {
			if (i == currentBranch)
				continue;

			for (int j = 0; j < Constants.GRID_WIDTH; j++) {
				switch (grid[i,j])
				{
					case GridIndex.LEFT:
						grid[i,j] = GridIndex.G_LEFT;
						break;
					case GridIndex.RIGHT:
						grid[i,j] = GridIndex.G_RIGHT;
						break;
					case GridIndex.UP:
						grid[i,j] = GridIndex.G_UP;
						break;
					case GridIndex.DOWN:
						grid[i,j] = GridIndex.G_DOWN;
						break;
				}
			}
		}
	}

}
