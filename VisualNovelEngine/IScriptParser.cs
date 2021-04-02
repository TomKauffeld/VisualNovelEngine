namespace VisualNovelEngine
{
    public interface IScriptParser<T>
    {
        IScript<T> Parse(string code, string type);
    }
}
