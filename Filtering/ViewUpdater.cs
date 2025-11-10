using System;
using System.Collections;
using System.Windows.Forms;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;

namespace FilteringApp.Filtering
{
    /// <summary>
    /// Implements view updates — applies filters to all visible Tekla views.
    /// </summary>
    public class ViewUpdater : IViewUpdater
    {
        public void ApplyRepresentation(string representationName)
        {
            if (string.IsNullOrWhiteSpace(representationName))
                return;

            var visibleViews = ViewHandler.GetVisibleViews();

            // Iterate through each visible view and apply the new filter representation
            while (visibleViews.MoveNext())
            {
                var currentView = visibleViews.Current;
                if (currentView == null)
                    continue;

                currentView.ViewFilter = representationName;
                currentView.Modify();
            }
        }

        public void ClearTeklaSelection()
        {
            try
            {
                var model = new Model();

                // ensure Tekla is running and a model is open
                if (!model.GetConnectionStatus())
                {
                    // optionally log or inform the user
                    return;
                }

                // UI selector is the common approach to change the view selection
                var selector = new Tekla.Structures.Model.UI.ModelObjectSelector();

                // passing an empty ArrayList clears the selection in the view
                selector.Select(new ArrayList());

                // commit so Tekla updates the UI
                model.CommitChanges();
            }
            catch (Exception ex)
            {
                // handle/log error so you know why it failed
                MessageBox.Show($"Failed to clear Tekla selection: {ex.Message}", "FilteringApp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
