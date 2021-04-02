using System.Collections.Generic;

namespace VisualNovelEngine
{
    public interface INodeLoader
    {
        IList<string> NodeIds();
        Node LoadNode(string node_id);
        IDictionary<string, Node> LoadNodes();
    }
}
