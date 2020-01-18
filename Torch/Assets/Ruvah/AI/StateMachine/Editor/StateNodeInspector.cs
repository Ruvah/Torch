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

        private const string ConnectionsProperty = "Connections";

        private GUIStyle HeaderStyle;
        private Color HeaderBackGroundColor = new Color(0.25f, 0.25f, 0.25f);
        private GUILayoutOption[] HeaderLabelOptions = new[] {GUILayout.ExpandWidth(false), GUILayout.Width(40)};
        private ReorderableList TransitionsList;

        // -- UNITY

        private void OnEnable()
        {
            HeaderStyle = new GUIStyle();
            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, HeaderBackGroundColor);
            tex.Apply();
            HeaderStyle.normal.background = tex;
            HeaderStyle.margin.bottom = 5;

            TransitionsList = new ReorderableList(serializedObject, serializedObject.FindProperty(ConnectionsProperty));
        }

        protected override void OnHeaderGUI()
        {
            GUILayout.BeginHorizontal(HeaderStyle);
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Name", HeaderLabelOptions);
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
            TransitionsList.DoLayoutList();
        }
    }
}
