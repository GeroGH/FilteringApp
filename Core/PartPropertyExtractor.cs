using System;
using System.Collections;
using System.Collections.Generic;
using Tekla.Structures.Model;

namespace FilteringApp.Core
{
    /// <summary>
    /// Responsible for extracting all relevant Tekla attributes from a Part.
    /// This class handles user-defined properties, report properties, and filtering out unwanted fields.
    /// </summary>
    public class PartPropertyExtractor : IPropertyExtractor
    {
        // Define substrings that, if found in a user property key, will cause it to be skipped.
        private static readonly string[] blacklistSubstrings = new[]
        {
            "SectionSize","PROFILE1","initial_GUID","initial_profile","FIRE_RATING","PRELIM_MARK",
            "SDNF_MEMBER_NUMBER","proIfcEntityOvrd","proIfcEntityPreDef","ENVIRONMENT","USE",
            "EN1090_EXC_PART","OUTPUT_ZONE","CELL_UTILIZATION","RFI"
        };

        public IEnumerable<AttributePair> Extract(ModelObject modelObject)
        {
            if (modelObject is Part part)
            {
                string Safe(string s) => s ?? string.Empty;

                // Core part attributes
                yield return new AttributePair("MATERIAL", Safe(part.Material?.MaterialString));
                yield return new AttributePair("FINISH", Safe(part.Finish?.ToString()));
                yield return new AttributePair("NAME", Safe(part.Name?.ToString()));
                yield return new AttributePair("CLASS_ATTR", Safe(part.Class));

                // Extracting additional report properties via Tekla API
                var profileType = GetReportProperty(part, "PROFILE_TYPE");
                yield return new AttributePair("PROFILE_TYPE", profileType);

                if (!string.Equals(profileType, "B", StringComparison.Ordinal))
                {
                    yield return new AttributePair("PROFILE", GetReportProperty(part, "PROFILE"));
                }
                else
                {
                    var sectionSize = GetReportProperty(part, "SectionSize");
                    if (!string.IsNullOrWhiteSpace(sectionSize))
                        yield return new AttributePair("SectionSize", sectionSize);
                }

                // Common Tekla fields and user fields
                var keys = new[]
                {
                "USER_FIELD_1","USER_FIELD_2","USER_FIELD_3","USER_FIELD_4",
                "PHASE.NAME","USER_PHASE","MATERIAL_TYPE","ASSY_STATUS",
                "PART_PREFIX","ASSEMBLY_PREFIX","ASSEMBLY_DEFAULT_PREFIX","FIRE_PRODUCT"
            };

                foreach (var key in keys)
                    yield return new AttributePair(key, GetReportProperty(part, key));

                // Handle user-defined properties from hash table
                var hash = new Hashtable();
                part.GetStringUserProperties(ref hash);

                if (hash == null)
                    yield break;

                foreach (DictionaryEntry entry in hash)
                {
                    if ((entry.Key) == null || entry.Value == null)
                    {
                        continue;
                    }
                    var key = entry.Key.ToString();
                    var value = entry.Value.ToString();

                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                        continue;

                    // Skip sentinel numeric values (Tekla internal placeholders)
                    if (value == "0" || value == "-2147483648")
                        continue;

                    // Skip blacklisted property names
                    if (ShouldSkip(key))
                        continue;

                    // Special case: RFI but not RFIcombined → skip
                    if (key.Contains("RFI") && !key.Contains("RFIcombined"))
                        continue;

                    yield return new AttributePair(key, value);
                }
            }
        }

        /// <summary>
        /// Returns true if this property key is to be ignored based on the blacklist.
        /// </summary>
        private static bool ShouldSkip(string key)
        {
            foreach (var s in blacklistSubstrings)
            {
                if (key.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Allow "RFIcombined"
                    return !string.Equals(s, "RFI", StringComparison.OrdinalIgnoreCase) ||
                        key.IndexOf("RFIcombined", StringComparison.OrdinalIgnoreCase) < 0;
                }
            }
            return false;
        }

        private static string GetReportProperty(ModelObject obj, string property)
        {
            var tmp = string.Empty;
            obj.GetReportProperty(property, ref tmp);
            return tmp ?? string.Empty;
        }
    }
}
