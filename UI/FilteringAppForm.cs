using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FilteringApp.Core;
using FilteringApp.Filtering;
using Tekla.Structures.Filtering;
using Tekla.Structures.Model;

namespace FilteringApp.UI
{
    /// <summary>
    /// The main Windows Form UI for the FilteringApp tool.
    /// Handles user interactions, delegates model data collection and filter creation,
    /// and displays attribute data in a grid.
    /// </summary>
    public partial class FilteringApp : Form
    {
        // Core dependencies (injected via composition root in the constructor)
        private readonly string filterName;
        private readonly ModelDataCollector dataCollector;
        private readonly IFilterBuilder filterBuilder;
        private readonly IViewUpdater viewUpdater;

        // Local DataTables for UI state
        private DataTable modelTable;
        private DataTable selectionTable;

        public FilteringApp()
        {
            this.InitializeComponent();

            // === Dependency Composition ===
            // Each component is injected manually here for simplicity,
            // but could be wired by a DI container in a larger project.
            var provider = new ModelObjectProvider();
            var partExtractor = new PartPropertyExtractor();
            var boltExtractor = new BoltPropertyExtractor();
            this.dataCollector = new ModelDataCollector(provider, partExtractor, boltExtractor);

            var modelPath = (new Model()).GetInfo().ModelPath;
            this.filterBuilder = new FilterBuilder(modelPath);
            this.viewUpdater = new ViewUpdater();

            // Generate unique filter name from user initials to avoid collisions
            var userName = Environment.UserName ?? string.Empty;
            var parts = userName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var initials = parts.Length >= 2
                ? (char.ToUpper(parts[0][0]).ToString() + char.ToUpper(parts[1][0]).ToString())
                : "XX";

            this.filterName = "FilteringAppFilter" + initials;
        }

        /// <summary>
        /// Form load event — this is where model data is collected and displayed.
        /// </summary>
        private void FilerForm_Load(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                // Adjust form position slightly so it doesn't overlap Tekla main window
                var pos = this.Location;
                this.Location = new Point(pos.X + 610, pos.Y - 150);
                this.Show();

                this.statusBarLabel.Text = "Collecting model data...";

                // === Collect all selected attributes from Tekla model ===
                this.modelTable = this.dataCollector.CollectSelectedAttributes();

                // === Bind the results to the DataGrid ===
                this.SetupDataGrid(this.modelTable);

                // === Deselect all parts in the Tekla model ===
                this.viewUpdater.ClearTeklaSelection();

                this.statusBarLabel.Text = $"Application ready. Objects: {this.dataCollector.LastPartsCount + this.dataCollector.LastBoltGroupsCount} Attributes: {this.modelTable.Rows.Count} Time: {sw.Elapsed.TotalSeconds:F3}s";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during load: {ex.Message}", "FilteringApp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.statusBarLabel.Text = "Error during initialization";
            }
            finally
            {
                sw.Stop();
            }
        }

        /// <summary>
        /// Prepares the DataGridView for display and ensures it is read-only and neatly formatted.
        /// </summary>
        private void SetupDataGrid(DataTable table)
        {
            this.selectionTable = table.Clone(); // Copy the schema (Name, Value)
            this.dataGrid.DataSource = table;

            // Disable editing features for a cleaner UI
            this.dataGrid.ReadOnly = true;
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToResizeRows = false;
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGrid.ColumnHeadersVisible = true;

            this.TexBoxUserInput.Select();
        }

        /// <summary>
        /// Called when the user chooses to apply a filter.
        /// Collects selected rows and builds a Tekla filter.
        /// </summary>
        private void Execute(BinaryFilterOperatorType binaryOperator)
        {
            this.CollectSelectedRows();

            // Convert selected DataGridView rows to AttributePair objects
            var attributes = this.selectionTable.AsEnumerable()
                .Select(r => new AttributePair(r.Field<string>("Name"), r.Field<string>("Value")))
                .ToList();

            // === Build the filter file ===
            this.filterBuilder.CreateFilter(this.filterName, binaryOperator, attributes);

            // === Apply filter to visible Tekla views ===
            this.viewUpdater.ApplyRepresentation(this.filterName);
        }

        /// <summary>
        /// Reads all selected rows in the DataGrid and copies them to the selectionTable.
        /// </summary>
        private void CollectSelectedRows()
        {
            this.selectionTable.Clear();

            foreach (DataGridViewRow row in this.dataGrid.SelectedRows)
            {
                if (row.Cells[0].Value == null || row.Cells[1].Value == null)
                    continue;

                var name = row.Cells[0].Value.ToString();
                var value = row.Cells[1].Value.ToString();
                this.selectionTable.Rows.Add(name, value);
            }
        }

        // === UI Event Handlers ===

        private void FilterOr_Click(object sender, EventArgs e)
            => this.Execute(BinaryFilterOperatorType.BOOLEAN_OR);

        private void FilterAnd_Click(object sender, EventArgs e)
            => this.Execute(BinaryFilterOperatorType.BOOLEAN_AND);

        private void DataGrid_Click(object sender, EventArgs e)
            => this.Execute(BinaryFilterOperatorType.BOOLEAN_OR);

        private void ReuseOldFilter_Click(object sender, EventArgs e)
            => this.viewUpdater.ApplyRepresentation(this.filterName);

        /// <summary>
        /// Filters the displayed attributes dynamically as the user types in the search box.
        /// </summary>
        private void TexBoxUserInput_TextChanged(object sender, EventArgs e)
        {
            var txt = this.TexBoxUserInput.Text.Replace("'", "''");
            this.modelTable.DefaultView.RowFilter =
                string.Format("[Name] LIKE '%{0}%' OR [Value] LIKE '%{0}%'", txt);

            this.statusBarLabel.Text = $"Filtered view: {this.dataGrid.Rows.Count} visible items";
        }
    }
}
