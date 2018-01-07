﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseManager.Services.DataService
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task InsertAsync(T item);
        Task UpdateAsync(T item);
        Task RemoveAsync(T item);
        Task<Dictionary<string, Dictionary<string, object>>> GetAttributesDictionary();
    }
}