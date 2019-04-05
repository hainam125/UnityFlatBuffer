using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class FakeDataWindow : EditorWindow {
	[MenuItem("My Game/Fake Data")]
	public static void ShowWindow() {
		GetWindow<FakeDataWindow>(false, "Cheats", true);
	}

	private static GUIContent
			moveButtonContent = new GUIContent("\u21b4", "move down"),
			duplicateButtonContent = new GUIContent("+", "duplicate"),
			deleteButtonContent = new GUIContent("-", "duplicate"),
			addButtonTextContent = new GUIContent("Add new", "add element"),
			addButtonIconContent = new GUIContent("+", "add element"),
			shrinkButtonIconContent = new GUIContent("\u25BC", "shrink element"),
			openButtonIconContent = new GUIContent("\u25B2", "open element"),

			userdataContent = new GUIContent("UserData"),
			masterdataContent = new GUIContent("MasterData");


	private static GUILayoutOption
			miniButtonWidth = GUILayout.Width(20f),
			normalButtonHeight = GUILayout.Height(30f);

	private static GUIStyle
			ToggleButtonStyleNormal = null,
			ToggleButtonStyleToggled = null;

	private bool isUserData;
	private bool isNew;
	private int screenNo;

	private static string defaultUserDataPath = Application.dataPath + "/Advance/Data/";
	private static string defaultMasterDataPath = Application.dataPath + "/Advance/Data/";
	private string dataPath;

	private bool isSavingData;
	private bool selfProperty;
	private string objectClass;
    private int showIdx;
	private Type objectType;
	private object cachedObject;
	private PropertyInfo[] propertyInfos;
	private List<object[]> fields;
	private List<bool[]> shrinkField;
	private List<bool> shrinkData;
	private IList list;
	private List<Type> subclassTypes;

	public void Awake() {
		screenNo = 0;
		SetupButtons();
	}

	private void OnGUI() {
		minSize = new Vector2(350, 600);
		GUILayout.Space(30);
		if (screenNo == 0) ScreenOne();
		else if (screenNo == 1) ScreenTwo();
		else if (screenNo == 2) ScreenThree();

	}

	private void SetupTypeData() {
		objectType = typeof(BaseFB).Assembly.GetType(objectClass);
		cachedObject = CreateNewObject();
		propertyInfos = ((BaseFB)cachedObject).GetProperties(selfProperty);
		fields = new List<object[]>();
		shrinkField = new List<bool[]>();
		shrinkData = new List<bool>();
	}

	private void SetupButtons() {
		ToggleButtonStyleNormal = "Button";
		ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
		ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
	}

	private void ShowUserOrMasterToggle() {
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button(userdataContent, isUserData ? ToggleButtonStyleToggled : ToggleButtonStyleNormal)) {
			isUserData = true;
		}

		if (GUILayout.Button(masterdataContent, isUserData ? ToggleButtonStyleNormal : ToggleButtonStyleToggled)) {
			isUserData = false;
		}

		EditorGUILayout.EndHorizontal();
	}

	private void GetSubClassTypes() {
		var baseType = isUserData ? typeof(UBaseTest) : typeof(MBaseTest);
		subclassTypes = Assembly.GetAssembly(baseType)
														.GetTypes()
														.Where(t => t.IsSubclassOf(baseType)).ToList();
	}

	private void ScreenOne() {
		ShowUserOrMasterToggle();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Next", GUILayout.Width(100), normalButtonHeight)) {
			screenNo = 1;
			GetSubClassTypes();
			if (isUserData) {
				dataPath = EditorPrefs.GetString("uDataPath", defaultUserDataPath);
			}
			else {
				dataPath = EditorPrefs.GetString("mDataPath", defaultMasterDataPath);
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	private void ScreenTwo() {
		var width = 100;
		var btnWidth = GUILayout.Width(width);
		var btnHeight = GUILayout.Height(width * 0.6f);
		int col = 0, maxCol = Mathf.FloorToInt(position.width / (width * 1.05f));
		bool newRow = true, needEndRow = false;
		for (int i = 0; i < subclassTypes.Count; i++) {
			col++;
			if (newRow) {
				newRow = false;
				needEndRow = true;
				EditorGUILayout.BeginHorizontal();
			}
			string className = subclassTypes[i].Name;
			if (GUILayout.Button(className, (className == objectClass) ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, btnWidth, btnHeight)) {
				objectClass = className;
			}
			if (col >= maxCol) {
				EditorGUILayout.EndHorizontal();
				col = 0;
				needEndRow = false;
				newRow = true;
			}
		}
		if (needEndRow) EditorGUILayout.EndHorizontal();
		GUILayout.Space(20);
		selfProperty = EditorGUILayout.Toggle("Self Property", selfProperty);
		EditorGUILayout.BeginHorizontal();
		dataPath = EditorGUILayout.TextField("Data Path", dataPath);
		if (GUILayout.Button("Set Default", GUILayout.Width(100), GUILayout.Height(15))) {
			dataPath = isUserData ? defaultUserDataPath : defaultMasterDataPath;
		}
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(35);
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Back", GUILayout.Width(100), normalButtonHeight)) {
			screenNo = 0;
		}
		if (GUILayout.Button("New", GUILayout.Width(100), normalButtonHeight) && !string.IsNullOrEmpty(objectClass)) {
			isNew = true;
			screenNo = 2;
            showIdx = -1;
			SetupTypeData();
			var listType = typeof(List<>).MakeGenericType(objectType);
			list = (IList)Activator.CreateInstance(listType);
		}
		if (GUILayout.Button("Load", GUILayout.Width(100), normalButtonHeight) && !string.IsNullOrEmpty(objectClass)) {
			isNew = false;
			screenNo = 2;
            showIdx = -1;
            SetupTypeData();
			LoadData();
		}
		EditorGUILayout.EndHorizontal();
	}

	private Vector2 scrollView;

	private void ScreenThree() {

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Save", GUILayout.Width(100), normalButtonHeight) && !isSavingData) {
			isSavingData = true;
		}
		if (GUILayout.Button("Back", GUILayout.Width(100), normalButtonHeight)) {
			screenNo = 1;
			objectClass = string.Empty;
		}


        if (list.Count == 0 && GUILayout.Button(addButtonTextContent, GUILayout.Width(100), normalButtonHeight)) {
			InsertDataAt(0);
			return;
		}
		EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        DrawSearchButton(showIdx, 400, newIdx => showIdx = newIdx);
        if (GUILayout.Button("X", miniButtonWidth, normalButtonHeight)) showIdx = -1;
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(15);

		scrollView = EditorGUILayout.BeginScrollView(scrollView, GUILayout.Width(position.width));
        
        for (int n = 0; n < list.Count; n++) {
            if (showIdx >= 0 && showIdx != n) continue;
			RenderObjectButtons(n);
			GUILayout.Space(10);
			if (n > list.Count - 1) break;//remove last element
			if (!shrinkData[n]) {
				for (int i = 0; i < propertyInfos.Length; i++) {
					RenderField(n, i);
				}
				GUILayout.Space(15);
			}
		}

		if (isSavingData) {
			SaveData();
		}

		EditorGUILayout.EndScrollView();

	}

    private void DrawSearchButton(int currentIdx, int width, Action<int> newIdAction ,GUIStyle toolbarButtonStyle = null, string nameDefault = "Search")
    {
        long value = 0;
        string[] namesOfValue = new string[list.Count];
        long[] idsOfValue = new long[list.Count];
        for (int i = 0; i < list.Count; i++) {
            long id = ((Base0)list[i]).Id;
            idsOfValue[i] = id;
            namesOfValue[i] = id.ToString();
            if (i == currentIdx) value = id;
        }

        var dataValues = list;


        Action<int> action = (index) =>
        {
            FilterPopup.Close();
            if (currentIdx != index)
            {
                newIdAction.Invoke(index);
                GUI.changed = true;
                Repaint();
            }
        };
        Utils.GUIModuleOfLongFilterPopupFullName(
            value,
            namesOfValue,
            idsOfValue, 
            action,
            false,
            width,
            30,
            width,
            toolbarButtonStyle,
            nameDefault
        );
    }

	private void AssignFields() {
		for (int n = 0; n < list.Count; n++) {
			var obj = list[n];
			for (int i = 0; i < propertyInfos.Length; i++) {
				propertyInfos[i].SetValue(obj, fields[n][i]);
			}
		}
	}

	private void RenderObjectButtons(int n) {
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Element " + n, EditorStyles.boldLabel);
		if (GUILayout.Button(shrinkData[n] ? shrinkButtonIconContent : openButtonIconContent, EditorStyles.miniButtonLeft, miniButtonWidth)) {
			shrinkData[n] = !shrinkData[n];
		}
		if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonMid, miniButtonWidth) && n + 1 < list.Count) {
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

		var shrinkTemp = shrinkField[index + 1];
		shrinkField[index + 1] = shrinkField[index];
		shrinkField[index] = shrinkTemp;

		var shrinkDataTemp = shrinkData[index + 1];
		shrinkData[index + 1] = shrinkData[index];
		shrinkData[index] = shrinkDataTemp;
	}

	private void RemoveDataAt(int index) {
		list.RemoveAt(index);
		fields.RemoveAt(index);
		shrinkField.RemoveAt(index);
		shrinkData.RemoveAt(index);
	}

	private void InsertDataAt(int index) {
		list.Insert(index, CreateNewObject());
		fields.Insert(index, new object[propertyInfos.Length]);
		shrinkField.Insert(index, new bool[propertyInfos.Length]);
		shrinkData.Insert(index, false);
	}

	private object CreateNewObject() {
		return Activator.CreateInstance(objectType);
	}

	private void RenderField(int n, int i) {
		object[] field = fields[n];
		var p = propertyInfos[i];
		var label = p.Name;
		if (p.IsType<bool>()) {
			if (field[i] == null) field[i] = false;
			field[i] = EditorGUILayout.Toggle(label, bool.Parse(string.Format("{0}", field[i])));
		}
		else if (p.IsType<short>()) {
			if (field[i] == null) field[i] = 0;
			field[i] = (short)EditorGUILayout.IntField(label, short.Parse(string.Format("{0}", field[i])));
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
        else if (p.PropertyType.IsEnum)
        {
            var enumType = p.PropertyType;
            if (field[i] == null) field[i] = Enum.GetValues(enumType).GetValue(0);
            field[i] = EditorGUILayout.EnumPopup(label, (Enum)Enum.ToObject(enumType, field[i]));
        }

        if (p.IsType<List<long>>()) {

			if (field[i] == null) field[i] = new List<long>();
			var fList = field[i] as List<long>;

			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(label);
			if (fList.Count == 0) {
				if (GUILayout.Button(addButtonIconContent, EditorStyles.miniButton, miniButtonWidth)) {
					fList.Add(0);
					field[i] = (object)fList;
				}
			}
			else {
				if (GUILayout.Button(shrinkField[n][i] ? shrinkButtonIconContent : openButtonIconContent, EditorStyles.miniButton, miniButtonWidth)) {
					shrinkField[n][i] = !shrinkField[n][i];
				}
			}
			EditorGUILayout.EndHorizontal();

			if (!shrinkField[n][i]) {
				EditorGUI.indentLevel++;
				field[i] = (object)fList;
				for (int j = 0; j < fList.Count; j++) {
					EditorGUILayout.BeginHorizontal();
					fList[j] = EditorGUILayout.LongField("Element " + j, fList[j]);
					if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth)) {
						fList.Insert(j + 1, 0);
						field[i] = (object)fList;
					}
					if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth)) {
						fList.RemoveAt(j);
						field[i] = (object)fList;
					}
					EditorGUILayout.EndHorizontal();

				}
				EditorGUI.indentLevel--;
			}

			if (isSavingData) field[i] = (object)fList;//make sure data are updated
		}
	}

	private void LoadData() {
		if (isUserData) {
			list = Data.LoadUserData(GetDataPath(objectType.Name + ".dat"), objectType);
		}
		else {
			list = Data.LoadMasterData(GetDataPath(objectType.Name + ".dat"), objectType);
		}
		for (int i = 0; i < list.Count; i++) {
			int length = propertyInfos.Length;
			object[] values = new object[length];
			for (int j = 0; j < length; j++) {
				values[j] = propertyInfos[j].GetValue(list[i]);
			}
			fields.Add(values);
			shrinkData.Add(false);
			shrinkField.Add(new bool[length]);
		}
	}

	private string GetDataPath(string file = "") {
		return dataPath + file;
	}

	private void SaveData() {
		isSavingData = false;
		AssignFields();
		if (isUserData) {
			var tList = new List<UBaseTest>();
			foreach (var item in list) tList.Add((UBaseTest)item);
            Data.SaveUserData(tList, GetDataPath(objectType.Name + ".dat"));
		}
		else {
			var tList = new List<MBaseTest>();
			foreach (var item in list) tList.Add((MBaseTest)item);
            Data.SaveMasterData(tList, GetDataPath(objectType.Name + ".dat"));
		}
	}
}