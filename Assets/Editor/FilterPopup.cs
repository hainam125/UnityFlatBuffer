using UnityEngine;
using UnityEditor;
using System;

public class FilterPopup : EditorWindow
{

	static private string[] mItems;

	static private Action<string> OnSelected { get; set; }
	static private Action<int> OnSelectedIndex { get; set; }

	const int MEMBER_HEIGHT = 37;
	static Rect inspectorRect;
	static Vector2 inspectorPos;
	static Vector2 scrollPosition;
	static UISearchField searchField;
	static ListView listView;
    static float mCustomWidth = 0;
    static float offsetLeft = 0;
	/// <summary>
	/// Show window as dropdown popup
	/// </summary>
	private static void Popup ()
	{
		FilterPopup fp = ScriptableObject.CreateInstance (typeof(FilterPopup)) as FilterPopup;

		int minHeight = mItems.Length * MEMBER_HEIGHT + MEMBER_HEIGHT * 2;
		int bestHeight = (int)(Screen.currentResolution.height / 2.5f);
		if (minHeight > bestHeight) minHeight = bestHeight;
        
		inspectorPos = GUIUtility.GUIToScreenPoint (new Vector2 (inspectorRect.x, inspectorRect.y));
        Rect rect = new Rect(new Vector2(inspectorPos.x + offsetLeft,inspectorPos.y) , inspectorRect.size);
        float minWidth = mCustomWidth > 0 ? mCustomWidth : inspectorRect.width;
        if (minWidth < 100) minWidth = 100;

        Vector2 vector2 = new Vector2(minWidth, minHeight);
        fp.ShowAsDropDown(rect,vector2);
	}

	/// <summary>
	/// Browse for field/property
	/// </summary>
	/// <param name="binderEditor"></param>
	/// <param name="field"></param>
	static public void Browse (string[] items, Action<string> onSelected, Action<int> onSelectedIndex,float offset = 0,float customWidth = 0)
	{
        SetShowPosition();
        offsetLeft = offset;
        mCustomWidth = customWidth;
		searchField = new UISearchField (Filter, null, null);
		OnSelected = onSelected;
		OnSelectedIndex = onSelectedIndex;
		mItems = items;

        if (items != null && items.Length > 0)
        {
            Popup();
        }
    }

	private void OnGUI ()
	{
		if (Event.current.keyCode == KeyCode.Escape)
			Close ();

		if (mItems == null)
			return;

		if (listView == null)
			listView = new ListView ();

		//Search field
		searchField.Draw ();
		listView.SetData (mItems, true, OnSelected, searchField.KeyWord, this, OnSelectedIndex);
		listView.Draw ();
	}

	/// <summary>
	/// Set the window's rectangle
	/// </summary>
	/// <param name="rect"></param>
	static public void SetPopupRect (Rect rect)
	{
		inspectorRect = rect;
	}

    static public void SetShowPosition()
    {
        var lastRect = GUILayoutUtility.GetLastRect();
		SetPopupRect (new Rect (lastRect.x, Event.current.mousePosition.y, lastRect.width, 10));
	}

    /// <summary>
    /// Filter items by keyword
    /// </summary>
    /// <param name="keyWord"></param>
    static void Filter (string keyWord)
	{

	}

	new static public void Close ()
	{
		if (listView != null && listView.Window != null) listView.Window.Close ();
	}
}