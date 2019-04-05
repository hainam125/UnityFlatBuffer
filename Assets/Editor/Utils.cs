using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

public static class Utils
{
    public static T LoadResource<T>(string path) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#else
			return null;
#endif
    }

    public static Texture2D LoadGUITexture(string folderPath, string filename)
    {
        Texture2D texture = AssetDatabase.LoadAssetAtPath(Path.Combine(folderPath, filename), typeof(Texture2D)) as Texture2D;
        if (texture != null)
            return texture;
        return Texture2D.whiteTexture;
    }


    //public static string ToListJson<T>(List<T> list)
    //{
    //    ListJson<T> wrapper = new ListJson<T>();
    //    wrapper.Items = list;
    //    return UnityEngine.JsonUtility.ToJson(wrapper);
    //}
    //public static ListJson<T> JsonToList<T>(string json)
    //{
    //    ListJson<T> wrapper = new ListJson<T>();
    //    UnityEngine.JsonUtility.FromJsonOverwrite(json, wrapper);
    //    return wrapper;
    //}

    //public static List<T> FromListJson<T>(string json)
    //{
    //    ListJson<T> wrapper = UnityEngine.JsonUtility.FromJson<ListJson<T>>(json);
    //    return wrapper.Items;
    //}

    public static void GUIModuleOfBool(string label, ref bool boolRef, ref bool flag,float labelWidth = 120)
    {
        var old = boolRef;
        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(labelWidth));
        }

        boolRef = GUILayout.Toggle(boolRef, "");

        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.EndHorizontal();
        }
        if (boolRef != old)
        {
            flag = true;
        }
    }

    public static void GUIModuleOfVector3(string label, ref Vector3 vector3Ref, ref bool flag)
    {
        var old = vector3Ref;
        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(65));
        }
        vector3Ref = EditorGUILayout.Vector3Field("", vector3Ref, GUILayout.Width(158));
        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.EndHorizontal();
        }
        if (vector3Ref != old)
        {
            flag = true;
        }
    }

    public static void GUIModuleOfFloat(string label, ref float floatRef, ref bool flag, float minValue = float.MinValue, float labelWidth = 120)
    {
        var old = floatRef;
        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(labelWidth));
        }

        floatRef = EditorGUILayout.FloatField(floatRef,GUILayout.Width(210-labelWidth));
        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.EndHorizontal();
        }
        if (floatRef < minValue)
        {
            floatRef = minValue;
        }
        if (floatRef != old)
        {
            flag = true;

        }
    }

    public static void GUIModuleOfInt(string label, ref int intRef, ref bool flag, int min = int.MinValue, int max = int.MaxValue, float labelWidth =  120)
    {
        var old = intRef;
        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(labelWidth));
        }
        intRef = EditorGUILayout.IntField(intRef,GUILayout.Width(220-labelWidth));
        if (intRef > max)
        {
            intRef = max;
        }
        else if (intRef < min)
        {
            intRef = min;
        }

        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.EndHorizontal();
        }

        if (old != intRef)
        {
            flag = true;
        }
    }

    public static void GUIModuleOfIntPopup(string label ,ref long refValue, int[] values, string[] names, ref bool flag)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(120));

        long old = refValue;
        refValue = EditorGUILayout.IntPopup((int)refValue, names, values);
        if (old != refValue)
        {
            flag = true;
        }
        GUILayout.EndHorizontal();
    }

    public static Enum GUIModuleOfEnum(string label, Enum enumRef,float labelWidth = 120,float popupWidth = 120, bool inLine = true)
    {
        if(inLine)
            GUILayout.BeginHorizontal();

        GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(labelWidth));
        enumRef = EditorGUILayout.EnumPopup(enumRef,GUILayout.Width(popupWidth));

        if (inLine)
            GUILayout.EndHorizontal();
        return enumRef;
    }

    public static long GUIModuleOfLongPopup(string label, long refValue, long[] values, string[] names, int[] indexes, ref bool flag, float labelWidth = 120)
    {
        GUILayout.BeginHorizontal();
        if (labelWidth > 0)
        {
            GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(labelWidth));
        }

        int index = -1;
        if (refValue > 0)
        {
            index = values.ToList().IndexOf(refValue);
        }

        long old = index;
        index = EditorGUILayout.IntPopup(index, names, indexes);
        if (old != index)
        {
            refValue = values[index];
            flag = true;
        }
        GUILayout.EndHorizontal();
        return refValue;

    }

    public static void GUIModuleOfLongFilterPopup(string label, long refValue, string[] names,long[] values, Action<int> onSelect,bool inline = true, float labelWidth = 120,float totalWidth = 220,float popupWidth = 200)
    {
        if (inline)
            GUILayout.BeginHorizontal();
        if (labelWidth > 0)
        {
            GUILayout.Label(label, EditorStyles.boldLabel, GUILayout.Width(labelWidth));
        }

        int index = -1;
        if (refValue > 0)
        {
            index = values.ToList().IndexOf(refValue);
        }

        bool clicked = false;

        if (GUILayout.Button(index >= 0 ? names[index] : "None", GUILayout.Width(totalWidth-labelWidth)))
        {
            FilterPopup.SetShowPosition();
            clicked = true;
        }

        if (inline)
            GUILayout.EndHorizontal();

        if (clicked)
        {
            FilterPopup.Browse(names, null, onSelect, 0, popupWidth);
        }
    }
    
    /// <summary>
    /// Van.tx
    /// </summary>
    /// <param name="refValue"></param>
    /// <param name="names"></param>
    /// <param name="values"></param>
    /// <param name="onSelect"></param>
    /// <param name="inline"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="popupWidth"></param>
    public static void GUIModuleOfLongFilterPopup(long refValue, string[] names,long[] values, Action<int> onSelect,bool inline = true, float width = 220, float height = 20,float popupWidth = 200)
    {
        if (inline)
            GUILayout.BeginHorizontal();

        int index = -1;
        if (refValue > 0)
        {
            index = values.ToList().IndexOf(refValue);
        }

        bool clicked = false;
        var btnName = "None";
        if (index >= 0)
        {
            btnName = names[index];
            var fIndex = btnName.IndexOf(" ");
            if (fIndex >= 0)
            {
                btnName = names[index].Substring(0,fIndex);
            }
        }
    
        if (GUILayout.Button(btnName, GUILayout.Width(width),GUILayout.Height(height)))
        {
            FilterPopup.SetShowPosition();
            clicked = true;
        }

        if (inline)
            GUILayout.EndHorizontal();

        if (clicked)
        {
            FilterPopup.Browse(names, null, onSelect, 0, popupWidth);
        }
        
        
    }
    /// <summary>
    /// Van.tx
    /// </summary>
    /// <param name="refValue"></param>
    /// <param name="names"></param>
    /// <param name="values"></param>
    /// <param name="onSelect"></param>
    /// <param name="inline"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="popupWidth"></param>
    public static void GUIModuleOfLongFilterPopupFullName(long refValue, string[] names,long[] values, Action<int> onSelect,bool inline = true, int width = 220, float height = 20
        ,float popupWidth = 200,GUIStyle toolbarButtonStyle = null, string nameDefault = "None"
        )
    {
        if (inline)
            GUILayout.BeginHorizontal();

        int index = -1;
        if (refValue > 0)
        {
            index = values.ToList().IndexOf(refValue);
        }

        bool clicked = false;
        var btnName = nameDefault;
        if (index >= 0)
        {
            btnName = names[index];
        }

        if (width == 0)
        {
            if (toolbarButtonStyle != null)
            {
                if (GUILayout.Button(btnName, toolbarButtonStyle,GUILayout.Height(height)))
                {
                    FilterPopup.SetShowPosition();
                    clicked = true;
                }
            }
            else
            {
                if (GUILayout.Button(btnName,GUILayout.Height(height)))
                {
                    FilterPopup.SetShowPosition();
                    clicked = true;
                }
            }
        }
        else
        {
            if (toolbarButtonStyle != null)
            {
                if (GUILayout.Button(btnName, toolbarButtonStyle,GUILayout.Width(width),GUILayout.Height(height)))
                {
                    FilterPopup.SetShowPosition();
                    clicked = true;
                }
            }
            else
            {
                if (GUILayout.Button(btnName,GUILayout.Width(width),GUILayout.Height(height)))
                {
                    FilterPopup.SetShowPosition();
                    clicked = true;
                }
            }
        }

        if (inline) GUILayout.EndHorizontal();

        if (clicked) FilterPopup.Browse(names, null, onSelect, 0, popupWidth);
    }
}


public class UISearchField
{

    string keyWord = "";

    public string KeyWord
    {
        get { return keyWord; }
        set { keyWord = value; }
    }

    string oldKeyWord = "";
    GUIStyle searchFieldStyle;
    GUIStyle searchCancelButtonStyle;
    GUIStyle toolbarStyle;
    GUIStyle toolbarButtonStyle;
    GUIStyle toolbarDropDownStyle;
    Action<string> _onKeyWordChange;
    Action<object> _onLeftButtonClick;
    string _leftButtonText = "Create";

    public UISearchField(Action<string> onKeyWordChange, Action<object> onLeftButtonClick, string leftButtonText = "Create")
    {
        _onKeyWordChange = onKeyWordChange;
        _onLeftButtonClick = onLeftButtonClick;

        searchFieldStyle = GUI.skin.FindStyle("ToolbarSeachTextField");
        searchCancelButtonStyle = GUI.skin.FindStyle("ToolbarSeachCancelButton");
        toolbarStyle = GUI.skin.FindStyle("Toolbar");
        toolbarButtonStyle = GUI.skin.FindStyle("toolbarbutton");
        toolbarDropDownStyle = GUI.skin.FindStyle("ToolbarDropDown");
        _leftButtonText = leftButtonText;
    }

    public void Draw()
    {
        GUILayout.BeginHorizontal(toolbarStyle);

        if (_onLeftButtonClick != null && !string.IsNullOrEmpty(_leftButtonText))
        {
            if (GUILayout.Button(_leftButtonText, toolbarButtonStyle, GUILayout.Width(50)))
            {
                if (_onLeftButtonClick != null)
                    _onLeftButtonClick(null);
            }
            GUILayout.Label("", toolbarDropDownStyle, GUILayout.Width(6));
            GUILayout.Space(5);
        }

        oldKeyWord = KeyWord;

        KeyWord = GUILayout.TextField(KeyWord, searchFieldStyle);

        if (GUILayout.Button("", searchCancelButtonStyle))
        {
            KeyWord = "";
            GUI.FocusControl(null);
        }

        if (!oldKeyWord.Equals(KeyWord))
        {
            if (_onKeyWordChange != null)
            {
                _onKeyWordChange(KeyWord);
            }
        }

        GUILayout.EndHorizontal();
    }
}

public class ListView
{
    GUIStyle menuItemStyle;
    string[] _items;
    bool _selectOnMouseHover;
    string _filter;
    Vector2 _scrollPosition;
    EditorWindow _window;

    public EditorWindow Window
    {
        get { return _window; }
    }

    Action<string> _onSelected;
    Action<int> _onSelectedIndex;

    int selectedIndex = -1;
    public string SelectedItem { get; set; }

    public ListView()
    {
        menuItemStyle = new GUIStyle(GUI.skin.FindStyle("MenuItem"));
    }

    public void SetData(string[] items, bool selectOnMouseHover, Action<string> onSelected, string filterString, EditorWindow window, Action<int> onSelectedIndex = null)
    {
        _selectOnMouseHover = selectOnMouseHover;
        _filter = filterString;
        _items = items;
        _window = window;
        _onSelected = onSelected;
        _onSelectedIndex = onSelectedIndex;
    }

    public void Draw()
    {
        //List of items
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        for (int i = 0; i < _items.Length; i++)
        {

            // Filter item by keyword
            if (!string.IsNullOrEmpty(_filter))
            {
                if (_items[i].ToLower().IndexOf(_filter.ToLower(), StringComparison.Ordinal) < 0)
                    continue;
            }

            // Draw suitable items
            if (_selectOnMouseHover)
            {
                if (GUILayout.Button(_items[i], menuItemStyle))
                {
                    DoSelect(i);
                }
            }
            else
            {
                bool val = i == selectedIndex ? true : false;
                bool newVal = GUILayout.Toggle(val, _items[i], "Button");
                if (val != newVal && newVal == true)
                {
                    DoSelect(i);
                }
            }


            // Update button's status (for hover event)
            if (_selectOnMouseHover)
                _window.Repaint();
        }

        GUILayout.EndScrollView();
    }

    void DoSelect(int i)
    {
        if (i > -1)
        {
            selectedIndex = i;
            if (_onSelected != null)
            {
                SelectedItem = _items[i];
                _onSelected(_items[i]);
            }

            if (_onSelectedIndex != null)
            {
                SelectedItem = _items[i];
                _onSelectedIndex(i);
            }
        }
    }

    public void Select(string item)
    {
        int itemIndex = ArrayUtility.IndexOf(_items, item);
        DoSelect(itemIndex);
    }
}
