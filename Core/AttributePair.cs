namespace FilteringApp.Core
{
    /// <summary>
    /// Simple Data Transfer Object representing a single attribute name-value pair
    /// extracted from a Tekla model object (Part or BoltGroup).
    /// </summary>
    public class AttributePair
    {
        /// <summary>
        /// The attribute name (Tekla report property name, user field, etc.)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The string value for this attribute, as reported from the model.
        /// </summary>
        public string Value { get; set; }

        public AttributePair(string name, string value)
        {
            Name = name;
            Value = value ?? string.Empty;
        }

        public override string ToString() => $"{Name} = {Value}";
    }
}
