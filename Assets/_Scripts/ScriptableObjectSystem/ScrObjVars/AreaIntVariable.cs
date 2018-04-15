using UnityEngine;

[CreateAssetMenu(menuName="Other ScrObj Variables/AreaInt")]
public class AreaIntVariable : IntVariable {

    public string AreaName() {
        Constants.SCENE_INDEXES scene = (Constants.SCENE_INDEXES)value;
        return scene.ToString();
    }
}
