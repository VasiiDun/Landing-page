﻿using System;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data;

using Repository.Interfaces;

namespace Repository
{

    public sealed class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
 
        readonly DbContext Context;

     
        readonly DbSet<TEntity> DbSet;


        public GenericRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }


        //public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        //{
        //    IQueryable<TEntity> query = DbSet;
        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //        .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        //    return orderBy?.Invoke(query).ToList() ?? query.ToList();
        //}
        public IEnumerable<TEntity> GetAll()
        {
            return DbSet.AsEnumerable();
        }

        
        public TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

    
        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

 
        public void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == System.Data.Entity.EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }

            DbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = System.Data.Entity.EntityState.Modified;
        }

      
        public void Save()
        {
            Context.SaveChanges();
        }
    }
}