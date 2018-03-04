using UnityEngine;

/// <summary>
/// Base class for all the different types of entries which are 
/// added to libraries for later access.
/// </summary>
public class ScrObjLibraryEntry : ScriptableObject {

	public string uuid;
	public string entryName;
	public Color repColor;
	public string tag;


	/// <summary>
	/// Resets the values to default.
	/// </summary>
	public virtual void ResetValues() {
		uuid = "";
		entryName = "";
		repColor = new Color();
		tag = "";
	}

	/// <summary>
	/// Copies the values from another entry.
	/// </summary>
	/// <param name="other"></param>
	public virtual void CopyValues(ScrObjLibraryEntry other) {
		uuid = other.uuid;
		entryName = other.entryName;
		repColor = other.repColor;
		tag = other.tag;
	}

	public virtual GUIContent GenerateRepresentation() {
		GUIContent content = new GUIContent();
		content.text = uuid;
		Texture2D tex;
		if (repColor.a != 0){
			tex = GenerateColorTexture(repColor);
		}
		else {
			tex = GenerateRandomColor();
		}
		content.image = tex;
		return content;
	}

	protected Texture2D GenerateRandomColor() {
		Color c = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
		return GenerateColorTexture(c);
	}

	protected Texture2D GenerateColorTexture(Color c) {
		int size = 16;
		Texture2D tex = new Texture2D(size,size);
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				tex.SetPixel(i,j,c);
			}
		}
		tex.Apply();
		return tex;
	}

	public virtual bool IsEqual(ScrObjLibraryEntry other) {
		return (uuid == other.uuid);
	}

	public static bool CompareEntries(ScrObjLibraryEntry obj1, ScrObjLibraryEntry obj2) {
		if (obj1 == null) {
			if (obj2 == null)
				return true;
			else
				return false;
		}
		if (obj2 == null)
			return false;

		return obj1.IsEqual(obj2);
	}
}
