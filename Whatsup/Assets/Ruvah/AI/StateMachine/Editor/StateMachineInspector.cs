using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ruvah.AI.Statemachine
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineInspector : Editor {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Open Editor"))
            {
                StateMachineWindow.Init();
            }
        }
    }
}
