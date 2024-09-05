using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
        private readonly string FilterName = "FiteringAppFilterGG";
        private string ModelFolder = string.Empty;
        private DataTable ModelTable = new DataTable();
        private DataTable SelectionTable = new DataTable();
        private List<Part> Parts = new List<Part>();
        private List<BoltGroup> BoltGroups = new List<BoltGroup>();

        public FilteringApp()
        {
            this.InitializeComponent();

            var model = new Model();
            this.ModelFolder = model.GetInfo().ModelPath;
        }

        private void FilerForm_Load(object sender, EventArgs e)
        {
            var currentFormLocation = this.Location;
            this.Location = new Point(currentFormLocation.X + 610, currentFormLocation.Y - 200);

            this.ModelTable.Columns.Add("Name", typeof(string));
            this.ModelTable.Columns.Add("Value", typeof(string));

            var mos = new ModelObjectSelector();
            var moe = mos.GetSelectedObjects();

            while (moe.MoveNext())
            {
                var part = moe.Current as Part;
                if (part != null)
                {
                    this.Parts.Add(part);
                    continue;
                }

                var boltGroup = moe.Current as BoltGroup;
                if (boltGroup != null)
                {
                    this.BoltGroups.Add(boltGroup);
                    continue;
                }
            }

            foreach (var part in this.Parts)
            {
                this.ModelTable.Rows.Add("MATERIAL", part.Material.MaterialString);
                this.ModelTable.Rows.Add("FINISH", part.Finish.ToString());
                this.ModelTable.Rows.Add("NAME", part.Name.ToString());
                this.ModelTable.Rows.Add("CLASS_ATTR", part.Class);

                var materialType = string.Empty;
                part.GetReportProperty("MATERIAL_TYPE", ref materialType);
                this.ModelTable.Rows.Add("MATERIAL_TYPE", materialType);

                var status = string.Empty;
                part.GetReportProperty("ASSY_STATUS", ref status);
                this.ModelTable.Rows.Add("ASSY_STATUS", status);

                var hashTable = new Hashtable();
                part.GetStringUserProperties(ref hashTable);

                var hashTableEnumerator = hashTable.GetEnumerator();
                while (hashTableEnumerator.MoveNext())
                {
                    if (hashTableEnumerator.Key.ToString().Contains("initial_GUID"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("initial_profile"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("FIRE_RATING"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("PRELIM_MARK"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("SDNF_MEMBER_NUMBER"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("proIfcEntityOvrd"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("proIfcEntityPreDef"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("ENVIRONMENT"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("USE"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("EN1090_EXC_PART"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("OUTPUT_ZONE"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("CELL_UTILIZATION"))
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Key.ToString().Contains("RFI"))
                    {
                        if (hashTableEnumerator.Key.ToString().Contains("RFIcombined"))
                        {
                            this.ModelTable.Rows.Add(hashTableEnumerator.Key, hashTableEnumerator.Value);
                        }
                        continue;
                    }

                    if (hashTableEnumerator.Value.ToString() == "0")
                    {
                        continue;
                    }

                    if (hashTableEnumerator.Value.ToString() == "-2147483648")
                    {
                        continue;
                    }

                    this.ModelTable.Rows.Add(hashTableEnumerator.Key, hashTableEnumerator.Value);
                }
            }

            foreach (var boltGroup in this.BoltGroups)
            {
                var standard = string.Empty;
                boltGroup.GetReportProperty("BOLT_STANDARD", ref standard);
                this.ModelTable.Rows.Add("BOLT_STANDARD", standard);
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