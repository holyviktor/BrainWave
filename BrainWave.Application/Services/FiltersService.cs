using BrainWave.Core.Entities;
using BrainWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainWave.Core.Interfaces;

namespace BrainWave.Application.Services
{
    public class FiltersService:IFilterInterface
    {
        private readonly BrainWaveDbContext _dbContext;
        public FiltersService(BrainWaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Article> ApplyFilters(string search, int? categoryId, string? sortType, string sortOrder)
        {
            var articles = from s in _dbContext.Articles
                           select s;
            if (!search.IsNullOrEmpty())
            {
                articles = Search(articles, search);
            }
            if (categoryId != null)
            {
                articles = Filter(articles, (int)categoryId);
            }
            if (!sortType.IsNullOrEmpty())
            {
                if (sortOrder == null)
                {
                    sortOrder = "Desc";
                }
                articles = Sort(articles, sortType, sortOrder);
            }
            return articles.Include(c => c.Comments)
                    .Include(c => c.User)
                    .Include(c => c.Likes)
                    .Include(c => c.Savings)
                    .ToList();
        }
        public IQueryable<Article> Search(IQueryable<Article> articles, string searchParam)
        {
            if (searchParam.IsNullOrEmpty())
            {
                throw new ArgumentException();
            }
            articles = articles.Include(c=>c.Category)
                .Where(c=>c.Title.Contains(searchParam)
                ||c.Text.Contains(searchParam)
                ||c.Category.Name.Contains(searchParam));
            return articles;
        }
        public IQueryable<Article> Filter(IQueryable<Article> articles, int categoryId)
        {
            var category = _dbContext.Categories.FirstOrDefault(x=>x.Id == categoryId);
            if (category == null)
            {
                throw new ArgumentException();
            }
            articles = articles.Where(c=>c.CategoryId == categoryId);

            return articles;
        }
        public IQueryable<Article> Sort(IQueryable<Article> articles, string orderType, string orderMode)
        {
            if (orderMode != null && orderType != null) {
                string sortOrder = orderType + "_" + orderMode;
                switch (sortOrder)
                {
                    case "Date_Desc":
                        articles = articles.OrderByDescending(m => m.Date);
                        break;
                    case "Date_Asc":
                        articles = articles.OrderBy(m => m.Date);
                        break;
                    case "Title_Desc":
                        articles = articles.OrderByDescending(m => m.Title);
                        break;
                    case "Title_Asc":
                        articles = articles.OrderBy(m => m.Title);
                        break;
                    case "Popularity_Desc":
                        articles = articles.OrderByDescending(m => m.Likes.Count);
                        break;
                    case "Popularity_Asc":
                        articles = articles.OrderBy(m => m.Likes.Count);
                        break;
                    case "Price_Desc":
                        articles = articles.OrderByDescending(m => m.Price);
                        break;
                    case "Price_Asc":
                        articles = articles.OrderBy(m => m.Price);
                        break;
                    default:
                        articles = articles.OrderBy(m => m.Date);
                        break;
                }
            }
            return articles;
        }
    }
}
