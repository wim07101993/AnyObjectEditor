﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Shared.Helpers.Extensions;
using TypelessDatabaseManager.Annotations;
// ReSharper disable All

namespace TypelessDatabaseManager.Models
{
    public class Object : IDictionary<string, Property>, INotifyPropertyChanged, IComparable<Object>, IComparable
    {
        #region FIELDS

        private object _value;
      
        #endregion FIELDS


        #region PROPERTIES

        public object Value
        {
            get => Properties ?? _value;
            set
            {
                if (Properties != null)
                    Properties = (Dictionary<string, Property>) value;
                else
                    _value = value;
            }
        }

        public Property this[string key]
        {
            get => Properties?[key];
            set
            {
                if (Properties == null)
                    throw new KeyNotFoundException();

                Properties[key] = value;
                RaisePropertyChanged(key);
            }
        }

        public ICollection<string> Keys
            => Properties?.Keys ?? (ICollection<string>) new List<string>();

        public ICollection<Property> Values
            => Properties?.Values ?? (ICollection<Property>) new List<Property>();

        public int Count
            => Properties?.Count ?? -1;

        public bool IsReadOnly => false;

        public Dictionary<string, Property> Properties { get; private set; }

        #endregion PROPERTIES


        #region CONSTRUCTORS

        public Object()
        {
        }

        public Object(IDictionary<string, Property> properties)
        {
            Properties = ToDictionary(properties);
        }
        
        #endregion CONSTRUCTORS


        #region METHODS

        public IEnumerator<KeyValuePair<string, Property>> GetEnumerator()
            => Properties?.GetEnumerator() ?? new Dictionary<string, Property>.Enumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(KeyValuePair<string, Property> item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, Property>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(string key, out Property value)
        {
            throw new System.NotImplementedException();
        }

        void IDictionary<string, Property>.Add(string key, Property value)
            => Properties.Add(key, value);

        bool IDictionary<string, Property>.Remove(string key)
            => Properties.Remove(key);

        void ICollection<KeyValuePair<string, Property>>.Add(KeyValuePair<string, Property> item)
        {
            if (Properties == null)
                throw new NullReferenceException();

            ((ICollection<KeyValuePair<string, Property>>) Properties).Add(item);
        }

        void ICollection<KeyValuePair<string, Property>>.Clear()
            => Properties.Clear();

        bool ICollection<KeyValuePair<string, Property>>.Remove(KeyValuePair<string, Property> item)
        {
            if (Properties == null)
                throw new NullReferenceException();

            return ((ICollection<KeyValuePair<string, Property>>) Properties).Remove(item);
        }

        private Dictionary<string, Property> ToDictionary(IDictionary<string, Property> iDictionary)
        {
            if (iDictionary == null)
                return null;

            if (iDictionary is Dictionary<string, Property> dictionary)
                return dictionary;

            var propDictionary = new Dictionary<string, Property>();
            foreach (var pair in iDictionary)
                ((ICollection<KeyValuePair<string, Property>>) propDictionary).Add(pair);

            return propDictionary;
        }

        public int CompareTo(Object other) 
            => Value?.ToString().CompareTo(other?.Value?.ToString()) ?? default(int);

        public int CompareTo(object obj)
            => Value?.ToString().CompareTo((obj as Object)?.Value?.ToString()) ?? default(int);


        public void ClearValues()
        {
            foreach (var key in Keys)
                this[key].Value.Value = this[key].Value.Value.GetType().GetDefault();
        }

        #endregion METHODS


        #region NOTIFY PROPERTYCHANGED

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion NOTIFY PROPERTYCHANGED

    }
}