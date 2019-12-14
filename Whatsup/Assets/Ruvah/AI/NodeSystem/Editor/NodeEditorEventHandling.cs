using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruvah.AI.NodeSystem
{
    public partial class NodeEditorWindow
    {
        // -- FIELDS

        
        public float Zoom;

        private const float ScrollDeltaModifier = 0.1f;
        private const float MinZoom = 0.5f;
        private const float MaxZoom = 1f;

        protected Vector2 MousePosition { get; private set; }
        protected Vector2 NodeViewMousePosition { get; private set; }
        protected bool IsMouseInNodeView { get; private set; }

        // -- METHODS


        private void HandleEvents()
        {
            Event current_event = Event.current;
            MousePosition = current_event.mousePosition;
            IsMouseInNodeView = HorizontalSplitView.View2Rect.Contains(MousePosition);
            NodeViewMousePosition = (MousePosition - HorizontalSplitView.View2Rect.position) /Zoom + NodeViewScrollPos;

            if (current_event.isMouse)
            {
                HandleMouseEvent(current_event);
            }
            else if (current_event.isScrollWheel)
            {
                HandleMouseScrollEvent(current_event);
            }
        }

        private void HandleMouseEvent(Event current_event)
        {
            switch (current_event.type)
            {
                case EventType.MouseDown:
                {
                    HandleMouseClick(current_event);
                    break;
                }
                case EventType.MouseDrag:
                {
                    HandleMouseDrag(current_event);
                    break;
                }
            }
        }

        private void HandleMouseClick(Event current_event)
        {
            if(IsMouseInNodeView)
            {
                HandleMouseClickNodeView(current_event);
            }

            if (CurrentState == NodeEditorState.CreatingConnection)
            {
                CancelConnectionCreation();
            }
        }

        private void HandleMouseScrollEvent(Event current_event)
        {
            if (!IsMouseInNodeView)
            {
                return;
            }

            Zoom -= Time.deltaTime * current_event.delta.y * ScrollDeltaModifier;
            Zoom = Mathf.Clamp(Zoom, MinZoom, MaxZoom);
            current_event.Use();
        }

        private void HandleMouseClickNodeView(Event current_event)
        {
            bool has_clicked_node = MouseClickNodes(current_event);
            if (has_clicked_node)
            {
                return;
            }

            bool has_clicked_connection = MouseClickConnections(current_event);
            if (has_clicked_connection)
            {
                return;
            }

            MouseClickNodeViewField(current_event);
        }

        private bool MouseClickNodes(Event current_event)
        {
            var node = GetMouseOverNode();
            if (node == null)
            {
                return false;
            }

            SelectedObject = node;

            switch (current_event.button)
            {
                case Constants.LeftMouseButton:
                {
                    LeftClickNode(node);
                    break;
                }
                case Constants.RightMouseButton:
                {
                    NodeMenu.ShowAsContext();
                    break;
                }
            }
            return true;

        }

        private BaseNode GetMouseOverNode()
        {
            foreach (var node in EditedSystem.NodesList)
            {
                if (node.WindowRect.Contains(NodeViewMousePosition))
                {
                    return node;
                }
            }
            return null;
        }

        private BaseConnection GetMouseOverConnection()
        {
            foreach (var connection in Connections)
            {
                if (connection.Contains(NodeViewMousePosition))
                {
                    return connection;
                }
            }
            return null;
        }

        private bool MouseClickConnections(Event current_event)
        {
            var connection = GetMouseOverConnection();
            if (connection == null)
            {
                return false;
            }

            SelectedObject = connection;
            switch (current_event.button)
            {
                case Constants.LeftMouseButton:
                {
                    LeftClickConnection(connection);
                    break;
                }
            }

            return true;
        }

        private void MouseClickNodeViewField(Event current_event)
        {
            switch (current_event.button)
            {
                case Constants.RightMouseButton:
                {
                    NodeViewContextMenu.ShowAsContext();
                    current_event.Use();
                    break;
                }
            }
        }

        private void HandleMouseDrag(Event current_event)
        {
            if (
                !IsMouseInNodeView
                || current_event.button != Constants.ScrollWheelButton
                )
            {
                return;
            }

            NodeViewScrollPos -= current_event.delta;
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
    }
}
