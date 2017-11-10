using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseManager
{
    public class PropertyList : Property, IReadOnlyList<Property>
    {
        #region FIELDS

        private readonly IReadOnlyList<Property> _list;

        #endregion FIELDS

        #region PROPERTIES

        public override string Name { get; } = "General";

        public override Type Type { get; } = typeof(PropertyList);

        public int Count
            => _list.Count;

        public Property this[int index] 
            => _list[index];

        #endregion PROPERTIES

        #region CONSTRUCTOR

        public PropertyList(IEnumerable<Property> collection)
        {
            _list = collection.ToList();
            Value = _list;
        }

        #endregion CONSTRUCTOR

        #region METHODS

        public IEnumerator<Property> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();
        
        #endregion METHODS
    }
}