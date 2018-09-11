using UnityEngine;
using UnityEditor;
using System;


public class FakeDataWindow : EditorWindow {
	[MenuItem("My Game/Fake Data")]
	public static void ShowWindow() {
		GetWindow<FakeDataWindow>(false, "Cheats", true);
	}

	public string objectClass;
	public bool selfProperty;
	public bool isEmpty;
	public object[] fields;

	public void Awake() {
		fields = new object[50];
		isEmpty = true;
	}

	private void OnGUI() {
		minSize = new Vector2(300, 300);

		if (isEmpty) {
			objectClass = EditorGUILayout.TextField("Class", objectClass);
			objectClass = "UTest";
			selfProperty = EditorGUILayout.Toggle("Self Property", selfProperty);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Generate", GUILayout.Width(100), GUILayout.Height(40)) && !string.IsNullOrEmpty(objectClass)) {
				var type = typeof(BaseFB).Assembly.GetType(objectClass);
				if (type != null) isEmpty = false;
				else {
					Debug.Log("No class with that name");
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		else {
			var obj = Activator.CreateInstance(typeof(BaseFB).Assembly.GetType(objectClass));
			var properties = ((BaseFB)obj).GetProperties(selfProperty);
			for (int i = 0; i < properties.Length; i++) {
				var p = properties[i];
				string label = p.Name;
				if (p.IsType<bool>()) {
					if (fields[i] == null) fields[i] = false;
					fields[i] = EditorGUILayout.Toggle(label, bool.Parse(string.Format("{0}",fields[i])));
				}
				else if (p.IsType<short>()) {
					if(fields[i] == null) fields[i] = 0;
					fields[i] = EditorGUILayout.LongField(label, short.Parse(string.Format("{0}",fields[i])));
				}
				else if (p.IsType<int>()) {
					if(fields[i] == null) fields[i] = 0;
					fields[i] = EditorGUILayout.IntField(label, int.Parse(string.Format("{0}",fields[i])));
				}
				else if (p.IsType<long>()) {
					if(fields[i] == null) fields[i] = 0;
					fields[i] = EditorGUILayout.LongField(label, long.Parse(string.Format("{0}",fields[i])));
				}
				else if (p.IsType<float>()) {
					if(fields[i] == null) fields[i] = 0;
					fields[i] = EditorGUILayout.FloatField(label, float.Parse(string.Format("{0}",fields[i])));
				}
				else if (p.IsType<double>()) {
					if(fields[i] == null) fields[i] = 0;
					fields[i] = EditorGUILayout.DoubleField(label, double.Parse(string.Format("{0}",fields[i])));
				}
				else if (p.IsType<string>()) {
					if(fields[i] == null) fields[i] = string.Empty;
					fields[i] = EditorGUILayout.TextField(label, string.Format("{0}",fields[i]));
				}
			}

			GUILayout.FlexibleSpace();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(40))) {
				isEmpty = true;
				objectClass = string.Empty;
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Clear", GUILayout.Width(100), GUILayout.Height(40))) {
				isEmpty = true;
				objectClass = string.Empty;
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}