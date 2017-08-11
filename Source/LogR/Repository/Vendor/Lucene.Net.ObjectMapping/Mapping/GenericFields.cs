﻿
namespace Lucene.Net.Mapping
{
    /// <summary>
    /// Generic fields added by Lucene.Net.ObjectMapping.
    /// </summary>
    public enum GenericField
    {
        /// <summary>
        /// The field which holds the actual type of the object stored in the document.
        /// </summary>
        ActualType,

        /// <summary>
        /// The field which holds the static type of the object stored in the document.
        /// </summary>
        StaticType,

        /// <summary>
        /// The field which holds the source for the object stored in the document.
        /// </summary>
        Source,

        /// <summary>
        /// The field which holds the timestamp for the object stored in the document.
        /// </summary>
        Timestamp,
    }
}
