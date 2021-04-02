using System;

namespace VisualNovelEngine
{
    public class Choice
    {
        public IChoiceScreen Screen { get; protected set; }

        private readonly IScript<bool> change;

        private readonly IScript<bool> requirements;

        public Choice(IChoiceScreen screen, IScript<bool> change, IScript<bool> requirements)
        {
            Screen = screen;
            this.change = change;
            this.requirements = requirements;
        }

        public Choice(Choice choice) : this(choice.Screen, choice.change, choice.requirements) { }

        public bool IsReady(Variables variables)
        {
            try
            {
                if (requirements != null)
                    return requirements.Execute(variables);
                return true;
            }
            catch(Exception e)
            {
                throw new Exception("Cannot calculate requirements", e);
            }
        }

        public void Execute(Variables variables)
        {
            try
            {
                if (change != null)
                    change.Execute(variables);
            }
            catch(Exception e)
            {
                throw new Exception("Cannot execute choice", e);
            }
        }
    }
}
