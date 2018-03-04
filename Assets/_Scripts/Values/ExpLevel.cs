using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpLevel {

    public int level;
    public int totalExp;
    public int restExp;

    private int[] levelReqs = new int[]{0,100,300,600,1000,1500,2100,2800,3600,4500,5500};

    /// <summary>
    /// Creates a ExpLevel with level and remaining exp corresponding to the total exp value.
    /// </summary>
    /// <param name="totalExp"></param>
    public ExpLevel(int totalExp) {
        SetExp(totalExp);
    }

    public bool SetExp(int totalExp) {
        int oldLevel = level;
        this.totalExp = totalExp;
        level = 1;
        while (totalExp >= levelReqs[level]) {
            level++;
        }
        restExp = levelReqs[level] - totalExp;
        // Debug.Log("Level is now: " + level + ", exp: " + restExp);
        return level != oldLevel;
    }

    public float PercentToNext() {
        float diff = levelReqs[level] - levelReqs[level-1];
        return Mathf.Min(1f,1f - (float)restExp / diff);
    }

    // /// <summary>
    // /// Creates an ExpLevel with the expected level and the remaining exp compared to that level.
    // /// </summary>
    // /// <param name="expectedLevel"></param>
    // /// <param name="totalExp"></param>
    // public ExpLevel(int expectedLevel, int totalExp) {
    //     level = expectedLevel;
    //     int realLevel = totalExp / 100 + 1;
    //     restExp = (realLevel != level) ? 0 : 100 - totalExp % 100;
    // }
}