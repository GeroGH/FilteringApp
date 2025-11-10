using Tekla.Structures.Model;

namespace FilteringApp.Core
{
    /// <summary>
    /// Provides access to Tekla's currently selected model objects.
    /// We separate this interface so that later we can mock or extend
    /// it to get model objects in other ways (by phase, query, etc.).
    /// </summary>
    public interface IModelObjectProvider
    {
        /// <summary>
        /// Gets the current Tekla model object enumerator for selected objects.
        /// </summary>
        ModelObjectEnumerator GetSelectedObjects();
    }
}
