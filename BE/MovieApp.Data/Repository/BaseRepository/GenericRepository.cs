using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieApp.Data.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Data.Repository.BaseRepository
{
    public class GenericRepository<T> where T : class
    {
        protected MovieAppDBContext _context;

        public GenericRepository()
        {
            _context ??= new MovieAppDBContext();
        }

        //Get ALL
        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        //Get By ID (int)
        public T GetById(long id)
        {
            return _context.Set<T>().Find(id);
        }
        public async Task<T> GetByIdAsync(long id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        //Get By ID (string)
        public T GetById(string code)
        {
            return _context.Set<T>().Find(code);
        }
        public async Task<T> GetByIdAsync(string code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

        //Create
        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
        public async Task<T> CreateAsync(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        //Update
        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }
        public async Task<T> UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        //Delete
        public bool Remove(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        //Check EXIST
        public async Task<bool> EntityExistsByPropertyAsync(string propertyId, long id)
        {
            return await _context.Set<T>()
                .AnyAsync(e => EF.Property<long>(e, propertyId) == id);
        }
        public async Task<bool> EntityExistsByPropertyAsync(string propertyName, string value)
        {
            var lowerCaseValue = value.ToLower(); 
            return await _context.Set<T>()
                .AnyAsync(e => EF.Property<string>(e, propertyName).ToLower() == lowerCaseValue);
        }

        public T GetById(Guid code)
        {
            return _context.Set<T>().Find(code);
        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            return await _context.Set<T>().FindAsync(code);
        }

    }
}
