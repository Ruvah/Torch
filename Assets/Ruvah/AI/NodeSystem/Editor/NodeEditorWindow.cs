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
        
        
        public GenericMenu ContextMenu = new GenericMenu();
        public GenericMenu NodeMenu = new GenericMenu();

        public NodeEditorState CurrentState;

        public NodeObject SelectedObject;
        
        public Rect ContentRect;
        public Color BackgroundColor = new Color(0.3f,0.3f,0.3f);
        public Texture2D BackgroundTexture;
        
        public NodeSystem EditedSystem;
        public float Zoom = 1f;
        public float MinZoom = 1f;
        public float MaxZoom = 3f;

        private const float ScrollDeltaModifier = 0.1f;
        
        protected Vector2 MousePos { get; private set; }

        // -- METHODS

        protected virtual void CreateContextMenu()
        {
            ContextMenu = new GenericMenu();
        }

        protected virtual void CreateNodeMenu()
        {
            NodeMenu = new GenericMenu();
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
        
        protected void Initialize()
        {
            BackgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            BackgroundTexture.SetPixel(0, 0, BackgroundColor);
            BackgroundTexture.Apply();
            CreateContextMenu();
            CreateNodeMenu();
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
            if (current_event.isScrollWheel)
            {
                Zoom -= Time.deltaTime * current_event.delta.y * ScrollDeltaModifier;
                Zoom = Mathf.Clamp(Zoom, MinZoom, MaxZoom);
            }
            else if (current_event.isMouse)
            {
                HandleMouseClick(current_event);
                
            }
        }

        private void StartConnection()
        {
            CurrentState = NodeEditorState.CreatingConnection;
            var connection = CreateInstance<BaseConnection>();
            connection.From = SelectedObject as BaseNode;
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
                    connection.name = $"{connection.From.name}_to_{connection.To.name}";
                    AssetDatabase.AddObjectToAsset(connection, EditedSystem);
                    EditorUtility.SetDirty(EditedSystem);
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
                var connection = SelectedObject as BaseConnection;
                connection.DrawToMouse(connection.From.GetBottom(), MousePos);
            }
        }

        private void Draw()
        {
            MouseEvent();
            DrawConnections();
            DrawNodes();
        }

        // -- UNITY

        private void OnGUI()
        {
            ContentRect.max = maxSize;
            GUI.DrawTexture(ContentRect,BackgroundTexture, ScaleMode.StretchToFill);
            
            Matrix4x4 before = GUI.matrix;
            //Scale my gui matrix
            Matrix4x4 Translation = Matrix4x4.TRS(new Vector3(0,25,0),Quaternion.identity,Vector3.one);
            Matrix4x4 Scale = Matrix4x4.Scale(Zoom * Vector3.one);
            GUI.matrix = Translation*Scale*Translation.inverse;
 
            //Draw my zoomed GUI
            Draw();
 
            //reset the matrix
            GUI.matrix = before;
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnEnable()
        {
            Initialize();
        }
    }

}
