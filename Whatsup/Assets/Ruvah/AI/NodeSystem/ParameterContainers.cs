using System;
using UnityEditor;
using UnityEngine;

// :HACK: not clean IMO, hack to have all these types stored in the same container AND HAVE THEM SERIALIZABLE
// System.Object is not serializable by Unity
namespace Ruvah.NodeSystem.ParameterContainers
{
    [Serializable]
    public abstract class ParameterContainer : ScriptableObject
    {
    }

    [Serializable]
    public class IntContainer: ParameterContainer
    {
        // -- FIELDS

        public int Value;

        // -- METHODS

        public void Initialize(string new_name, int value)
        {
            name = new_name;
            Value = value;
        }
    }

    [Serializable]
    public class FloatContainer: ParameterContainer
    {
        // -- FIELDS

        public float Value;

        // -- METHODS

        public void Initialize(string new_name, float value)
        {
            name = new_name;
            Value = value;
        }
    }

    [Serializable]
    public class BoolContainer: ParameterContainer
    {
        // -- FIELDS

        public bool Value;

        // -- METHODS

        public void Initialize(string new_name, bool value)
        {
            name = new_name;
            Value = value;
        }
    }

    [Serializable]
    public class GameObjectContainer: ParameterContainer
    {
        // -- FIELDS

        public GameObject Value;

        // -- METHODS

        public void Initialize(string new_name, GameObject value)
        {
            name = new_name;
            Value = value;
        }
    }

}

