using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using entity.Models;
using Repository.Contract;


namespace Repository.EfCore.Abstarct
{
    public class RepositoryBase : IRepositoryBase<Post>
    {
     


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Post> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Post> GetAll(Expression<Func<Post, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Post GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Post entity)
        {
            throw new NotImplementedException();
        }


        public void Update(int id,Post entity)
        {
            throw new NotImplementedException();
        }
    }
}
