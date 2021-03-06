﻿using System.Collections;
using System.Linq;

namespace Shared.Helpers.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty(IEnumerable This)
            => This == null || !This.Cast<object>().Any();
    }
}