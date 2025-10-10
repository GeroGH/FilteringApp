using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tekla.Structures.Filtering;
using Tekla.Structures.Filtering.Categories;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using ModelObjectSelector = Tekla.Structures.Model.UI.ModelObjectSelector;


namespace FilteringApp
{
    public partial class FilteringApp : Form
    {
        private readonly string FilterName = String.Empty;
        private readonly string ModelFolder = string.Empty;

        private readonly List<Part> Parts = new List<Part>();
        private readonly List<BoltGroup> BoltGroups = new List<BoltGroup>();

        private DataTable ModelTable = new DataTable();
        private DataTable SelectionTable = new DataTable();

        public FilteringApp()
        {
            this.InitializeComponent();

            var model = new Model();
            this.ModelFolder = model.GetInfo().ModelPath;

            var userName = Environment.UserName;
            var firstName = userName.Split('.')[0];
            var secondName = userName.Split('.')[1];
            var initials = char.ToUpper(firstName[0]).ToString() + char.ToUpper(secondName[0]).ToString();
            this.FilterName = "FiteringAppFilter" + initials;
        }

        private void FilerForm_Load(object sender, EventArgs e)
        {
            var stopwatch = Stopwatch.StartNew();

            var currentFormLocation = this.Location;
            this.Location = new Point(currentFormLocation.X + 610, currentFormLocation.Y - 200);
            this.Show();

            this.ModelTable.Columns.Add("Name", typeof(string));
            this.ModelTable.Columns.Add("Value", typeof(string));

            this.statusBarLabel.Text = "Collecting parts from the model ...";

            var mos = new ModelObjectSelector();
            var moe = mos.GetSelectedObjects();

            var parts = new List<Part>();
            var bolts = new List<BoltGroup>();

            var selectedModelObjects = new List<ModelObject>();
            while (moe.MoveNext())
            {
                var current = moe.Current;
                if (current != null)
                    selectedModelObjects.Add(current);
            }

            foreach (var modelObject in selectedModelObjects)
            {
                var part = modelObject as Part;
                if (part != null)
                {
                    parts.Add(part);
                    continue;
                }

                var bolt = modelObject as BoltGroup;
                if (bolt != null)
                {
                    bolts.Add(bolt);
                }
            }

            this.Parts.AddRange(parts);
            this.BoltGroups.AddRange(bolts);

            this.statusBarLabel.Text = "Collecting user fields from the parts ...";

            foreach (var part in this.Parts)
            {
                this.ModelTable.Rows.Add("MATERIAL", part.Material.MaterialString);
                this.ModelTable.Rows.Add("FINISH", part.Finish.ToString());
                this.ModelTable.Rows.Add("NAME", part.Name.ToString());
                this.ModelTable.Rows.Add("CLASS_ATTR", part.Class);

                var profileType = string.Empty;
                part.GetReportProperty("PROFILE_TYPE", ref profileType);
                this.ModelTable.Rows.Add("PROFILE_TYPE", profileType);

                if (profileType != "B")
                {
                    var profile = string.Empty;
                    part.GetReportProperty("PROFILE", ref profile);
                    this.ModelTable.Rows.Add("PROFILE", profile);
                }

                if (profileType == "B")
                {
                    var sectionSize = string.Empty;
                    part.GetReportProperty("SectionSize", ref sectionSize);

                    if (sectionSize != string.Empty)
                    {
                        this.ModelTable.Rows.Add("SectionSize", sectionSize);
                    }
                }

                var uf1 = string.Empty;
                part.GetReportProperty("USER_FIELD_1", ref uf1);
                this.ModelTable.Rows.Add("USER_FIELD_1", uf1);

                var uf2 = string.Empty;
                part.GetReportProperty("USER_FIELD_2", ref uf2);
                this.ModelTable.Rows.Add("USER_FIELD_2", uf2);

                var uf3 = string.Empty;
                part.GetReportProperty("USER_FIELD_3", ref uf3);
                this.ModelTable.Rows.Add("USER_FIELD_3", uf3);

                var uf4 = string.Empty;
                part.GetReportProperty("USER_FIELD_4", ref uf4);
                this.ModelTable.Rows.Add("USER_FIELD_4", uf4);

                var phaseName = string.Empty;
                part.GetReportProperty("PHASE.NAME", ref phaseName);
                this.ModelTable.Rows.Add("PHASE.NAME", phaseName);

                var userPhase = string.Empty;
                part.GetReportProperty("USER_PHASE", ref userPhase);
                this.ModelTable.Rows.Add("USER_PHASE", userPhase);

                var materialType = string.Empty;
                part.GetReportProperty("MATERIAL_TYPE", ref materialType);
                this.ModelTable.Rows.Add("MATERIAL_TYPE", materialType);

                var status = string.Empty;
                part.GetReportProperty("ASSY_STATUS", ref status);
                this.ModelTable.Rows.Add("ASSY_STATUS", status);

                var partPrefix = string.Empty;
                part.GetReportProperty("PART_PREFIX", ref partPrefix);
                this.ModelTable.Rows.Add("PART_PREFIX", partPrefix);

                var assemblyPrefix = string.Empty;
                part.GetReportProperty("ASSEMBLY_PREFIX", ref assemblyPrefix);
                this.ModelTable.Rows.Add("ASSEMBLY_PREFIX", assemblyPrefix);

                var assemblyDefaultPrefix = string.Empty;
                part.GetReportProperty("ASSEMBLY_DEFAULT_PREFIX", ref assemblyDefaultPrefix);
                this.ModelTable.Rows.Add("ASSEMBLY_DEFAULT_PREFIX", assemblyDefaultPrefix);

                var fireProduct = string.Empty;
                part.GetReportProperty("FIRE_PRODUCT", ref fireProduct);
                this.ModelTable.Rows.Add("FIRE_PRODUCT", fireProduct);

                var hashTable = new Hashtable();
                part.GetStringUserProperties(ref hashTable);

                foreach (DictionaryEntry entry in hashTable)
                {
                    if (entry.Key.ToString().Contains("SectionSize"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("PROFILE1"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("initial_GUID"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("initial_profile"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("FIRE_RATING"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("PRELIM_MARK"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("SDNF_MEMBER_NUMBER"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("proIfcEntityOvrd"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("proIfcEntityPreDef"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("ENVIRONMENT"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("USE"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("EN1090_EXC_PART"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("OUTPUT_ZONE"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("CELL_UTILIZATION"))
                    {
                        continue;
                    }

                    if (entry.Key.ToString().Contains("RFI"))
                    {
                        if (entry.Key.ToString().Contains("RFIcombined"))
                        {
                            this.ModelTable.Rows.Add(entry.Key, entry.Value);
                        }
                        continue;
                    }

                    if (entry.Value.ToString() == "0")
                    {
                        continue;
                    }

                    if (entry.Value.ToString() == "-2147483648")
                    {
                        continue;
                    }

                    this.ModelTable.Rows.Add(entry.Key, entry.Value);
                }
            }

            foreach (var boltGroup in this.BoltGroups)
            {
                var boltStandard = string.Empty;
                var boltComment = string.Empty;

                boltGroup.GetReportProperty("BOLT_STANDARD", ref boltStandard);
                boltGroup.GetReportProperty("BOLT_COMMENT", ref boltComment);

                this.ModelTable.Rows.Add("BOLT_STANDARD", boltStandard);
                this.ModelTable.Rows.Add("BOLT_COMMENT", boltComment);
            }

            var rowsToDelete = this.ModelTable.AsEnumerable()
                    .Where(row => string.IsNullOrWhiteSpace(row["Value"]?.ToString()))
                    .ToList();

            foreach (var row in rowsToDelete)
            {
                this.ModelTable.Rows.Remove(row);
            }

            this.ModelTable = this.ModelTable.DefaultView.ToTable(true);
            this.ModelTable.DefaultView.Sort = "Name, Value";

            this.SelectionTable = this.ModelTable.Clone();

            this.dataGrid.DataSource = this.ModelTable;
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.AllowUserToResizeRows = false;
            this.dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.ReadOnly = true;
            this.dataGrid.ColumnHeadersVisible = true;

            this.TexBoxUserInput.Select();

            mos.Select(new ArrayList(), false);

            stopwatch.Stop();
            this.statusBarLabel.Text = $"Application ready, time used in seconds: {stopwatch.Elapsed.TotalSeconds}";
        }
        private void Execute(BinaryFilterOperatorType binaryOperator)
        {
            this.CollectSelectedRows();
            this.CreateFilter(this.FilterName, binaryOperator);
            this.ChangeRepresentation(this.FilterName);
        }
        private void CollectSelectedRows()
        {
            this.SelectionTable.Clear();
            var selectedRows = this.dataGrid.SelectedRows;
            foreach (DataGridViewRow row in selectedRows)
            {
                var name = row.Cells[0].Value.ToString();
                var value = row.Cells[1].Value.ToString();
                this.SelectionTable.Rows.Add(name, value);
            }
        }

        public void CreateFilter(string filterName, BinaryFilterOperatorType type)
        {
            var collection = new BinaryFilterExpressionCollection();

            foreach (DataRow row in this.SelectionTable.Rows)
            {
                var template = new TemplateFilterExpressions.CustomString(row["Name"].ToString());
                var value = new StringConstantFilterExpression($"\"{row["Value"]}\"");
                var expresion = new BinaryFilterExpression(template, StringOperatorType.IS_EQUAL, value);
                var item = new BinaryFilterExpressionItem(expresion, type);
                collection.Add(item);
            }

            var Filter = new Filter(collection);
            var fileName = Path.Combine(this.ModelFolder, @".\attributes", filterName);
            Filter.CreateFile(FilterExpressionFileType.OBJECT_GROUP_VIEW, fileName);
        }

        private void ChangeRepresentation(string representation)
        {
            var VisibleViews = ViewHandler.GetVisibleViews();
            while (VisibleViews.MoveNext())
            {
                var CurrentView = VisibleViews.Current;
                CurrentView.ViewFilter = representation;
                CurrentView.Modify();
            }
        }

        private void FilterOr_Click(object sender, EventArgs e)
        {
            this.Execute(BinaryFilterOperatorType.BOOLEAN_OR);
        }

        private void FilterAnd_Click(object sender, EventArgs e)
        {
            this.Execute(BinaryFilterOperatorType.BOOLEAN_AND);
        }

        private void DataGrid_Click(object sender, EventArgs e)
        {
            this.Execute(BinaryFilterOperatorType.BOOLEAN_OR);
        }

        private void ReuseOldFilter_Click(object sender, EventArgs e)
        {
            this.ChangeRepresentation(this.FilterName);
        }

        private void TexBoxUserInput_TextChanged(object sender, EventArgs e)
        {
            this.ModelTable.DefaultView.RowFilter = string.Format($"[Name] LIKE '%{this.TexBoxUserInput.Text}%' OR [Value] LIKE '%{this.TexBoxUserInput.Text}%'");
        }
    }
}