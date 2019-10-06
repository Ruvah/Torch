using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ruvah.NodeSystem
{

    public abstract class BaseNode : ScriptableObject
    {
        // -- FIELDS
        
        public Rect WindowRect = new Rect(0,0,200,100);
        public string WindowTitle;
            
        // -- METHODS


        public Vector2 GetMiddle()
        {
            Vector2 middle;
            middle.x = WindowRect.x + WindowRect.width * 0.5f;
            middle.y = WindowRect.yMin + WindowRect.height * 0.5f;

            return middle;
        }

        public Vector2 GetBottom()
        {
            Vector2 bottom;
            bottom.x = WindowRect.x + WindowRect.width * 0.5f;
            bottom.y = WindowRect.yMin;

            return bottom;
        }
        
        public Vector2 GetTop()
        {
            Vector2 top;
            top.x = WindowRect.x + WindowRect.width * 0.5f;
            top.y = WindowRect.yMax;

            return top;
        }

        public abstract void DrawConnections();

        public virtual void DrawWindow()
        {
            WindowTitle = EditorGUILayout.TextField("Title", WindowTitle);
        }
        
        public virtual void OnClicked(Vector2 mouse_pos)
        {
            
        }


    }
}
