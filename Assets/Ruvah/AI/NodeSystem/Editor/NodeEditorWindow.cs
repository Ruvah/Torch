using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ruvah.NodeSystem
{

    public abstract class NodeEditorWindow : EditorWindow
    {
        public enum NodeEditorState
        {
            None,
            CreatingConnection,
            Count
        }
        
        // -- FIELDS
        
        
        protected GenericMenu ContextMenu = new GenericMenu();
        protected GenericMenu NodeMenu = new GenericMenu();

        protected NodeEditorState CurrentState;

        private NodeObject SelectedObject;
        
        private Rect ContentRect;
        private static Color BackgroundColor = new Color(0.3f,0.3f,0.3f);
        private static Texture2D BackgroundTexture;

        protected static NodeSystem EditedSystem;
        protected Vector2 MousePos { get; private set; }


        // -- METHODS

        protected virtual void CreateContextMenu()
        {
            
        }

        protected virtual void CreateNodeMenu()
        {
            NodeMenu.AddItem(new GUIContent("CreateTransition"), false, StartConnection);
        }

        private void HandleMouseClick(Event e)
        {
            foreach (var node in EditedSystem.NodesList)
            {
                if (node.WindowRect.Contains(MousePos))
                {
                    SelectNode(node);
                    node.OnClicked(MousePos);
                    switch (e.button)
                    {
                        case 0:
                        {

                            break;
                        }
                        case 1:
                        {
                            NodeMenu.ShowAsContext();
                            break;
                        }

                    }
                    return;
                }
            }

            if (CurrentState == NodeEditorState.CreatingConnection)
            {
                SelectedObject = null;
                CurrentState = NodeEditorState.None;
            }
            if (e.button == 1)
            {
                ContextMenu.ShowAsContext();
            }
        }

        private void DrawNodes()
        {
            BeginWindows();
            for (var i = 0; i < EditedSystem.NodesList.Count; i++)
            {
                var node = EditedSystem.NodesList[i];
                node.WindowRect = GUI.Window(i, node.WindowRect, node.DrawWindow, node.WindowTitle);
            }
            EndWindows();
        }

        private void MouseEvent()
        {
            Event current_event = Event.current;
            MousePos = current_event.mousePosition;
            if (current_event.isMouse)
            {
                HandleMouseClick(current_event);
            }
        }

        private void StartConnection()
        {
            CurrentState = NodeEditorState.CreatingConnection;
            var connection = new BaseConnection
            {
                From = SelectedObject as BaseNode
            };
            SelectedObject = connection;
        }

        private void SelectNode(BaseNode node)
        {
            switch (CurrentState)
            {
                case NodeEditorState.None:
                {
                    SelectedObject = node;
                    break;
                }

                case NodeEditorState.CreatingConnection:
                {
                    var connection = SelectedObject as BaseConnection;
                    connection.To = node;
                    connection.Apply();
                    CurrentState = NodeEditorState.None;
                    break;
                }
            }
        }

        private void DrawConnections()
        {
            foreach (var node in EditedSystem.NodesList)
            {
                node.DrawConnections();
            }
            if (CurrentState == NodeEditorState.CreatingConnection)
            {
                (SelectedObject as BaseConnection)?.DrawToMouse(MousePos);
            }
        }

        // -- UNITY

        private void OnGUI()
        {
            ContentRect.max = maxSize;
            GUI.DrawTexture(ContentRect,BackgroundTexture, ScaleMode.StretchToFill);
            
            MouseEvent();
            DrawConnections();
            DrawNodes();
        }

        protected virtual void Awake()
        {
            BackgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            BackgroundTexture.SetPixel(0, 0, BackgroundColor);
            BackgroundTexture.Apply();
            CreateContextMenu();
            CreateNodeMenu();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }
    }

}
