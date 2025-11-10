using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity // BaseEntity : Parent for All Models
    {
        // Static Query
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);


        // Dynamic Query
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);


        // Get Count of Total Records
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);


        Task AddAsync(T entity); // Add Entity to Database

        void Update(T entity);

        void Delete(T entity);
    }
}
