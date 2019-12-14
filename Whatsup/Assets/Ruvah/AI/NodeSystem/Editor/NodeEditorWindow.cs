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

        public GenericMenu NodeViewContextMenu = new GenericMenu();
        public GenericMenu NodeMenu = new GenericMenu();
        public NodeEditorState CurrentState;
        public NodeSystem EditedSystem;

        private const float ScrollDeltaModifier = 0.1f;
        private NodeObject _SelectedObject;
        private List<BaseConnection> Connections = new List<BaseConnection>();

        private BaseConnection ConnectionInCreation;
        private BaseNode ConnectionFromNode;


        [SerializeField] private EditorGUISplitView HorizontalSplitView = new EditorGUISplitView (EditorGUISplitView.Direction.Horizontal,0.3f);

        // -- METHODS

        public abstract void SetWindowTitle();


        protected virtual void CreateNodeViewContextMenu()
        {
            NodeViewContextMenu = new GenericMenu();
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

            InitializeConnections();
            InitalizeNodeView();
            InitializeVariablesView();
            CreateNodeViewContextMenu();
            CreateNodeMenu();
        }

        private void CancelConnectionCreation()
        {
            DestroyImmediate(ConnectionInCreation);
            ConnectionInCreation = null;
            CurrentState = NodeEditorState.None;
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

        private void InitializeConnections()
        {
            Connections.Clear();
            if (EditedSystem == null) { return; }
            foreach (var node in EditedSystem.NodesList)
            {
                Connections.AddRange(node.Connections);
                ConnectionInCreation = null;
            }
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
            SetWindowTitle();
        }
    }

}
