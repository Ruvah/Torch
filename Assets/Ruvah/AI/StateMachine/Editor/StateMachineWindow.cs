using System;
using System.Collections;
using System.Collections.Generic;
using Ruvah.NodeSystem;
using UnityEditor;
using UnityEngine;

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

        public static void Init(StateMachine state_machine)
        {
            StateMachineWindow window = (StateMachineWindow) EditorWindow.GetWindow(typeof(StateMachineWindow));
            window.EditedSystem = state_machine;
            EditorUtility.SetDirty(window);
            window.Show();
        }


        protected override void CreateContextMenu()
        {
            ContextMenu.AddItem(new GUIContent("AddState"), false, ContextMenuOption,
                StateMachineContextOptions.CreateState);
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
            var new_state = new StateNode();
            new_state.WindowRect.center = MousePos;
            EditedSystem.NodesList.Add(new_state);
            EditorUtility.SetDirty(EditedSystem);
        }
    }
    
}
