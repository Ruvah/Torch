using System;

namespace Ruvah.AI.NodeSystem.ParameterContainers
{
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
    }
