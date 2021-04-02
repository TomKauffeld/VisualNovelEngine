namespace VisualNovelEngine
{
    public class VariableFloat : Variable
    {
        public VariableFloat(float value) : base(VariableType.NUMBER, value.ToString()) { }
    }
}
