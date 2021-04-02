using VisualNovelEngine;
using System.Collections.Generic;
using System;
using VisualNovelDirectory;
using System.IO;
using VisualNovelLua;

namespace TestVisualNovel
{
    class Program
    {
        private static readonly string SCRIPT_PATH = Path.Combine(AppContext.BaseDirectory, "res", "scripts");
        private static readonly string NODES_PATH = Path.Combine(AppContext.BaseDirectory, "res", "nodes");


        private static void ShowException(Exception exception)
        {
            if (exception == null)
                return;
            Console.WriteLine(exception.Message);
            ShowException(exception.InnerException);
        }

        static void Main(string[] args)
        {
            try
            {
                LuaScriptParser<string> scriptLuaStringParser = new LuaScriptParser<string>();
                LuaScriptParser<bool> scriptLuaBoolParser = new LuaScriptParser<bool>();

                GenericScriptParser<string> scriptStringParser = new GenericScriptParser<string>();
                GenericScriptParser<bool> scriptBoolParser = new GenericScriptParser<bool>();

                scriptStringParser.AddParser("lua", scriptLuaStringParser);
                scriptBoolParser.AddParser("lua", scriptLuaBoolParser);

                ScriptLoader<string> scriptStringLoader = new ScriptLoader<string>(SCRIPT_PATH, scriptStringParser);
                ScriptLoader<bool> scriptBoolLoader = new ScriptLoader<bool>(SCRIPT_PATH, scriptBoolParser);

                ChoiceLoader choiceLoader = new ChoiceLoader(NODES_PATH, scriptBoolLoader);
                NodeLoader nodeLoader = new NodeLoader(NODES_PATH, scriptStringLoader, choiceLoader);

                IDictionary<string, Node> nodes = nodeLoader.LoadNodes();

                Engine engine = new Engine(nodes, "start");

                while(!engine.IsFinished)
                {
                    engine.Render();
                    bool ok = false;
                    uint? choice = null;
                    while (!ok)
                    {
                        string line = Console.ReadLine();
                        if (engine.CurrentChoices.Count > 0 && uint.TryParse(line, out uint c) && engine.CurrentChoices.ContainsKey(c))
                        {
                            choice = c;
                            ok = true;
                        }
                        else if (engine.CurrentChoices.Count == 0)
                            ok = true;
                    }
                    engine.MakeChoice(choice);
                }
                Console.WriteLine("Done");

            }
            catch(Exception e)
            {
                ShowException(e);
            }
        }
    }
}
