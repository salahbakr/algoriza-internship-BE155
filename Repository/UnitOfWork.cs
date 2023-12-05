using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBaseRepository<Specialization> Specializations { get; private set; }
        public IBaseRepository<Appointment> Appointments { get; private set; }
        public IBaseRepository<Request> Requests { get; private set; }
        public IBaseRepository<Booking> Bookings { get; private set; }
        public IAuthRepository AuthRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;

            Specializations = new BaseRepository<Specialization>(_context);
            Appointments = new BaseRepository<Appointment>(_context);
            Requests = new BaseRepository<Request>(_context);
            Bookings = new BaseRepository<Booking>(_context);
            AuthRepository = new AuthRepository(userManager);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
