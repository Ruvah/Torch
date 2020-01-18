using System;

namespace Ruvah.AI.NodeSystem.ParameterContainers
{
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

}
