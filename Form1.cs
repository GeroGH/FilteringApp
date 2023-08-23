using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Tekla.Structures;
using Tekla.Structures.Filtering;
using Tekla.Structures.Filtering.Categories;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using ModelObjectSelector = Tekla.Structures.Model.UI.ModelObjectSelector;


namespace FilteringApp
{
    public partial class Form1 : Form
    {
        private string filterName;
        private readonly DataTable selectionTable = new DataTable();

        public Form1()
        {
            this.InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.filterName = "TestFilterGG";
            this.selectionTable.Columns.Add("Name", typeof(string));
            this.selectionTable.Columns.Add("Value", typeof(string));

            var mos = new ModelObjectSelector();
            var moe = mos.GetSelectedObjects();

            var dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            while (moe.MoveNext())
            {
                var part = moe.Current as Part;
                if (part == null)
                {
                    continue;
                }

                dt.Rows.Add("MATERIAL", part.Material.MaterialString);
                dt.Rows.Add("NAME", part.Name.ToString());

                var hashTable = new Hashtable();
                part.GetAllUserProperties(ref hashTable);

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

                    if (hashTableEnumerator.Key.ToString().Contains("RFI"))
                    {
                        if (hashTableEnumerator.Key.ToString().Contains("RFIcombined"))
                        {
                            dt.Rows.Add(hashTableEnumerator.Key, hashTableEnumerator.Value);
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

                    dt.Rows.Add(hashTableEnumerator.Key, hashTableEnumerator.Value);
                }
            }

            dt = dt.DefaultView.ToTable(true);
            dt.DefaultView.Sort = "Name, Value";
            this.dataGrid.DataSource = dt;
            this.dataGrid.RowHeadersVisible = false;
            this.dataGrid.AllowUserToResizeRows = false;
            this.dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.ReadOnly = true;
            this.dataGrid.ColumnHeadersVisible = true;
        }

        public void CreateFilter(string filterName, BinaryFilterOperatorType type)
        {
            var collection = new BinaryFilterExpressionCollection();

            foreach (DataRow row in this.selectionTable.Rows)
            {
                var template = new TemplateFilterExpressions.CustomString(row["Name"].ToString());
                var value = new StringConstantFilterExpression($"\"{row["Value"]}\"");
                var expresion = new BinaryFilterExpression(template, StringOperatorType.IS_EQUAL, value);
                var item = new BinaryFilterExpressionItem(expresion, type);
                collection.Add(item);
            }

            var Filter = new Filter(collection);
            var fileName = Path.Combine(@".\attributes", filterName);
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

        private void CollectSelectedRows()
        {
            var selectedRows = this.dataGrid.SelectedRows;
            foreach (DataGridViewRow row in selectedRows)
            {
                var name = row.Cells[0].Value.ToString();
                var value = row.Cells[1].Value.ToString();
                this.selectionTable.Rows.Add(name, value);
            }
        }

        private void FilterOr_Click(object sender, EventArgs e)
        {
            this.CollectSelectedRows();
            this.CreateFilter(this.filterName, BinaryFilterOperatorType.BOOLEAN_OR);
            this.ChangeRepresentation(this.filterName);
            this.selectionTable.Clear();
        }

        private void FilterAnd_Click(object sender, EventArgs e)
        {
            this.CollectSelectedRows();
            this.CreateFilter(this.filterName, BinaryFilterOperatorType.BOOLEAN_AND);
            this.ChangeRepresentation(this.filterName);
            this.selectionTable.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var mos = new ModelObjectSelector();
            var moe = mos.GetSelectedObjects();

            var parts = new List<Identifier>();
            while (moe.MoveNext())
            {
                var part = moe.Current as ModelObject;
                if (part == null)
                {
                    continue;
                }

                var id = part.Identifier;
                parts.Add(id);
            }

            ModelObjectVisualization.SetTransparencyForAll(TemporaryTransparency.HIDDEN);
            ModelObjectVisualization.SetTransparency(parts, TemporaryTransparency.VISIBLE);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ModelObjectVisualization.ClearAllTemporaryStates();
        }
    }
}
