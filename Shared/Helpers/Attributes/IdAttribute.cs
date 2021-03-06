﻿namespace Shared.Helpers.Attributes
{
    public class IdAttribute : AAttribute
    {
        public static readonly IdAttribute Yes = new IdAttribute(true);
        public static readonly IdAttribute No = new IdAttribute(false);
        public static readonly IdAttribute Default = No;
        public const string NAME = "id";


        public bool IsId { get; private set; }

        public override string Name => NAME;

        public override object Value
        {
            get => IsId;
            protected set => IsId = (bool) value;
        }

        public override object DefaultValue { get; } = true;


        public IdAttribute()
        {
        }

        public IdAttribute(bool description) : base(description)
        {
        }
    }
}