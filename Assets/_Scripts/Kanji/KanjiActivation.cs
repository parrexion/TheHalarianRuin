using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KanjiActivation : ScriptableObject {


	abstract public  bool CanActivate(KanjiValues values, MouseInformation info);
}
