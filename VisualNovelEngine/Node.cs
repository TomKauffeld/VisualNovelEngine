using System;
using System.Collections.Generic;

namespace VisualNovelEngine
{
    public class Node
    {
        public IScreen Screen { get; protected set; }

        private readonly IDictionary<uint, Choice> choices = new Dictionary<uint, Choice>();

        private readonly IScript<string> next;

        public Node(IScreen screen, IDictionary<uint, Choice> choices, IScript<string> next)
        {
            Screen = screen;
            foreach (KeyValuePair<uint, Choice> choice in choices)
                this.choices.Add(choice.Key, new Choice(choice.Value));
            this.next = next;
        }

        public Node(Node node) : this(node.Screen, node.choices, node.next) { }

        public string Next(Variables variables)
        {
            try
            {
                return next.Execute(variables);
            }
            catch(Exception e)
            {
                throw new Exception("Cannot calculate next node", e);
            }
        }

        public IDictionary<uint, IChoiceScreen> Choices(Variables variables)
        {
            try
            {
                Dictionary<uint, IChoiceScreen> choices = new Dictionary<uint, IChoiceScreen>();
                foreach (KeyValuePair<uint, Choice> choice in this.choices)
                    if (choice.Value.IsReady(variables))
                        choices.Add(choice.Key, choice.Value.Screen);
                return choices;
            }
            catch(Exception e)
            {
                throw new Exception("Cannot load the choices", e);
            }
        }

        public void ExecuteChoice(Variables variables, uint choice)
        {
            try
            {
                if (!choices.ContainsKey(choice) || !choices[choice].IsReady(variables))
                    throw new Exception("Invalid choice");
                choices[choice].Execute(variables);
            }
            catch(Exception e)
            {
                throw new Exception("Cannot execute choice", e);
            }
        }
        
    }
}
