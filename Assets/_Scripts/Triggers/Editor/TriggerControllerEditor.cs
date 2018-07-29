// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

[CustomEditor(typeof(TriggerController))]
public class TriggerControllerEditor : Editor {

    public override void OnInspectorGUI () {
        TriggerController controller = target as TriggerController;
        DrawDefaultInspector();

        if (GUILayout.Button("Update Triggers")){

            UpdateTriggers(controller);
        }

        GUILayout.Space(20);

        Constants.CHAPTER cChapter = (Constants.CHAPTER)controller.currentChapter.value;
        cChapter = (Constants.CHAPTER)EditorGUILayout.EnumPopup("Current Chapter", (Constants.CHAPTER)cChapter);
        controller.currentChapter.value = (int)cChapter;

        Constants.SCENE_INDEXES cArea = (Constants.SCENE_INDEXES)controller.currentScene.value;
        cArea = (Constants.SCENE_INDEXES)EditorGUILayout.EnumPopup("Current Area", (Constants.SCENE_INDEXES)cArea);
        controller.currentScene.value = (int)cArea;
        controller.currentPlayerArea.value = controller.currentScene.value;
        
        Constants.ROOMNUMBER cRoom = (Constants.ROOMNUMBER)controller.currentRoomNumber.value;
        cRoom = (Constants.ROOMNUMBER)EditorGUILayout.EnumPopup("Room Number", (Constants.ROOMNUMBER)cRoom);
        controller.currentRoomNumber.value = (int)cRoom;

        GUILayout.Space(20);

        if (GUILayout.Button("Reactivate", GUILayout.Height(50))){
            ReactivateTriggers(controller);
        }
        if (GUILayout.Button("Verify trigger IDs")){
            VerifyTriggers(controller);
        }
        
    }


    void UpdateTriggers(TriggerController con) {
        Debug.Log("Updating triggers");
        Transform triggerParent = con.transform.GetChild(0);
        int size = triggerParent.childCount;
        TriggerChapter[] childrens = triggerParent.GetComponentsInChildren<TriggerChapter>();

        con.sectionList = childrens;
        bool res;
        for (int i = 0; i < size; i++) {
            res = childrens[i].SetupTriggers(true);
            if (res)
                EditorUtility.SetDirty(childrens[i]);
        }

        EditorUtility.SetDirty(con);
        EditorSceneManager.MarkSceneDirty(con.gameObject.scene);
    }

    void ReactivateTriggers(TriggerController con) {
        Debug.Log("Reactivating triggers");

        con.ReactivateTriggers();
        EditorUtility.SetDirty(con);
        EditorSceneManager.MarkSceneDirty(con.gameObject.scene);
    }

    void VerifyTriggers(TriggerController con) {
        Debug.Log("Verify triggers");
        Dictionary<string,string> foundIDs = new Dictionary<string, string>();
        Transform triggerParent = con.transform.GetChild(0);
        bool changed = false;

        int size = triggerParent.childCount;
        for (int i = 0; i < size; i++) {
            Transform child = triggerParent.GetChild(i);
            Debug.Log("Checking child " + child.name);
            UUID[] uuids = child.GetComponentsInChildren<UUID>(true);
            for (int j = 0; j < uuids.Length; j++) {
                if (foundIDs.ContainsKey(uuids[j].uuid)) {
                    Debug.LogWarning(string.Format("Duplicate key: {0}", uuids[j].uuid));
                    uuids[j].uuid = System.Guid.NewGuid().ToString();
                    changed = true;
                }
                else
                    foundIDs.Add(uuids[j].uuid, child.name);
            }
        }

        if (changed) {
            EditorUtility.SetDirty(con);
            EditorSceneManager.MarkSceneDirty(con.gameObject.scene);
        }
        Debug.Log("Verify triggers  -  DONE");
    }
}