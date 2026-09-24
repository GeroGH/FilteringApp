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
                var keys = new[] { "NAME",
                                "BOLT_STANDARD",
                                "BOLT_COMMENT",
                                "BOLT_USERFIELD_1",
                                "BOLT_USERFIELD_2",
                                "BOLT_USERFIELD_3",
                                "BOLT_USERFIELD_4",
                                "BOLT_USERFIELD_5",
                                "BOLT_USERFIELD_6",
                                "BOLT_USERFIELD_7",
                                "BOLT_USERFIELD_8"
                };
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
