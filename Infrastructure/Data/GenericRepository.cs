using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsyne(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        


        //定义了一个私有方法 ApplySpecification，用于将给定的规范应用于查询，并获取查询结果。
        //它使用一个上下文 _context 来获取实体 T 的数据集，并将其转换为 IQueryable<T>，
        //然后将查询和规范传递给 SpecificationEvaluator<T>.GetQuery 方法进行处理，并返回查询结果。
        // A private method ApplySpecification has been defined to apply a given specification to a query and obtain the query results. 
        //It uses a context_ Context to obtain the dataset of entity T and convert it into IQueryable<T>, 
        //then pass the query and specification to the SpecificationEvaluator<T>. GetQuery method for processing, 
        //and return the query results.
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            //这行代码调用 SpecificationEvaluator<T>.GetQuery 静态方法，
            //并传递 _context.Set<T>().AsQueryable() 作为输入查询和给定的规范 spec。然后，它返回从静态方法获取的查询结果。
            //This line of code calls the SpecificationEvaluator<T>. GetQuery static method and passes the_ Context. Set<T>(). 
            //AsQueryable() as input query and given specification spec. 
            //Then, it returns the query results obtained from the static method.
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(),spec);
        }
    }
} 