using System;
using System.Collections.Generic;
using System.IO;
using VisualNovelConsole;
using VisualNovelEngine;

namespace VisualNovelDirectory
{
    public class NodeLoader : INodeLoader
    {
        private readonly DirectoryInfo directory;
        private readonly IChoiceLoader choiceLoader;
        private readonly IScriptLoader<string> scriptLoader;

        public NodeLoader(string directory, IScriptLoader<string> scriptLoader, IChoiceLoader choiceLoader)
        {
            this.choiceLoader = choiceLoader;
            this.scriptLoader = scriptLoader;
            this.directory = new DirectoryInfo(directory);
        }

        public IList<string> NodeIds()
        {
            try
            {
                List<string> nodes = new List<string>();
                foreach (DirectoryInfo info in directory.EnumerateDirectories())
                    nodes.Add(info.Name);
                return nodes;
            }
            catch(Exception e)
            {
                throw new Exception("Couldn't load the node ids", e);
            }
        }

        public Node LoadNode(string node_id)
        {
            try
            {
                DirectoryInfo nodeDir = new DirectoryInfo(Path.Combine(directory.FullName, node_id));
                string path = nodeDir.FullName;
                FileInfo scriptFile = new FileInfo(Path.Combine(path, "next.txt"));
                if (!nodeDir.Exists)
                    return null;
                IScript<string> next = null;
                if (scriptFile.Exists)
                {
                    string script = File.ReadAllText(scriptFile.FullName);
                    next = scriptLoader.LoadScript(script);
                }
                else
                    next = new ScriptStringNull();
                string text = File.ReadAllText(Path.Combine(path, "text.txt"));
                IScreen screen = new Screen(text);
                Dictionary<uint, Choice> choices = new Dictionary<uint, Choice>();
                uint id = 0;
                foreach (Choice choice in choiceLoader.LoadChoices(node_id))
                    choices.Add(++id, choice);
                if (next == null)
                    throw new Exception("Invalid script");
                return new Node(screen, choices, next);
            }
            catch(Exception e)
            {
                throw new Exception(string.Format("Couldn't load the node {0}", node_id), e);
            }
        }

        public IDictionary<string, Node> LoadNodes()
        {
            try
            {
                Dictionary<string, Node> nodes = new Dictionary<string, Node>();
                foreach (string node in NodeIds())
                    nodes.Add(node, LoadNode(node));
                return nodes;
            }
            catch(Exception e)
            {
                throw new Exception("Couldn't load the nodes", e);
            }
        }

    }
}
