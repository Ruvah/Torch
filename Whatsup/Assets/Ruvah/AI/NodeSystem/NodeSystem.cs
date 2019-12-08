using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruvah.NodeSystem
{
    // -- TYPES

    [Serializable]
    public class Variable
    {
        public Variable(Type type, string name, object value)
        {
            Type = type;
            Name = name;
            Value = value;
        }

        public Type Type;
        public string Name;
        public object Value;
    }

    // -- FIELDS

    [Serializable]
    public class NodeSystem : ScriptableObject
    {
        public List<BaseNode> NodesList = new List<BaseNode>();
        public List<Variable> Variables = new List<Variable>();
    }
}
