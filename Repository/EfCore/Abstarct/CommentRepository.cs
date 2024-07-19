using entity.Models;
using OpenQA.Selenium;
using Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EfCore.Abstarct
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyContext _context;
        public CommentRepository(MyContext context)
        {
            _context = context;
        }
        public void Delete(int id)
        {
            var comment = _context.comments.FirstOrDefault(x=>x.CommentId == id);
            if (comment == null)
            {
                throw new NotFoundException("Post not found"); // Özelleştirilmiş bir exception türü kullanarak, post bulunamadığında istisna fırlatma
            }
            _context.comments.Remove(comment);
            _context.SaveChanges();

        }

        public IQueryable<Comment> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Comment> GetAll(Expression<Func<Comment, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Comment? GetById(int id)
        {
            return _context.comments.FirstOrDefault(c => c.CommentId == id);
        }

        public void Insert(Comment entity)
        {
            var addedEntity =_context.Entry(entity);
            addedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Added;
          
            _context.SaveChanges();
        }

        public void Update(int id, Comment entity)
        {
            throw new NotImplementedException();
        }

    }
}
