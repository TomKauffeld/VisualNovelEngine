using NLua;
using System;
using System.Collections.Generic;
using VisualNovelEngine;

namespace VisualNovelLua
{
    class LuaScript<T> : IScript<T>
    {
        private readonly string code;
        public LuaScript(string code)
        {
            this.code = code;
        }

        public T Execute(Variables variables)
        {
            try
            {
                using (Lua state = new Lua())
                {
                    state.LoadCLRPackage();

                    state.DoString("variables = {}");

                    foreach (KeyValuePair<string, Variable> var in variables)
                    {
                        string line = string.Format("variables[\"{0}\"] = \"{1}\"\n", var.Key, var.Value.Value);
                        state.DoString(line);
                    }
                    LuaTable table = state["variables"] as LuaTable;

                    state.DoString(code);
                    if (!(state["Execute"] is LuaFunction function))
                        throw new Exception("couldn't load the function");


                    object[] returns = function.Call();
                    if (returns.Length < 1)
                        throw new Exception("script didn't return anything");


                    foreach (object key in table.Keys)
                    {
                        if (key is string k)
                        {
                            string value = table[key]?.ToString();
                            if (variables[k] == null || value != variables[k].Value)
                                variables[k] = new VariableString(value);
                        }
                    }

                    if (typeof(Variable).IsEquivalentTo(typeof(T)))
                    {
                        object o = new Variable(VariableType.STRING, returns[0] as string);
                        return (T)o;
                    }
                    return (T)returns[0];
                }
            }
            catch(Exception e)
            {
                throw new Exception("Couldn't execute the script", e);
            }
        }
    }
}
