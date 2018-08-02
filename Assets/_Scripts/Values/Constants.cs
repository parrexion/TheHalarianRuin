using System;

public static class Constants {

	public enum Direction {NEUTRAL,LEFT,RIGHT,UP,DOWN};

	//Screen
	public const int SCREEN_WIDTH = 1200;
	public const int SCREEN_HEIGHT = 525;

	//Battle arena
	// public const float ANDROID_BORDER_WIDTH = 6.1f;
	// public const float ANDROID_BORDER_HEIGHT = 3.5f;
	// public const float ANDROID_START_X = -5.0f;
	// public const float ANDROID_START_Y = 0f;
	public const float SOLDIER_BORDER_WIDTH = 3.5f;
	public const float SOLDIER_BORDER_HEIGHT = 2.25f;
	public const float SOLDIER_START_X = 13.0f;
	public const float SOLDIER_START_Y = -1f;
	public const float CAMERA_BORDER_WIDTH_ANDROID = 6.0f;
	public const float CAMERA_BORDER_HEIGHT_ANDROID = 2.9f;
	public const float ENEMY_OFFSET_MIN_ANDROID = 2f;
	public const float ENEMY_OFFSET_MAX_ANDROID = 4f;
	public const float ENEMY_OFFSET_XMIN_SOLDIER = 2f;
	public const float ENEMY_OFFSET_XMAX_SOLDIER = 5f;
	public const float ENEMY_OFFSET_YMIN_SOLDIER = -2f;
	public const float ENEMY_OFFSET_YMAX_SOLDIER = 0.5f;
	

	//Dialogue
	public const int DIALOGUE_PLAYERS_COUNT = 4;

	//Spirit grid
	public const int GRID_BRANCH = 5;
	public const int GRID_WIDTH = 7;

	//Weapon container
	public const int MODULE_SPRITE_SIZE = 64;
	public const float MODULE_GUI_XPOS = 0.55f;
	public const float MODULE_GUI_YPOS = 0.04f;

	//Inventory
	public const int GEAR_EQUIP_SPACE = 4;
	public const int MODULE_EQUIP_SPACE = 4;
	// public const int MODULE_EQUIP_VISIBLE = 4;
	// public const int MODULE_EQUIP_ROWLENGTH = 4;
	public const int GEAR_BAG_SPACE = 20;
	public const int MODULE_BAG_SPACE = 20;
	// public const int MODULE_BAG_VISIBLE = 10;
	// public const int MODULE_BAG_ROWLENGTH = 5;

	//Player stats
	public const int PLAYER_HEALTH_BASE = 50;
	public const int PLAYER_HEALTH_SCALE = 50;

	
	/// <summary>
	/// Enum for all the scenes
	/// </summary>
	public enum SCENE_INDEXES {
		STARTUP = 0,
		MAINMENU = 1,
		DIALOGUE = 3,
		BATTLE = 4,
		SCORE = 5,
		INVENTORY = 6,
		SHOP = 7,
		OPTIONS = 8,
		ANDROID_BAY = 9,
		CORRIDORS = 10,
		SOUTH_GATE = 12,
		SOUTHEAST_GATE = 13,
		OUTSIDE_CORRIDORS_1 = 14,
		SHIELD_GENERATOR = 15,
		TEST_SCENE = 50
	}

	public enum ROOMNUMBER {
		ROOM_A = 0,
		ROOM_B = 1,
		ROOM_C = 2,
		ROOM_D = 3
	}

	/// <summary>
	/// Enum for all areas in the game.
	/// </summary>
	public enum OverworldArea {
		DEFAULT = 0,
		MAINMENU = 1,
		ANDROID_BAY = 9,
		CORRIDORS = 10,
		SOUTH_GATE = 12,
		SOUTHEAST_GATE = 13,
		OUTSIDE_CORRIDORS_1 = 14,
		SHIELD_GENERATOR = 15,
		TEST_SCENE = 50
	}

	public enum CHAPTER {
		DEFAULT = 0,
		PROLOGUE = 1,
		CHAPTER1,
		CHAPTER2,
		CHAPTER3,
		CHAPTER4,
		CHPATER5,
		EPILOGUE
	}


	/// UTILITY FUNCTIONS

	public static string PlayTimeFromInt(int playTime, bool useSeconds) {
		int _seconds = (playTime % 60);
		int _minutes = (playTime / 60) % 60;
		int _hours = (playTime / 3600);
		if (useSeconds)
			return string.Format("{0} : {1:D2} : {2:D2}",_hours, _minutes, _seconds);
		else
			return string.Format("{0} : {1:D2}",_hours, _minutes);
	}
}

