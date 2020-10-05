using System;
using System.Collections.Generic;

namespace CakeExtracter.Extensions
{
    /// <inheritdoc />
    /// <summary>
    /// Comparer for EntityStorageId for comparison unicode characters.
    /// </summary>
    public class EntityIdStorageEqualityComparer : IEqualityComparer<string>
    {
        /// <inheritdoc/>
        public bool Equals(string a, string b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null)
            {
                return false;
            }
            return string.Equals(a, b, StringComparison.Ordinal);
        }

        /// <inheritdoc/>
        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
