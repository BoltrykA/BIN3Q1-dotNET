﻿using School.Repository;
using SchoolApp.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Repository
{
    public class SchoolBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {

        protected readonly SchoolContext _dbContext;
        public SchoolBaseRepository(SchoolContext context)
        {
            _dbContext = context;
        }


        public void Insert(TEntity entity)
        {

            _dbContext.Set<TEntity>().Add(entity);

            SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            SaveChanges();
        }

        public IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate).ToList();

        }

        public IList<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public bool Save(TEntity entity, Expression<Func<TEntity, bool>> predicate)
        {
            TEntity ent = (SearchFor(predicate)).FirstOrDefault();

            if (ent == null)
            {
                Insert(entity);
                return true;
            }
            SaveChanges();
            return false;
        }

        protected void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.InnerException.Message);
            }
        }
    }
}
