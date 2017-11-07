﻿namespace DatabaseManager.EventArgs
{
    public class ValueChangedEventArgs : System.EventArgs
    {
        #region PROPERTIES

        public object OldValue { get; set; }
        public object NewValue { get; set; }

        #endregion PROPERTIES

        #region CONSTRUCTOR

        public ValueChangedEventArgs()
        {
        }

        public ValueChangedEventArgs(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        #endregion CONSTRUCTOR
    }
}