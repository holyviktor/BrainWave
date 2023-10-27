using BrainWave.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.Interfaces
{
    public interface IFiltersService
    {
        public IQueryable<Article> Search(IQueryable<Article> articles, string searchParam);
        public IQueryable<Article> Filter(IQueryable<Article> articles, int categoryId);
        public IQueryable<Article> Sort(IQueryable<Article> articles, string orderType, string orderMode);
    }
}
