using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorGUISplitView
{

	public enum Direction {
		Horizontal,
		Vertical
	}

	public Rect View1Rect
	{
		get
		{
			var rect = AvailableRect;
			if (SplitDirection == Direction.Horizontal)
			{
				rect.width = AvailableRect.width * SplitNormalizedPosition;
			}
			else
			{
				rect.height = AvailableRect.height * SplitNormalizedPosition;
			}
			return rect;
		}
	}

	public Rect View2Rect
	{
		get
		{
			var rect = AvailableRect;
			if (SplitDirection == Direction.Horizontal)
			{
				rect.xMin = AvailableRect.width * SplitNormalizedPosition;
			}
			else
			{
				rect.yMin = AvailableRect.height * SplitNormalizedPosition;
			}
			return rect;
		}
	}

	public Vector2 ScrollPosition;


	private const float HandleThickness = 2f;

	private Direction SplitDirection;
	private float SplitNormalizedPosition;
	private bool Resize;
	private Rect AvailableRect;


	public EditorGUISplitView(Direction splitDirection, float split_position = 0.5f) {
		SplitNormalizedPosition = split_position;
		this.SplitDirection = splitDirection;
	}

	public void BeginSplitView() {
		Rect tempRect;

		if(SplitDirection == Direction.Horizontal)
			tempRect = EditorGUILayout.BeginHorizontal (GUILayout.ExpandWidth(true));
		else
			tempRect = EditorGUILayout.BeginVertical (GUILayout.ExpandHeight(true));

		if (tempRect.width > 0.0f) {
			AvailableRect = tempRect;
		}
		if(SplitDirection == Direction.Horizontal)
			ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.Width(AvailableRect.width * SplitNormalizedPosition));
		else
			ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, GUILayout.Height(AvailableRect.height * SplitNormalizedPosition));
	}

	public void Split() {
		GUILayout.EndScrollView();
		ResizeSplitFirstView ();
	}

	public void EndSplitView() {

		if(SplitDirection == Direction.Horizontal)
			EditorGUILayout.EndHorizontal ();
		else
			EditorGUILayout.EndVertical ();
	}

	private void ResizeSplitFirstView(){

		Rect resizeHandleRect;

		if(SplitDirection == Direction.Horizontal)
			resizeHandleRect = new Rect (AvailableRect.width * SplitNormalizedPosition, AvailableRect.y, HandleThickness, AvailableRect.height);
		else
			resizeHandleRect = new Rect (AvailableRect.x,AvailableRect.height * SplitNormalizedPosition, AvailableRect.width, HandleThickness);

		GUI.DrawTexture(resizeHandleRect,EditorGUIUtility.whiteTexture);

		if(SplitDirection == Direction.Horizontal)
			EditorGUIUtility.AddCursorRect(resizeHandleRect,MouseCursor.ResizeHorizontal);
		else
			EditorGUIUtility.AddCursorRect(resizeHandleRect,MouseCursor.ResizeVertical);

		if( Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition)){
			Resize = true;
		}
		if(Resize){
			if(SplitDirection == Direction.Horizontal)
				SplitNormalizedPosition = Event.current.mousePosition.x / AvailableRect.width;
			else
				SplitNormalizedPosition = Event.current.mousePosition.y / AvailableRect.height;
		}
		if(Event.current.type == EventType.MouseUp)
			Resize = false;
	}
}

