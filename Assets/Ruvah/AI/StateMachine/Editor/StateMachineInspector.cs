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
            if(GUILayout.Button("Open Editor"))
            {
                StateMachineWindow.Init(target as StateMachine);
            }
        }
    }
}
