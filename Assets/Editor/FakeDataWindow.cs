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

        userdataContent = new GUIContent("UserData"),
        masterdataContent = new GUIContent("MasterData");


    private static GUILayoutOption
        miniButtonWidth = GUILayout.Width(20f),
        normalButtonHeight = GUILayout.Height(40f);

    private static GUIStyle
        ToggleButtonStyleNormal = null,
        ToggleButtonStyleToggled = null;

    Vector2 scrollPos;

    private bool isUserData;
    private bool isNew;
    private int screenNo;
    private string dataPath = "/Advance/Data/M_Saved.dat";
    private string DataPath { get { return Application.dataPath + dataPath; } }

    private bool isSavingData;
    private bool selfProperty;
    private string objectClass;
    private Type objectType;
    private object cachedObject;
    private PropertyInfo[] propertyInfos;
    private List<object[]> fields;
    private IList list;
    private List<Type> subclassTypes;

    public void Awake() {
        screenNo = 0;
        SetupButtons();
    }

    private void SetupButtons()
    {
        ToggleButtonStyleNormal = "Button";
        ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
        ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
    }

    private void ShowUserOrMasterToggle()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(userdataContent, isUserData ? ToggleButtonStyleToggled : ToggleButtonStyleNormal))
        {
            isUserData = true;
        }

        if (GUILayout.Button(masterdataContent, isUserData ? ToggleButtonStyleNormal : ToggleButtonStyleToggled))
        {
            isUserData = false;
        }

        EditorGUILayout.EndHorizontal();
    }

    private void GetSubClassTypes()
    {
        var baseType = isUserData ? typeof(UBaseTest) : typeof(MBaseTest);
        subclassTypes = Assembly.GetAssembly(baseType)
                                .GetTypes()
                                .Where(t => t.IsSubclassOf(baseType)).ToList();
        Debug.Log(subclassTypes.Count);
    }

    private void ScreenOne()
    {
        ShowUserOrMasterToggle();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Next", GUILayout.Width(100), normalButtonHeight))
        {
            screenNo = 1;
            GetSubClassTypes();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ScreenTwo()
    {
        var btnWidth = GUILayout.Width(100);
        var btnHeight = GUILayout.Height(60);
        int col = 0, maxCol = 3;
        bool newRow = true, needEndRow = false;
        for (int i = 0; i < subclassTypes.Count; i++)
        {
            col++;
            if (newRow)
            {
                newRow = false;
                needEndRow = true;
                EditorGUILayout.BeginHorizontal();
            }
            string className = subclassTypes[i].Name;
            if (GUILayout.Button(className, (className == objectClass) ? ToggleButtonStyleToggled : ToggleButtonStyleNormal, btnWidth, btnHeight))
            {
                objectClass = className;
            }
            if (col >= maxCol)
            {
                EditorGUILayout.EndHorizontal();
                col = 0;
                needEndRow = false;
                newRow = true;
            }
        }
        if (needEndRow) EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);
        selfProperty = EditorGUILayout.Toggle("Self Property", selfProperty);
        GUILayout.Space(35);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Back", GUILayout.Width(100), normalButtonHeight))
        {
            screenNo = 0;
        }
        if (GUILayout.Button("New", GUILayout.Width(100), normalButtonHeight) && !string.IsNullOrEmpty(objectClass))
        {
            isNew = true;
            screenNo = 2;
            SetupTypeData();
            var listType = typeof(List<>).MakeGenericType(objectType);
            list = (IList)Activator.CreateInstance(listType);
        }
        if (GUILayout.Button("Load", GUILayout.Width(100), normalButtonHeight) && !string.IsNullOrEmpty(objectClass))
        {
            isNew = false;
            screenNo = 2;
            SetupTypeData();
            list = DataHelper.LoadMaster(DataPath, objectType);
            for (int i = 0; i < list.Count; i++)
            {
                object[] values = new object[propertyInfos.Length];
                for (int j = 0; j < propertyInfos.Length; j++)
                {
                    values[j] = propertyInfos[j].GetValue(list[i]);
                }
                fields.Add(values);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SetupTypeData()
    {
        objectType = typeof(BaseFB).Assembly.GetType(objectClass);
        cachedObject = CreateNewObject();
        propertyInfos = ((BaseFB)cachedObject).GetProperties(selfProperty);
        fields = new List<object[]>();
    }

    private void ScreenSave()
    {
        for (int n = 0; n < list.Count; n++)
        {
            RenderObjectButtons(n);
            GUILayout.Space(10);
            if (n > list.Count - 1) break;//remove last element
            var obj = list[n];
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                RenderField(fields[n], i);
            }

            GUILayout.Space(15);
        }

        if (list.Count == 0 && GUILayout.Button(addButtonTextContent, GUILayout.Width(100), normalButtonHeight))
        {
            InsertDataAt(0);
        }

        GUILayout.Space(15);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Width(100), normalButtonHeight) && !isSavingData)
        {
            isSavingData = true;
        }
        if (GUILayout.Button("Back", GUILayout.Width(100), normalButtonHeight))
        {
            screenNo = 1;
            objectClass = string.Empty;
        }
        EditorGUILayout.EndHorizontal();
    }

	private void OnGUI() {
		minSize = new Vector2(300, 600);
        GUILayout.Space(30);
        if (screenNo == 0) ScreenOne();
        else if (screenNo == 1) ScreenTwo();
        else if (screenNo == 2) ScreenSave();

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

	private void RenderObjectButtons(int n) {
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

            if (field[i] == null) field[i] = new List<long>();
            var fList = field[i] as List<long>;
            if (fList.Count == 0 && GUILayout.Button(addButtonIconContent, EditorStyles.miniButton, miniButtonWidth))
            {
                fList.Add(0);
                field[i] = (object)fList;
            }
            
            for(int j = 0; j < fList.Count; j++)
            {
                EditorGUILayout.BeginHorizontal();
                fList[j] = EditorGUILayout.LongField("Element " + j, fList[j]);
                if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
                {
                    fList.Insert(j + 1, 0);
                    field[i] = (object)fList;
                }
                if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
                {
                    fList.RemoveAt(j);
                    field[i] = (object)fList;
                }
                EditorGUILayout.EndHorizontal();

            }
            if(isSavingData) field[i] = (object)fList;
            EditorGUI.indentLevel--;
        }
        if (isSavingData)
        {
            SaveData();
        }
    }

    private void SaveData()
    {
        isSavingData = false;
        AssignFields();
        if (isUserData)
        {
            var tList = new List<UBaseTest>();
            foreach (var item in list) tList.Add((UBaseTest)item);
            //Helper.SaveMasterData(tList, Application.dataPath + "/Advance/Data/U_Saved.dat");
        }
        else
        {
            var tList = new List<MBaseTest>();
            foreach (var item in list) tList.Add((MBaseTest)item);
            DataHelper.SaveMasterData(tList, DataPath);
        }
    }
}