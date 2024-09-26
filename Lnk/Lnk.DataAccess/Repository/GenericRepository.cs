using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lnk.DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Lnk.DataAccess.Repository
{
	public class GenericRepository<T> where T : class
	{
		private readonly LnkDbContext _context;

		public GenericRepository(LnkDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null)
		{
			if (expression == null)
			{
				return await _context.Set<T>().ToListAsync();
			}

			return await _context.Set<T>().Where(expression).ToListAsync();
		}

		public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression)
		{
			return await _context.Set<T>().SingleOrDefaultAsync(expression);
		}

		// CreateAsync()
		public async Task CreateAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
		}

		public void Update(T entity)
		{
			_context.Set<T>().Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		public void Delete(T entity)
		{
			_context.Set<T>().Attach(entity);
			_context.Entry(entity).State = EntityState.Deleted;
		}

		public async Task CommitAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}