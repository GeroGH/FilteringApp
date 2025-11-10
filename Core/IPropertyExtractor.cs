using System.Collections.Generic;
using Tekla.Structures.Model;

namespace FilteringApp.Core
{
    /// <summary>
    /// Defines the contract for extracting attributes from a Tekla model object.
    /// </summary>
    public interface IPropertyExtractor
    {
        /// <summary>
        /// Extracts all meaningful Tekla properties as AttributePair objects.
        /// </summary>
        IEnumerable<AttributePair> Extract(ModelObject modelObject);
    }
}
