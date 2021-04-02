using System;
using System.Collections.Generic;

namespace VisualNovelEngine
{
    public class GenericScriptParser<T> : IScriptParser<T>
    {
        private readonly IDictionary<string, IScriptParser<T>> parsers = new Dictionary<string, IScriptParser<T>>();

        public void AddParser(string type, IScriptParser<T> parser)
        {
            if (parsers.ContainsKey(type.ToLower()))
                parsers[type.ToLower()] = parser;
            else
                parsers.Add(type.ToLower(), parser);
        }

        private IScriptParser<T> GetParser(string type) => parsers.ContainsKey(type.ToLower()) ? parsers[type.ToLower()] : null;

        public IScript<T> Parse(string code, string type)
        {
            try
            {
                IScriptParser<T> parser = GetParser(type);
                if (parser == null)
                    throw new Exception(string.Format("Unsupported script type {0}", type));
                return parser.Parse(code, type);
            }
            catch (Exception e)
            {
                throw new Exception("Couldn't parse the script", e);
            }
        }
    }
}
