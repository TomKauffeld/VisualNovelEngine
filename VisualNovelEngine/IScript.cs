using System.Collections.Generic;

namespace VisualNovelEngine
{
    public interface IScript<T>
    {

        T Execute(Variables variables);
    }
}
