// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChapterString))]
public class ChapterStringVariableEditor : Editor {

    Constants.CHAPTER chapter;

    public override void OnInspectorGUI () {
        ChapterString chapterString = target as ChapterString;
        if (!System.Enum.IsDefined(typeof(Constants.CHAPTER),chapterString.value)){
            chapterString.value = Constants.CHAPTER.DEFAULT.ToString();
        }
        GUILayout.Label("Value : " + chapterString.value.ToString());
        chapter = (Constants.CHAPTER)EditorGUILayout.EnumPopup("Chapter", (Constants.CHAPTER)System.Enum.Parse(typeof(Constants.CHAPTER),chapterString.value));
        chapterString.value = chapter.ToString();
    }
}