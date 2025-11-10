using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        // Dictionary to store all Repositories 
        // Key(string) --> Type of Repository & Value(GenericRepository<BaseEntity>) --> Repository object
        private readonly Dictionary<Type, object> _repositories;

        //private Hashtable _repositories; // Hashtable is the non-generic version of Dictionary [Accept object for key and value]

        // Ask CLR to create object from DbContext [Implicitly]
        public UnitOfWork(StoreContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>(); // Initialize the dictionary
        }

        // Store Repositories on key value pair [HashSet] as if we need it again we can get it from the dictionary and no need to call UnitOfWork again 
        // Key --> Type of Repository & Value --> GenericRepository object
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            Type type = typeof(TEntity); // Get the name of the type of entity

            // Check if the repository required is already created or not
            if (!_repositories.ContainsKey(type))
            {
                GenericRepository<TEntity>? repository = new GenericRepository<TEntity>(_context);

                //GenericRepository<BaseEntity>? repository = Activator.CreateInstance(typeof(GenericRepository<>).MakeGenericType(typeof(TEntity)), _context) as GenericRepository<BaseEntity>;

                _repositories.Add(type, repository);
            }

            return (IGenericRepository<TEntity>)_repositories[type]; // Return the repository from the dictionary

        }

        public async Task<int> Complete()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 DB Error: " + ex.InnerException?.Message ?? ex.Message);
                throw;
            }
            //return await _context.SaveChangesAsync(); // Save Changes to Database [Return number of affectedRows]
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync(); // Dispose the context
        }

    }
}
