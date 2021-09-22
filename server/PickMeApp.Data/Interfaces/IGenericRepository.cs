﻿using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> ListAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Guid id);
    }
}
