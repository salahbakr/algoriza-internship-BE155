using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository AuthRepository { get; }
        IBaseRepository<Specialization> Specializations { get; }
        IBaseRepository<Appointment> Appointments { get; }
        IBaseRepository<Request> Requests { get; }
        IBaseRepository<Booking> Bookings { get; }
        IBaseRepository<DayTime> Time { get; }

        int Complete();
    }
}
