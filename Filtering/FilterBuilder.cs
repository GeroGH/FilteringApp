using System;
using System.Collections.Generic;
using System.IO;
using FilteringApp.Core;
using Tekla.Structures.Filtering;
using Tekla.Structures.Filtering.Categories;

namespace FilteringApp.Filtering
{
    /// <summary>
    /// Responsible for creating and writing Tekla filter files (.filter)
    /// from a list of attribute pairs (Name/Value).
    /// </summary>
    public class FilterBuilder : IFilterBuilder
    {
        private readonly string modelFolder;

        public FilterBuilder(string modelFolder)
        {
            this.modelFolder = modelFolder ?? throw new ArgumentNullException(nameof(modelFolder));
        }

        /// <summary>
        /// Creates a filter file for the current Tekla model based on the provided attributes.
        /// </summary>
        public void CreateFilter(string filterName, BinaryFilterOperatorType type, IEnumerable<AttributePair> attributePairs)
        {
            if (string.IsNullOrWhiteSpace(filterName))
                throw new ArgumentException("Filter name cannot be empty.", nameof(filterName));

            // Tekla uses BinaryFilterExpressionCollection to define complex boolean logic between criteria
            var collection = new BinaryFilterExpressionCollection();

            foreach (var attr in attributePairs)
            {
                // The left-hand side of the expression is the Tekla attribute name
                var template = new TemplateFilterExpressions.CustomString(attr.Name);

                // The right-hand side (constant) is wrapped in quotes to match Tekla syntax
                var value = new StringConstantFilterExpression($"\"{attr.Value}\"");

                // Create an equality expression: e.g., NAME == "BEAM"
                var expr = new BinaryFilterExpression(template, StringOperatorType.IS_EQUAL, value);

                // Combine using the provided operator (AND/OR)
                var item = new BinaryFilterExpressionItem(expr, type);

                collection.Add(item);
            }

            // Build the final filter object and write to Tekla model attributes folder
            var filter = new Filter(collection);

            var attrFolder = Path.Combine(this.modelFolder, ".\\attributes");
            if (!Directory.Exists(attrFolder))
            {
                try { Directory.CreateDirectory(attrFolder); }
                catch { /* ignore — Tekla may manage this itself */ }
            }

            var filePath = Path.Combine(attrFolder, filterName);
            filter.CreateFile(FilterExpressionFileType.OBJECT_GROUP_VIEW, filePath);
        }
    }
}
