using System;
using VisualNovelEngine;

namespace VisualNovelLua
{
    public class LuaScriptParser<T> : IScriptParser<T>
    {
        public IScript<T> Parse(string code, string type)
        {
            try
            {
                if (!type.ToLower().Equals("lua"))
                    throw new Exception(string.Format("Code isn't lua but {0}", type));
                return new LuaScript<T>(code);
            }
            catch(Exception e)
            {
                throw new Exception("Couldn't parse the lua script", e);
            }
        }
    }
}
