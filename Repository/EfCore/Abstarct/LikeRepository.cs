using entity.Models;
using Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EfCore.Abstarct
{
    public class LikeRepository : ILikeRepository
    {
        private readonly MyContext _context;
        public LikeRepository(MyContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Like> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Like> GetAll(Expression<Func<Like, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Like? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Like entity)
        {   
            _context.likes.Add(entity);
            _context.SaveChanges();


        }

        public void Update(int id, Like entity)
        {
            throw new NotImplementedException();
        }
    }
}
