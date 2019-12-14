using System;
using System.Collections;
using System.Collections.Generic;
using Ruvah.AI.NodeSystem;
using UnityEditor;
using UnityEngine;
using EditorGUIUtility = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUIUtility;

namespace Ruvah.AI.Statemachine
{
    [Serializable]
    public class StateMachineWindow : NodeEditorWindow
    {
        public enum StateMachineContextOptions
        {
            CreateState,
        }

        // -- FIELDS

        // -- METHODS

        public static void Init()
        {
            StateMachineWindow window = (StateMachineWindow) GetWindow(typeof(StateMachineWindow));
            window.Initialize();
            EditorUtility.SetDirty(window);
            window.Show();
        }


        public override void SetWindowTitle()
        {
            titleContent = new GUIContent("StateMachine");
        }

        protected override void CreateNodeViewContextMenu()
        {
            base.CreateNodeViewContextMenu();
            NodeViewContextMenu.AddItem(new GUIContent("AddState"), false, ContextMenuOption,
                StateMachineContextOptions.CreateState);
        }

        protected override void CreateNodeMenu()
        {
            base.CreateNodeMenu();
            NodeMenu.AddItem(new GUIContent("Remove"), false, DeleteState);
        }

        protected void ContextMenuOption(object option)
        {
            var selected_option = (StateMachineContextOptions) option;
            switch (selected_option)
            {
                case StateMachineContextOptions.CreateState:
                {
                    CreateState();
                    break;
                }
            }
        }

        private void CreateState()
        {
            var new_state = CreateInstance<StateNode>();
            new_state.WindowRect.center = NodeViewMousePosition;
            new_state.name = new_state.WindowTitle;
            new_state.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(new_state, EditedSystem);
            EditedSystem.NodesList.Add(new_state);
            EditorUtility.SetDirty(EditedSystem);
        }

        private void DeleteState()
        {
            if (!(SelectedObject is StateNode state))
            {
                return;
            }

            EditedSystem.NodesList.Remove(state);
            AssetDatabase.RemoveObjectFromAsset(state);
            EditorUtility.SetDirty(EditedSystem);
            DestroyImmediate(state);
        }
    }

}
