using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleActivation : ScriptableObject {


	abstract public  bool CanActivate(Module values, MouseInformation info);
}
