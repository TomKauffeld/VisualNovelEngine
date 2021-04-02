namespace VisualNovelEngine
{
    public interface IScriptLoader<T>
    {
        IScript<T> LoadScript(string script_id);
    }
}
