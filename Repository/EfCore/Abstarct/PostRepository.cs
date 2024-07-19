
using DTO;
using entity.Models;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EfCore.Abstarct
{
    public class PostRepository : IPostRepository
    {
        private readonly MyContext _context;
        public PostRepository(MyContext context)
        {
            _context = context;
        }
      

        public void Delete(int id)
        {
            var Post =_context.Posts.FirstOrDefault(x=>x.PostId == id);
            if (Post == null)
            {
                throw new NotFoundException("Post not found"); // Özelleştirilmiş bir exception türü kullanarak, post bulunamadığında istisna fırlatma
            }
            _context.Posts.Remove(Post);
            _context.SaveChanges();

        }
        public IQueryable<Post> GetAll()
        {
            return _context.Posts
                .Include(p => p.User)
                .Include(p => p.Likes)
                .Include(p => p.Comments);
        }

        public IQueryable<Post> GetAll(Expression<Func<Post, bool>> predicate)
        {
             throw new NotImplementedException();
        }

        public IQueryable<Post> GetById(int id)
        {
            return _context.Posts
               .Include(p => p.User)
               .Include(p => p.Likes)
               .Include(p => p.Comments);
        }

        public void Insert(Post entity)
        {
            _context.Add(entity);
            _context.SaveChanges();

        }

     
        public void Update(int id, Post entity)
        {
            var existingPost = _context.Posts.FirstOrDefault(p => p.PostId == id);

            if (existingPost == null)
            {
                throw new NotFoundException("Post not found"); // Özelleştirilmiş bir exception türü kullanarak, post bulunamadığında istisna fırlatma
            }

            // Güncellenen alanları kontrol ederek sadece değişen alanları güncelliyoruz
            existingPost.Title = entity.Title;
            existingPost.Description = entity.Description;
            existingPost.Image = entity.Image;
            existingPost.IsActive = entity.IsActive;
            existingPost.UserId = entity.UserId;

            try
            {
                _context.SaveChanges(); // Değişiklikleri veritabanına kaydet
            }
            catch (DbUpdateConcurrencyException )
            {
                // İşlem sırasında concurrency hatası oluşabilir (başka bir kullanıcı aynı kaydı güncellediği için)
                // Bu durumu ele almak gerekir
                // Loglama veya kullanıcıya bilgilendirme yapılabilir
                throw new NotFoundException("Post not found");// Özelleştirilmiş bir exception türü kullanarak işlem hatasını fırlatma
            }
        }

        Post? IRepositoryBase<Post>.GetById(int id)
        {
            return _context.Posts.FirstOrDefault(y => y.PostId == id);
        }
    }
}
