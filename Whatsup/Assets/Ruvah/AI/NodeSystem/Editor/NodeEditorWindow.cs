using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Ruvah.NodeSystem
{

    public abstract partial class NodeEditorWindow : EditorWindow
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
        protected Vector2 NodeViewMousePosition { get; private set; }
        protected bool IsMouseInNodeView { get; private set; }

        private const float ScrollDeltaModifier = 0.1f;
        private NodeObject _SelectedObject;
        private List<BaseConnection> Connections = new List<BaseConnection>();

        private Rect NodeViewRect = new Rect(0,0,10000,10000);

        private BaseConnection ConnectionInCreation;
        private BaseNode ConnectionFromNode;

        [SerializeField] private EditorGUISplitView HorizontalSplitView = new EditorGUISplitView (EditorGUISplitView.Direction.Horizontal,0.3f);
        [SerializeField] private Vector2 NodeViewScrollPos = Vector2.zero;

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
            if(IsMouseInNodeView)
            {
                HandleMousClickNodeView(e);
            }

            if (CurrentState == NodeEditorState.CreatingConnection)
            {
                CancelConnectionCreation();
            }
        }

        private void HandleMousClickNodeView(Event e)
        {
            foreach (var node in EditedSystem.NodesList)
            {
                if (node.WindowRect.Contains(NodeViewMousePosition))
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
                if (connection.Contains(NodeViewMousePosition))
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

            if (e.button == Constants.RightMouseButton)
            {
                ContextMenu.ShowAsContext();
            }
        }

        protected void Initialize()
        {
            if (Selection.activeObject is NodeSystem system)
            {
                EditedSystem = system;
            }

            InitializeVariablesView();
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
            BeginWindows();
            for (var i = 0; i < EditedSystem.NodesList.Count; i++)
            {
                var node = EditedSystem.NodesList[i];
                node.WindowRect = GUI.Window(i, node.WindowRect, node.DrawContent, node.WindowTitle);
                node.WindowRect.x = Mathf.Clamp(node.WindowRect.x, 0, NodeViewRect.width - node.WindowRect.width);
                node.WindowRect.y = Mathf.Clamp(node.WindowRect.y, 0, NodeViewRect.height - node.WindowRect.height);
            }
            EndWindows();
        }

        private void MouseEvent()
        {
            Event current_event = Event.current;
            MousePosition = current_event.mousePosition;
            IsMouseInNodeView = HorizontalSplitView.View2Rect.Contains(MousePosition);
            NodeViewMousePosition = MousePosition - HorizontalSplitView.View2Rect.position + NodeViewScrollPos;
            if (IsMouseInNodeView && current_event.isScrollWheel)
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
            if (IsMouseInNodeView && CurrentState == NodeEditorState.CreatingConnection)
            {
                ConnectionInCreation.DrawToMouse(ConnectionFromNode.GetBottom(), NodeViewMousePosition);
            }
        }

        private void DrawNodeView()
        {

            MouseEvent();
            NodeViewScrollPos = GUI.BeginScrollView(HorizontalSplitView.View2Rect, NodeViewScrollPos, NodeViewRect);
            DrawConnections();
            DrawNodes();
            GUI.EndScrollView();
        }

        // -- UNITY

        private void OnGUI()
        {
            HorizontalSplitView.BeginSplitView();
            DrawVariablesView();
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
