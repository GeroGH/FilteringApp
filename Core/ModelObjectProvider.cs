using Tekla.Structures.Model;

namespace FilteringApp.Core
{
    /// <summary>
    /// Implementation of IModelObjectProvider using Tekla's ModelObjectSelector.
    /// Responsible for accessing the model safely and returning selected objects.
    /// </summary>
    public class ModelObjectProvider : IModelObjectProvider
    {
        public ModelObjectEnumerator GetSelectedObjects()
        {
            var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();
            return selector.GetSelectedObjects();
        }
    }
}
