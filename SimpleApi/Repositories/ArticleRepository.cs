using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleApi.Core;
using SimpleApi.DB;

namespace SimpleApi.Repositories
{
    public class ArticleRepository
    {
        private readonly JournalContext journalContext;

        public ArticleRepository(JournalContext journalContext)
        {
            this.journalContext = journalContext;
        }

        public void Insert(Article article)
        {
            journalContext.Articles.Add(article);
            journalContext.SaveChanges();
        }

        public Article FindById(Guid id)
        {
            return journalContext.Articles
                .Where(article => article.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Article> FindByCreatorId(Guid creatorId)
        {
            var creator = journalContext.Authors
                .Include(author => author.Works)
                .Where(author => author.Id == creatorId)
                .FirstOrDefault();

            if (creator == null)
                return null;

            return creator.Works;
        }

        public IEnumerable<Guid> GetAll()
        {
            return journalContext.Articles
                .Select(article => article.Id);
        }

        public void Delete(Article article)
        {
            journalContext.Articles.Remove(article);
        }

        /*public void Update(Article article)
        {
            journalContext.Articles.
        }*/
    }
}
