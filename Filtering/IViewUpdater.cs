namespace FilteringApp.Filtering
{
    /// <summary>
    /// Defines the interface for applying view filters (representations)
    /// to currently visible Tekla views.
    /// </summary>
    public interface IViewUpdater
    {
        /// <summary>
        /// Applies the specified representation (filter) name to all visible views.
        /// </summary>
        void ApplyRepresentation(string representationName);
        void ClearTeklaSelection();
    }
}
