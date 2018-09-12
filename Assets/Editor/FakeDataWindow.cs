using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

public class FakeDataWindow : EditorWindow {
	[MenuItem("My Game/Fake Data")]
	public static void ShowWindow() {
		GetWindow<FakeDataWindow>(false, "Cheats", true);
	}

	private static GUIContent
		moveButtonContent = new GUIContent("\u21b4", "move down"),
		duplicateButtonContent = new GUIContent("+", "duplicate"),
		deleteButtonContent = new GUIContent("-", "duplicate"),
		addButtonContent = new GUIContent("Add new", "add element");

	private static GUILayoutOption
        miniButtonWidth = GUILayout.Width(20f),
        normalButtonHeight = GUILayout.Height(40f);

	private bool selfProperty;
	private bool isEmpty;
	private string objectClass;
	private Type objectType;
	private object cachedObject;
	private PropertyInfo[] propertyInfos;
	private List<object[]> fields;
	private IList list;

	public void Awake() {
		isEmpty = true;
	}

	private void OnGUI() {
		minSize = new Vector2(300, 600);

		if (isEmpty) {
			objectClass = EditorGUILayout.TextField("Class", objectClass);
			objectClass = "MTest";
			selfProperty = EditorGUILayout.Toggle("Self Property", selfProperty);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Generate", GUILayout.Width(100), normalButtonHeight) && !string.IsNullOrEmpty(objectClass)) {
				objectType = typeof(BaseFB).Assembly.GetType(objectClass);
				if (objectType != null) {
					isEmpty = false;
					cachedObject = CreateNewObject();
					propertyInfos = ((BaseFB)cachedObject).GetProperties(selfProperty);
					fields = new List<object[]>();
				}
				else {
					Debug.Log("No class with that name");
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		else {
			var listType = typeof(List<>).MakeGenericType(objectType);
			if (list == null) {
				list = (IList)Activator.CreateInstance(listType);
			}

			GUILayout.Space(30);

			for (int n = 0; n < list.Count; n++) {
				RenderButtons(n);
				GUILayout.Space(10);
				if (n > list.Count - 1) break;//remove last element
				var obj = list[n];
				for (int i = 0; i < propertyInfos.Length; i++) {
					RenderField(fields[n], i);
				}

				GUILayout.Space(15);
			}

			if (list.Count == 0 && GUILayout.Button(addButtonContent, normalButtonHeight)) {
				InsertDataAt(0);
			}

			GUILayout.Space(15);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Save", GUILayout.Width(100), normalButtonHeight)) {
                AssignFields();
                if (objectType.IsSubclassOf(typeof(MBaseTest))) {
                    var tList = new List<MBaseTest>();
                    foreach (var item in list) tList.Add((MBaseTest)item);
                    Helper.SaveMasterData(tList, Application.dataPath + "/Advance/mSaved.dat");
                }
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Clear", GUILayout.Width(100), normalButtonHeight)) {
				isEmpty = true;
				objectClass = string.Empty;
			}
			EditorGUILayout.EndHorizontal();
		}
	}

    private void AssignFields()
    {
        for (int n = 0; n < list.Count; n++)
        {
            var obj = list[n];
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                propertyInfos[i].SetValue(obj, fields[n][i]);
            }
        }
    }

	private void RenderButtons(int n) {
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Element " + n, EditorStyles.boldLabel);
		if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth) && n + 1 < list.Count) {
			MoveDownDataAt(n);
		}
		if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth)) {
			InsertDataAt(n + 1);
		}
		if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth)) {
			RemoveDataAt(n);
		}
		EditorGUILayout.EndHorizontal();
	}

	private void MoveDownDataAt(int index) {
		var dataTemp = list[index + 1];
		list[index + 1] = list[index];
		list[index] = dataTemp;

		var fieldTemp = fields[index + 1];
		fields[index + 1] = fields[index];
		fields[index] = fieldTemp;
	}

	private void RemoveDataAt(int index) {
		list.RemoveAt(index);
		fields.RemoveAt(index);
	}

	private void InsertDataAt(int index) {
		list.Insert(index, CreateNewObject());
		fields.Insert(index, new object[propertyInfos.Length]);
	}

	private object CreateNewObject() {
		return Activator.CreateInstance(objectType);
	}

	private void RenderField(object[] field, int i) {
		var p = propertyInfos[i];
		var label = p.Name;
		if (p.IsType<bool>()) {
			if (field[i] == null) field[i] = false;
			field[i] = EditorGUILayout.Toggle(label, bool.Parse(string.Format("{0}", field[i])));
		}
		else if (p.IsType<short>()) {
			if (field[i] == null) field[i] = 0;
			field[i] = EditorGUILayout.LongField(label, short.Parse(string.Format("{0}", field[i])));
		}
		else if (p.IsType<int>()) {
			if (field[i] == null) field[i] = 0;
			field[i] = EditorGUILayout.IntField(label, int.Parse(string.Format("{0}", field[i])));
		}
		else if (p.IsType<long>()) {
			if (field[i] == null) field[i] = 0;
			field[i] = EditorGUILayout.LongField(label, long.Parse(string.Format("{0}", field[i])));
		}
		else if (p.IsType<float>()) {
			if (field[i] == null) field[i] = 0;
			field[i] = EditorGUILayout.FloatField(label, float.Parse(string.Format("{0}", field[i])));
		}
		else if (p.IsType<double>()) {
			if (field[i] == null) field[i] = 0;
			field[i] = EditorGUILayout.DoubleField(label, double.Parse(string.Format("{0}", field[i])));
		}
		else if (p.IsType<string>()) {
			if (field[i] == null) field[i] = string.Empty;
			field[i] = EditorGUILayout.TextField(label, string.Format("{0}", field[i]));
        }

        if (p.IsGenericListType())
        {
            GUILayout.Label(label);
            EditorGUI.indentLevel++;
            

            if (field[i] == null) field[i] = new List<string>();
            

            field[i] = EditorGUILayout.TextField(label, string.Format("{0}", field[i]));
            EditorGUI.indentLevel--;
        }
    }
}