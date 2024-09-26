using Lnk.DataAccess.DataAccess;
using System;

namespace Lnk.DataAccess.Repository;

public class UnitOfWork
{
	private readonly LnkDbContext _context;
	// private IBookRepository? _bookRepository;

	public UnitOfWork(LnkDbContext context)
	{
		_context = context;
	}

	// public IBookRepository BookRepository => _bookRepository ??= new BookRepository(_context);

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}

	public void Dispose()
	{
		if (_context != null)

			_context.Dispose();
	}
}