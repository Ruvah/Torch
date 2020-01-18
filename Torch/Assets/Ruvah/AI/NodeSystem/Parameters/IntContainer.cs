using System;
namespace Ruvah.AI.NodeSystem.ParameterContainers
{
    [Serializable]
    public class IntContainer : ParameterContainer
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
}

