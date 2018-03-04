using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="LibraryEntries/-Library-")]
public class ScrObjLibraryVariable : ScriptableObject {

	public bool initialized = false;
	public string pathToLibrary;
	
	[SerializeField] private List<ScrObjLibraryEntry> list = new List<ScrObjLibraryEntry>();
	private List<GUIContent> representations = new List<GUIContent>();
	private Dictionary<string, ScrObjLibraryEntry> entries = new Dictionary<string, ScrObjLibraryEntry>();

	public void GenerateDictionary() {
		Debug.Log("Generating");
		entries.Clear();
		representations.Clear();
		for (int i = 0; i < list.Count ; i++) {
			entries.Add(list[i].uuid, list[i]);
			AddRepresentation(list[i]);
		}
		if (list.Count != entries.Keys.Count)
			Debug.Log("One or more uuids are not unique!.");
		Debug.Log("Loaded " + list.Count + " entries into the " + pathToLibrary + " library");
		initialized = true;
	}

	public bool ContainsID(string id) {
		if (!initialized)
			GenerateDictionary();

		return entries.ContainsKey(id);
	}

	public ScrObjLibraryEntry GetEntry(string uuid) {
		if (!initialized)
			GenerateDictionary();

		if (entries.ContainsKey(uuid))
			return entries[uuid];
		Debug.Log("Could not find uuid: " + uuid);
		return null;
	}

	public ScrObjLibraryEntry GetEntryByIndex(int index) {
		if (!initialized)
			GenerateDictionary();

		if (index < 0)
			return null;

		return list[index];
	}

	public ScrObjLibraryEntry GetRandomEntry() {
		if (!initialized)
			GenerateDictionary();

		return list[Random.Range(0,list.Count)];
	}

	public GUIContent[] GetRepresentations(string filter, string search) {
		if (!initialized)
			GenerateDictionary();

		bool useFilter = !string.IsNullOrEmpty(filter);
		bool useSearch = !string.IsNullOrEmpty(search);

		if (!useFilter && !useSearch) {
			return representations.ToArray();
		}
		List<GUIContent> filteredList = new List<GUIContent>();
		for (int i = 0; i < list.Count; i++) {
			if ((!useFilter || list[i].tag == filter) &&
				(!useSearch || list[i].name.ToLowerInvariant().Contains(search.ToLowerInvariant())))
				filteredList.Add(representations[i]);
		}
		return filteredList.ToArray();
	}

	public int Size() {
		return list.Count;
	}

	public void AddEntry(ScrObjLibraryEntry obj) {
		if (!initialized)
			GenerateDictionary();

		list.Add(obj);
		entries.Add(obj.entryName, obj);
		AddRepresentation(obj);
	}

	public void InsertEntry(ScrObjLibraryEntry obj, int pos) {
		if (!initialized)
			GenerateDictionary();

		list.Insert(pos, obj);
		entries.Add(obj.entryName, obj);
		InsertRepresentation(obj, pos);
	}

	public void RemoveEntryByIndex(int index) {
		if (!initialized)
			GenerateDictionary();

		ScrObjLibraryEntry entry = GetEntryByIndex(index);
		list.RemoveAt(index);
		entries.Remove(entry.uuid);
		representations.RemoveAt(index);
	}

	private void AddRepresentation(ScrObjLibraryEntry entry) {
		representations.Add(entry.GenerateRepresentation());
	}

	private void InsertRepresentation(ScrObjLibraryEntry entry, int pos) {
		representations.Insert(pos,entry.GenerateRepresentation());
	}

	public Sprite TextureToSprite(Texture2D tex) {
		return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
	}
}
