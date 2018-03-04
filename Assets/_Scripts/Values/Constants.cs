using System;

public static class Constants {

	public enum Direction {NEUTRAL,LEFT,RIGHT,UP,DOWN};

	//Screen
	public const int SCREEN_WIDTH = 1200;
	public const int SCREEN_HEIGHT = 525;

	//Battle arena
	public static float AndroidBorderWidth = 6.0f;
	public static float AndroidBorderHeight = 3f;
	public static float AndroidStartX = -5.0f;
	public static float AndroidStartY = 0f;
	public static float SpiritBorderWidth = 3.5f;
	public static float SpiritBorderHeight = 2.25f;
	public static float SoldierStartX = 13.0f;
	public static float SoldierStartY = 0f;


	public const float cameraBorderWidth = 6.0f;
	public const float cameraBorderHeight = 2.9f;

	//Dialogue
	public const int DIALOGUE_PLAYERS_COUNT = 4;

	//Spirit grid
	public static int GRID_BRANCH = 5;
	public const int GRID_WIDTH = 7;
	public static float SPIRIT_SIZE = 32.0f;
	public static int SPIRIT_IMAGES = 18;
	public static int BALANCE_IMAGES = 9;

	//Weapon container
	public const int MAX_EQUIPPED_MODULE = 4;
	public const int moduleSize = 64;
	public const float moduleGuiOffsetWidth = 0.55f;
	public const float moduleGuiOffsetHeight = 0.04f;

	//Inventory
	public const int gearEquipSpace = 4;
	public const int gearBagSpace = 20;
	public const int moduleEquipSpace = 4;
	public const int moduleBagSpace = 20;

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
		BATTLETOWER = 7,
		OPTIONS = 8,
		ANDROID_BAY = 9,
		CORRIDORS = 10,
		SOUTH_GATE = 12,
		SOUTHEAST_GATE = 13,
		OUTSIDE_CORRIDORS_1 = 14,
		SHIELD_GENERATOR = 15
	}

	public enum RoomNumber {
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
		TOWER = 7,
		ANDROID_BAY = 9,
		CORRIDORS = 10,
		SOUTH_GATE = 12,
		SOUTHEAST_GATE = 13,
		OUTSIDE_CORRIDORS_1 = 14,
		SHIELD_GENERATOR = 15
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
}

