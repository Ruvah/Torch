using System;
using UnityEngine;

namespace Ruvah.AI.NodeSystem.ParameterContainers
{
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
