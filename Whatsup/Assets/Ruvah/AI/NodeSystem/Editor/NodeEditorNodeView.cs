using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruvah.AI.NodeSystem
{

    public partial class NodeEditorWindow
    {
        // -- FIELDS

        protected GUIStyle NodeViewScrollbarStyle = new GUIStyle();
        private Rect NodeViewRect = new Rect(0,0,10000,10000);
        private Matrix4x4 RegularMatrix;

        [SerializeField] private Vector2 NodeViewScrollPos = new Vector2(5000,5000);

        // -- METHODS

        private void DrawNodeView()
        {
            StartNodeViewZoom();

            var rect = HorizontalSplitView.View2Rect;
            rect.size /= Zoom;
            NodeViewScrollPos = GUI.BeginScrollView(rect, NodeViewScrollPos, NodeViewRect, NodeViewScrollbarStyle,
                NodeViewScrollbarStyle);
            DrawConnections();
            DrawNodes();
            GUI.EndScrollView();

            EndNodeViewZoom();
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

        private void StartNodeViewZoom()
        {
            GUI.EndGroup();
            Rect rect = new Rect(0, Constants.EditorConstants.EditorTabHeight, Screen.width / Zoom,
                Screen.height / Zoom);
            GUI.BeginGroup(rect);
            RegularMatrix = GUI.matrix;
            Matrix4x4 transl =
                Matrix4x4.TRS(
                    new Vector3(HorizontalSplitView.View2Rect.position.x, Constants.EditorConstants.EditorTabHeight, 1),
                    Quaternion.identity, Vector3.one);
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

        private void DeleteNode()
        {

        }

    }
}
