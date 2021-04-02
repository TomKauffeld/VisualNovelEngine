using System;
using System.Collections.Generic;

namespace VisualNovelEngine
{
    public class Engine
    {
        private readonly IDictionary<string, Node> nodes = new Dictionary<string, Node>();

        private string activeNode = null;

        private readonly Variables variables = new Variables();

        private Node CurrentNode { get => activeNode != null && nodes.ContainsKey(activeNode) ? nodes[activeNode] : null; }

        public IScreen CurrentScreen { get => CurrentNode?.Screen; }

        public IDictionary<uint, IChoiceScreen> CurrentChoices { get; private set; } = new Dictionary<uint, IChoiceScreen>();

        public bool IsFinished => CurrentNode == null;

        public Engine(IDictionary<string, Node> nodes, string start)
        {
            try
            {
                foreach (KeyValuePair<string, Node> node in nodes)
                    this.nodes.Add(node.Key, node.Value);
                Load(start);
            }
            catch(Exception e)
            {
                throw new Exception("Couldn't start the engine", e);
            }
        }

        public void Render()
        {
            try
            {
                CurrentScreen.Render(variables);
                foreach (KeyValuePair<uint, IChoiceScreen> choice in CurrentChoices)
                    choice.Value.Render(variables, choice.Key);
            }
            catch(Exception e)
            {
                throw new Exception("Couldn't render the node", e);
            }
        }

        public void Load(string node_id)
        {
            try
            {
                if (!nodes.ContainsKey(node_id))
                    throw new Exception("Invalid node");
                activeNode = node_id;
                variables["engine.active_node"] = new VariableString(activeNode);
                CurrentChoices = CurrentNode.Choices(variables);
            }
            catch(Exception e)
            {
                throw new Exception("Couldn't load the node", e);
            }
        }

        public void MakeChoice(uint? choice)
        {
            try
            {
                if (CurrentNode == null)
                    throw new Exception("No node loaded");
                variables["engine.active_node"] = new VariableString(activeNode);

                if (choice.HasValue)
                {
                    variables["engine.choice"] = new VariableFloat(choice.Value);
                    CurrentNode.ExecuteChoice(variables, choice.Value);
                }
                else if (CurrentChoices.Count > 0)
                    throw new Exception("Need to make a choice");

                string next = CurrentNode.Next(variables);
                if (next != null)
                    Load(next);
                else
                {
                    activeNode = null;
                    CurrentChoices = new Dictionary<uint, IChoiceScreen>();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Cannot make choice", e);
            }
        }
    }
}
