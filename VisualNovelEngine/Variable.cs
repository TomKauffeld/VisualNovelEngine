using System;

namespace VisualNovelEngine
{
    public class Variable
    {

        public VariableType VarType { get; protected set; }

        private string value;

        public string Value { get => value; protected set => SetValue(value); }
        
        public float FloatValue { get => float.Parse(Value); protected set => SetValue(value.ToString()); }


        public Variable(VariableType type, string value)
        {
            VarType = type;
            Value = value;
        }

        private void SetValue(string value)
        {
            try
            {
                switch(VarType)
                {
                    case VariableType.STRING:
                        this.value = value;
                        break;
                    case VariableType.NUMBER:
                        this.value = float.Parse(value).ToString();
                        break;
                    default:
                        throw new Exception("unsupported type");
                }
            }
            catch(Exception e)
            {
                throw new Exception("invalid type", e);
            }
        }
    }
}
