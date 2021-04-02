using System.Collections.Generic;

namespace VisualNovelEngine
{
    public interface IChoiceLoader
    {
        IList<Choice> LoadChoices(string node_id);
    }
}
