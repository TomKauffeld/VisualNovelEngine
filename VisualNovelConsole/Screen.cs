using System;
using VisualNovelEngine;

namespace VisualNovelConsole
{
    public class Screen : IScreen, IChoiceScreen
    {
        public string Text { get; protected set; }

        public Screen(string text)
        {
            Text = text;
        }


        public void Render(Variables variables)
        {
            Console.WriteLine(Text);
        }


        public void Render(Variables variables, uint choice_id)
        {
            Console.WriteLine("{0} : {1}", choice_id, Text);
        }
    }
}
