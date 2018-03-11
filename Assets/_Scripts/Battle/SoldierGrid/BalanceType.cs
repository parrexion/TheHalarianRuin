using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BalanceType {

    public Sprite icon;
    public int heatValue;


    public abstract void Trigger();
}