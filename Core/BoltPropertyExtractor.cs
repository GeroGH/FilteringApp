using System.Collections.Generic;
using Tekla.Structures.Model;

namespace FilteringApp.Core
{
    /// <summary>
    /// Extracts Tekla bolt group attributes and wraps them as AttributePair objects.
    /// </summary>
    public class BoltPropertyExtractor : IPropertyExtractor
    {
        public IEnumerable<AttributePair> Extract(ModelObject modelObject)
        {
            if (modelObject is BoltGroup bolt)
            {
                var keys = new[] { "NAME", "BOLT_STANDARD", "BOLT_COMMENT", "BOLT_USERFIELD_1", "BOLT_USERFIELD_2" };
                foreach (var key in keys)
                    yield return new AttributePair(key, GetReportProperty(bolt, key));
            }
        }

        private static string GetReportProperty(ModelObject obj, string property)
        {
            var tmp = string.Empty;
            obj.GetReportProperty(property, ref tmp);
            return tmp ?? string.Empty;
        }
    }
}
