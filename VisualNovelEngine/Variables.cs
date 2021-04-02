using System.Collections;
using System.Collections.Generic;

namespace VisualNovelEngine
{
    public class Variables : IEnumerable<KeyValuePair<string, Variable>>
    {
        private readonly IDictionary<string, Variable> variables = new Dictionary<string, Variable>();


        public Variable this[string key] { get => GetVariable(key); set => SetVariable(key, value); }

        public IEnumerator<KeyValuePair<string, Variable>> GetEnumerator() => variables.GetEnumerator();

        public Variable GetVariable(string key) => variables.ContainsKey(key) ? variables[key] : null;

        public void SetVariable(string key, Variable value)
        {
            if (variables.ContainsKey(key))
                variables[key] = value;
            else
                variables.Add(key, value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)variables).GetEnumerator();
        }
    }
}
