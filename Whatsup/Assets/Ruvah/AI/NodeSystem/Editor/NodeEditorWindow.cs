using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
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


        // -- General
        public NodeObject SelectedObject
        {
            get => _SelectedObject;
            set
            {
                _SelectedObject = value;
                Selection.activeObject = _SelectedObject;
            }
        }

        public GenericMenu ContextMenu = new GenericMenu();
        public GenericMenu NodeMenu = new GenericMenu();
        public NodeEditorState CurrentState;
        public NodeSystem EditedSystem;
        public float Zoom = 1f;
        public float MinZoom = 1f;
        public float MaxZoom = 3f;

        protected Vector2 MousePosition { get; private set; }

        private const float ScrollDeltaModifier = 0.1f;
        private NodeObject _SelectedObject;
        private List<BaseConnection> Connections = new List<BaseConnection>();

        private Rect NodeViewRect = new Rect(0,0,float.MaxValue,float.MaxValue);

        private BaseConnection ConnectionInCreation;
        private BaseNode ConnectionFromNode;

        [SerializeField]
        private EditorGUISplitView HorizontalSplitView = new EditorGUISplitView (EditorGUISplitView.Direction.Horizontal);
        [SerializeField]
        private Vector2 NodeViewScrollPos = new Vector2(0.5f,0.5f);

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
                if (node.WindowRect.Contains(MousePosition))
                {
                    SelectedObject = node;
                    switch (e.button)
                    {
                        case 0:
                        {
                            LeftClickNode(node);
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

            foreach (var connection in Connections)
            {
                if (connection.Contains(MousePosition))
                {
                    SelectedObject = connection;
                    switch (e.button)
                    {
                        case 0:
                        {
                            LeftClickConnection(connection);
                            break;
                        }
                        case 1:
                        {

                            break;
                        }

                    }
                    return;
                }
            }

            if (CurrentState == NodeEditorState.CreatingConnection)
            {
                CancelConnectionCreation();
            }
            if (e.button == 1 && HorizontalSplitView.View2Rect.Contains(MousePosition))
            {
                ContextMenu.ShowAsContext();
            }
        }

        protected void Initialize()
        {
            CreateContextMenu();
            CreateNodeMenu();
            Connections.Clear();
            if (EditedSystem == null) { return; }
            foreach (var node in EditedSystem.NodesList)
            {
                Connections.AddRange(node.Connections);
            }
        }

        private void CancelConnectionCreation()
        {
            DestroyImmediate(ConnectionInCreation);
            ConnectionInCreation = null;
            CurrentState = NodeEditorState.None;
        }

        private void DrawNodes()
        {
            NodeViewScrollPos = GUI.BeginScrollView(HorizontalSplitView.View2Rect, NodeViewScrollPos, NodeViewRect);
            BeginWindows();
            for (var i = 0; i < EditedSystem.NodesList.Count; i++)
            {
                var node = EditedSystem.NodesList[i];
                node.WindowRect = GUI.Window(i, node.WindowRect, node.DrawContent, node.WindowTitle);
                node.WindowRect.x = Mathf.Clamp(node.WindowRect.x, 0, NodeViewRect.width - node.WindowRect.width);
                node.WindowRect.y = Mathf.Clamp(node.WindowRect.y, 0, NodeViewRect.height - node.WindowRect.height);
            }
            EndWindows();
            GUI.EndScrollView();
        }

        private void MouseEvent()
        {
            Event current_event = Event.current;
            MousePosition = current_event.mousePosition;
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
            ConnectionInCreation = connection;
            ConnectionFromNode = SelectedObject as BaseNode;
        }


        private void CompleteConnectionCreation(BaseNode to_node)
        {
            ConnectionInCreation.From = ConnectionFromNode;
            ConnectionInCreation.To = to_node;
            var created_connection = ConnectionFromNode.AddConnection(ConnectionInCreation);
            if (created_connection == ConnectionInCreation)
            {
                ConnectionInCreation.name = $"{ConnectionInCreation.From.name}_to_{to_node.name}";
                Connections.Add(ConnectionInCreation);
                AssetDatabase.AddObjectToAsset(ConnectionInCreation, ConnectionInCreation.From);
                EditorUtility.SetDirty(EditedSystem);
            }
            ConnectionInCreation = null;
            ConnectionFromNode = null;
            SelectedObject = created_connection;
            CurrentState = NodeEditorState.None;
        }

        private void LeftClickNode(BaseNode node)
        {
            switch (CurrentState)
            {
                case NodeEditorState.None:
                {

                    break;
                }

                case NodeEditorState.CreatingConnection:
                {
                    CompleteConnectionCreation(node);
                    break;
                }
            }
        }

        private void LeftClickConnection(BaseConnection connection)
        {
            switch (CurrentState)
            {
                case NodeEditorState.None:
                {
                    break;
                }

                case NodeEditorState.CreatingConnection:
                {
                    CancelConnectionCreation();
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
                ConnectionInCreation.DrawToMouse(ConnectionFromNode.GetBottom(), MousePosition);
            }
        }

        private void DrawNodeView()
        {
//            Matrix4x4 before = GUI.matrix;
//
//            Matrix4x4 Translation = Matrix4x4.TRS(new Vector3(0,25,0),Quaternion.identity,Vector3.one);
//            Matrix4x4 Scale = Matrix4x4.Scale(Zoom * Vector3.one);
//            GUI.matrix = Translation*Scale*Translation.inverse;

            MouseEvent();
            DrawConnections();
            DrawNodes();

            //GUI.matrix = before;
        }

        // -- UNITY

        private void OnGUI()
        {
            HorizontalSplitView.BeginSplitView();

            HorizontalSplitView.Split ();
            DrawNodeView();
            HorizontalSplitView.EndSplitView();
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
