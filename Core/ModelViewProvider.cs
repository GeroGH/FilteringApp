using System;
using Tekla.Structures.Model.UI;

namespace FilteringApp.Core
{
    /// <summary>
    /// Provides access to currently visible Tekla views.
    /// Returns view names while skipping internal FilteringApp views.
    /// </summary>
    public static class ModelViewProvider
    {
        private const string FilterTag = "FilteringAppFilter";

        /// <summary>
        /// Returns the first visible Tekla view name that does not contain "FilteringAppFilter".
        /// Returns an empty string if none found.
        /// </summary>
        public static string GetUserViewFilter()
        {
            try
            {
                var visibleViews = ViewHandler.GetVisibleViews();
                while (visibleViews.MoveNext())
                {
                    var view = visibleViews.Current;
                    if (view == null)
                        continue;

                    var viewFilter = view.ViewFilter?.ToString() ?? string.Empty;

                    if (viewFilter.IndexOf(FilterTag, StringComparison.OrdinalIgnoreCase) < 0)
                        return viewFilter;
                }
            }
            catch
            {
                // Intentionally ignored
            }

            return string.Empty;
        }
    }
}
