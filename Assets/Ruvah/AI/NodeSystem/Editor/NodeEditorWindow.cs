using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ruvah.NodeSystem
{

    public abstract class NodeEditorWindow : EditorWindow
    {
        // -- FIELDS
        
        
        protected GenericMenu ContextMenu = new GenericMenu();

        private BaseNode SelectedNode;
        
        private Rect ContentRect = new Rect();
        private static Color BackgroundColor = new Color(0.3f,0.3f,0.3f);
        private static Texture2D BackgroundTexture;

        // -- METHODS
        

        public static void DrawNodeConnection(Vector2 start, Vector2 end)
        {
            Handles.DrawLine(start,end);
        }

        protected virtual void CreateContextMenu()
        {
            
        }

        private void HandleMouseClick(Vector2 mouse_pos)
        {
            foreach (var node in NodesList)
            {
                if (node.WindowRect.Contains(mouse_pos))
                {
                    node.OnClicked(mouse_pos);
                    break;
                }
            }
            
            ContextMenu.ShowAsContext();
        }

        protected virtual void ContextMenuOption(object option)
        {
            
        }

        // -- UNITY

        private void OnGUI()
        {
            ContentRect.max = maxSize;
            GUI.DrawTexture(ContentRect,BackgroundTexture, ScaleMode.StretchToFill);
            
            //TODO: refactor this
            Event current_event = Event.current;
            if (current_event.isMouse && current_event.button == 1)
            {
                HandleMouseClick(current_event.mousePosition);
                current_event.Use();
            }
        }

        protected virtual void Awake()
        {
            BackgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            BackgroundTexture.SetPixel(0, 0, BackgroundColor);
            BackgroundTexture.Apply();
            CreateContextMenu();
        }
    }
    
}
