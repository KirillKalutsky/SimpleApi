using SimpleApi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleApi.DB;

namespace SimpleApi.Repositories
{
    public class AuthorRepository
    {
        private readonly JournalContext context;

        public AuthorRepository(JournalContext context)
        {
            this.context = context;
        }

        public void Insert(Author author)
        {
            context.Authors.Add(author);
            SaveChanges();
        }

        public Author FindById(Guid authorId)
        {
            return context.Authors
                .Include(author=>author.Works)
                .Where(author => author.Id==authorId)
                .FirstOrDefault();
        }

        public bool AddArticleById(Author author, Guid articleId)
        {
            var article = context.Articles
                .Where(article => article.Id == articleId)
                .FirstOrDefault();

            if(article == null)                
                return false;

            author.Works.Add(article);
            SaveChanges();
            return true;
        }

        public void Remove(Author author)
        {
            context.Authors.Remove(author);
            SaveChanges();
        }

        public IEnumerable<Guid> GetAll()
        {
            return context.Authors
                .Select(author => author.Id);
        }

        public void SaveChanges() => context.SaveChanges();
    }
}
