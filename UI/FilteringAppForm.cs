using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FilteringApp.Core;
using FilteringApp.Filtering;
using FilteringApp.UserData;
using Tekla.Structures.Filtering;
using Tekla.Structures.Model;

namespace FilteringApp.UI
{
    /// <summary>
    /// Main UI for FilteringApp.
    /// Handles user interaction, delegates model data collection and filter creation.
    /// </summary>
    public partial class FilteringApp : Form
    {
        // === Core dependencies ===
        private readonly string filterName;
        private readonly ModelDataCollector dataCollector;
        private readonly IFilterBuilder filterBuilder;
        private readonly IViewUpdater viewUpdater;
        private readonly string initials;

        private DataTable modelTable;
        private DataTable selectionTable;

        public FilteringApp()
        {
            this.InitializeComponent();

            // Initialize dependencies
            var provider = new ModelObjectProvider();
            var partExtractor = new PartPropertyExtractor();
            var boltExtractor = new BoltPropertyExtractor();
            this.dataCollector = new ModelDataCollector(provider, partExtractor, boltExtractor);

            var modelPath = (new Model()).GetInfo().ModelPath ?? string.Empty;
            this.filterBuilder = new FilterBuilder(modelPath);
            this.viewUpdater = new ViewUpdater();

            // Generate initials (fallback if not available)
            var userName = Environment.UserName ?? "user";
            var parts = userName.Split('.');
            this.initials = parts.Length >= 2
                ? $"{char.ToUpper(parts[0][0])}{char.ToUpper(parts[1][0])}"
                : "XX";

            this.filterName = $"FilteringAppFilter{this.initials}";
        }

        /// <summary>
        /// On form load — collect model data, setup UI.
        /// </summary>
        private async void FilerForm_Load(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                // === Show form immediately ===
                this.Show();
                this.statusBarLabel.Text = "Initializing...";

                // Initialize user settings
                var provider = new TeklaModelPathProvider();
                UserSettingsStorage.Initialize(provider, this.initials);

                // Adjust UI position slightly to avoid overlapping Tekla window
                var pos = this.Location;
                this.Location = new Point(pos.X + 610, pos.Y - 150);

                // === Collect model data asynchronously ===
                this.statusBarLabel.Text = "Collecting model data from Tekla...";
                var collected = await System.Threading.Tasks.Task.Run(() =>
                {
                    return this.dataCollector.CollectSelectedAttributes() ?? new DataTable();
                });

                this.modelTable = collected;

                // === Setup DataGrid on UI thread ===
                this.statusBarLabel.Text = "Setting up data grid...";
                this.SetupDataGrid(this.modelTable);

                // Load last user input into the search box
                this.TexBoxUserInput.Text = UserSettingsStorage.LoadTextBoxValue() ?? string.Empty;

                // Clear Tekla selection (can run async if slow)
                this.statusBarLabel.Text = "Clearing Tekla selection...";
                await System.Threading.Tasks.Task.Run(() => this.viewUpdater.ClearTeklaSelection());

                // Final ready message
                var objectsCount = (this.dataCollector?.LastPartsCount ?? 0) +
                                   (this.dataCollector?.LastBoltGroupsCount ?? 0);
                var attributesCount = this.modelTable?.Rows?.Count ?? 0;
                this.statusBarLabel.Text =
                    $"Application ready — {objectsCount} objects, {attributesCount} attributes. Time: {sw.Elapsed.TotalSeconds:F2}s";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during initialization:\n{ex.Message}", "FilteringApp",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.statusBarLabel.Text = "Error during initialization.";
            }
            finally
            {
                sw.Stop();
            }
        }

        /// <summary>
        /// Configure DataGridView with collected data.
        /// </summary>
        private void SetupDataGrid(DataTable table)
        {
            if (table == null) return;

            this.selectionTable = table.Clone();
            this.dataGrid.DataSource = table;

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
        /// Executes filter creation and application to Tekla.
        /// </summary>
        private void Execute(BinaryFilterOperatorType binaryOperator)
        {
            try
            {
                this.statusBarLabel.Text = "Building filter...";

                var activeViewName = ModelViewProvider.GetUserViewFilter() ?? string.Empty;
                UserSettingsStorage.SaveViewFilter(activeViewName);

                this.CollectSelectedRows();
                if (this.selectionTable.Rows.Count == 0)
                {
                    this.statusBarLabel.Text = "No rows selected.";
                    return;
                }

                var attributes = this.selectionTable.AsEnumerable()
                    .Select(r => new AttributePair(r.Field<string>("Name"), r.Field<string>("Value")))
                    .Where(a => !string.IsNullOrWhiteSpace(a.Name) && !string.IsNullOrWhiteSpace(a.Value))
                    .ToList();

                if (attributes.Count == 0)
                {
                    this.statusBarLabel.Text = "No valid attributes to filter.";
                    return;
                }

                this.filterBuilder.CreateFilter(this.filterName, binaryOperator, attributes);
                this.viewUpdater.ApplyRepresentation(this.filterName);

                this.statusBarLabel.Text = $"Filter '{this.filterName}' applied.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing filter:\n{ex.Message}", "FilteringApp",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.statusBarLabel.Text = "Filter execution failed.";
            }
        }

        /// <summary>
        /// Collect selected rows from grid.
        /// </summary>
        private void CollectSelectedRows()
        {
            this.selectionTable.Clear();
            if (this.dataGrid.SelectedRows.Count == 0) return;

            foreach (DataGridViewRow row in this.dataGrid.SelectedRows)
            {
                if (row.Cells.Count < 2) continue;
                var name = row.Cells[0].Value?.ToString();
                var value = row.Cells[1].Value?.ToString();

                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(value))
                    this.selectionTable.Rows.Add(name, value);
            }
        }

        // === UI event handlers ===
        private void FilterOr_Click(object sender, EventArgs e) => this.Execute(BinaryFilterOperatorType.BOOLEAN_OR);

        private void FilterAnd_Click(object sender, EventArgs e) => this.Execute(BinaryFilterOperatorType.BOOLEAN_AND);

        private void DataGrid_Click(object sender, EventArgs e) => this.Execute(BinaryFilterOperatorType.BOOLEAN_OR);

        private void ReuseOldFilter_Click(object sender, EventArgs e)
        {
            try
            {
                this.statusBarLabel.Text = "Reapplying last filter...";
                this.viewUpdater.ApplyRepresentation(this.filterName);
                this.statusBarLabel.Text = "Last filter applied.";
            }
            catch (Exception ex)
            {
                this.statusBarLabel.Text = "Failed to reapply filter.";
                Debug.WriteLine(ex.Message);
            }
        }

        private void ReuseUserViewFilter_Click(object sender, EventArgs e)
        {
            try
            {
                var savedFilter = UserSettingsStorage.LoadViewFilter();
                if (string.IsNullOrWhiteSpace(savedFilter))
                {
                    this.statusBarLabel.Text = "No saved view filter found.";
                    return;
                }

                this.viewUpdater.ApplyRepresentation(savedFilter);
                this.statusBarLabel.Text = $"View filter '{savedFilter}' applied.";
            }
            catch (Exception ex)
            {
                this.statusBarLabel.Text = "Failed to apply saved view filter.";
                Debug.WriteLine(ex.Message);
            }
        }

        private void TexBoxUserInput_TextChanged(object sender, EventArgs e)
        {
            if (this.modelTable == null) return;

            var txt = this.TexBoxUserInput.Text.Replace("'", "''");
            this.modelTable.DefaultView.RowFilter =
                $"[Name] LIKE '%{txt}%' OR [Value] LIKE '%{txt}%'";

            this.statusBarLabel.Text = $"Filtered view: {this.dataGrid.Rows.Count} items";
            UserSettingsStorage.SaveTextBoxValue(this.TexBoxUserInput.Text);
        }
    }
}
