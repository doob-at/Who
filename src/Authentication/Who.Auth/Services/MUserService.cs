//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using who.Auth.Context;
//using who.Auth.Entities;

//namespace who.Auth.Services
//{
//    public class MUserService: IMUserService
//    {
//        private readonly AuthDbContext _context;

//        public MUserService( AuthDbContext context)
//        {
//            _context = context;
//        }

//        public Task<List<MUser>> GetAllUsersAsync()
//        {
//            return _context.Users.ToListAsync();
//        }

//        public Task DeleteUserAsync(params Guid[] id)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
