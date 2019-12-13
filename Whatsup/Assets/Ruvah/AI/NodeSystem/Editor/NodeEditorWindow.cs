using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Ruvah.AI.NodeSystem
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
        public float MinZoom = 0.5f;
        public float MaxZoom = 1f;

        protected GUIStyle NodeViewScrollbarStyle = new GUIStyle();

        private const float ScrollDeltaModifier = 0.1f;
        private NodeObject _SelectedObject;
        private List<BaseConnection> Connections = new List<BaseConnection>();

        private Rect NodeViewRect = new Rect(0,0,10000,10000);

        private BaseConnection ConnectionInCreation;
        private BaseNode ConnectionFromNode;

        private Matrix4x4 RegularMatrix;

        [SerializeField] private EditorGUISplitView HorizontalSplitView = new EditorGUISplitView (EditorGUISplitView.Direction.Horizontal,0.3f);
        [SerializeField] private Vector2 NodeViewScrollPos = new Vector2(5000,5000);

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
            StartNodeViewZoom();

            var rect = HorizontalSplitView.View2Rect;
            rect.size /= Zoom;
            NodeViewScrollPos = GUI.BeginScrollView(rect, NodeViewScrollPos, NodeViewRect, NodeViewScrollbarStyle, NodeViewScrollbarStyle);
            DrawConnections();
            DrawNodes();
            GUI.EndScrollView();

            EndNodeViewZoom();
        }

        private void StartNodeViewZoom()
        {
            GUI.EndGroup();
            Rect rect = new Rect(0, Constants.EditorConstants.EditorTabHeight, Screen.width / Zoom, Screen.height / Zoom);
            GUI.BeginGroup(rect);
            RegularMatrix = GUI.matrix;
            Matrix4x4 transl = Matrix4x4.TRS(new Vector3(HorizontalSplitView.View2Rect.position.x , Constants.EditorConstants.EditorTabHeight, 1), Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(Zoom, Zoom, Zoom));
            GUI.matrix = transl * scale * transl.inverse;
        }

        private void EndNodeViewZoom()
        {
            GUI.matrix = RegularMatrix;
            GUI.EndGroup();
            Rect rect = new Rect(0, Constants.EditorConstants.EditorTabHeight, Screen.width, Screen.height);
            GUI.BeginGroup(rect);
        }

        // -- UNITY

        private void OnGUI()
        {
            HandleEvents();
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
