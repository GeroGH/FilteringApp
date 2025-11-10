using System.Collections.Generic;
using Tekla.Structures.Filtering;
using FilteringApp.Core;

namespace FilteringApp.Filtering
{
    /// <summary>
    /// Defines the behavior for creating Tekla filter (.filter) files
    /// based on a collection of attribute pairs.
    /// </summary>
    public interface IFilterBuilder
    {
        /// <summary>
        /// Builds and saves a Tekla filter file.
        /// </summary>
        /// <param name="filterName">Name of the filter file to create (without extension).</param>
        /// <param name="type">Boolean operator type (AND / OR).</param>
        /// <param name="attributePairs">Collection of attribute pairs to include in the filter.</param>
        void CreateFilter(string filterName, BinaryFilterOperatorType type, IEnumerable<AttributePair> attributePairs);
    }
}
