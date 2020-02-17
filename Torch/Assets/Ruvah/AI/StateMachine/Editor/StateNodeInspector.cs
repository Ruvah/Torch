using Ruvah.AI.NodeSystem;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace Ruvah.AI.Statemachine
{
    [CustomEditor(typeof(StateNode))]
    public class StateNodeInspector : Editor
    {
        // -- FIELDS

        private GUIStyle headerStyle;
        private Color headerBackGroundColor = new Color(0.25f, 0.25f, 0.25f);
        private GUILayoutOption[] headerLabelOptions = {GUILayout.ExpandWidth(false), GUILayout.Width(40)};
        private ReorderableList connectionsList;
        private StateNode node;


        private void ConnectionsList_DrawElementCallback(Rect rect, int index, bool is_active, bool is_focused)
        {
            var connection = (BaseConnection) connectionsList.list[index];
            rect.y += 2;
            EditorGUI.LabelField(rect, $"{connection.From.name} -> {connection.To.name}");
        }

        private void ConnectionsList_DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Connections");
        }

        // -- UNITY

        private void OnEnable()
        {
            node = target as StateNode;
            headerStyle = new GUIStyle();
            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, headerBackGroundColor);
            tex.Apply();
            headerStyle.normal.background = tex;
            headerStyle.margin.bottom = 5;

            connectionsList = new ReorderableList(node.Connections, typeof(BaseConnection), draggable: true, displayHeader: true, displayAddButton: false, displayRemoveButton: true);
            connectionsList.drawElementCallback += ConnectionsList_DrawElementCallback;
            connectionsList.drawHeaderCallback += ConnectionsList_DrawHeaderCallback;
        }



        protected override void OnHeaderGUI()
        {
            GUILayout.BeginHorizontal(headerStyle);
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Name", headerLabelOptions);
                        target.name = EditorGUILayout.TextField(target.name);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(20f);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            connectionsList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
