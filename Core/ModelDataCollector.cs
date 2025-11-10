using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tekla.Structures.Model;

namespace FilteringApp.Core
{
    /// <summary>
    /// Coordinates the collection of all selected Tekla model objects and uses appropriate
    /// property extractors to gather and normalize their attributes into a single DataTable.
    /// </summary>
    public class ModelDataCollector
    {
        private readonly IModelObjectProvider objectProvider;
        private readonly IPropertyExtractor partExtractor;
        private readonly IPropertyExtractor boltExtractor;

        public int LastPartsCount { get; private set; }
        public int LastBoltGroupsCount { get; private set; }

        public ModelDataCollector(IModelObjectProvider provider,
                                  IPropertyExtractor partExtractor,
                                  IPropertyExtractor boltExtractor)
        {
            objectProvider = provider;
            this.partExtractor = partExtractor;
            this.boltExtractor = boltExtractor;
        }

        /// <summary>
        /// Retrieves all selected parts and bolt groups from the Tekla model,
        /// extracts their attributes, deduplicates them, sorts them, and
        /// returns a DataTable ready for binding to a grid.
        /// </summary>
        public DataTable CollectSelectedAttributes()
        {
            var enumerator = objectProvider.GetSelectedObjects();
            var parts = new List<Part>();
            var bolts = new List<BoltGroup>();

            // Enumerate Tekla objects efficiently
            var e = enumerator as System.Collections.IEnumerator;
            while (e != null && e.MoveNext())
            {
                if (e.Current is Part p) parts.Add(p);
                else if (e.Current is BoltGroup b) bolts.Add(b);
            }

            LastPartsCount = parts.Count;
            LastBoltGroupsCount = bolts.Count;

            var attributes = new List<AttributePair>(parts.Count * 10 + bolts.Count * 5);

            // Extract all attributes from parts
            foreach (var p in parts)
                attributes.AddRange(partExtractor.Extract(p));

            // Extract all attributes from bolt groups
            foreach (var b in bolts)
                attributes.AddRange(boltExtractor.Extract(b));

            // Deduplicate on (Name, Value)
            var seen = new HashSet<string>(StringComparer.Ordinal);
            var unique = new List<AttributePair>(attributes.Count);

            foreach (var attr in attributes)
            {
                if (string.IsNullOrWhiteSpace(attr.Value))
                    continue;

                var composite = attr.Name + "|" + attr.Value;
                if (seen.Add(composite))
                    unique.Add(attr);
            }

            // Sort alphabetically for readability
            unique.Sort((a, b) =>
            {
                var cmp = string.CompareOrdinal(a.Name, b.Name);
                return cmp != 0 ? cmp : string.CompareOrdinal(a.Value, b.Value);
            });

            // Build DataTable
            var dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            foreach (var attr in unique)
                dt.Rows.Add(attr.Name, attr.Value);

            return dt;
        }
    }
}
