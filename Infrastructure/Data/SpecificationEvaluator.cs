using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity:BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec)
        {
            var query = inputQuery;
            if(spec.Criteria!=null)
            {
                query=query.Where(spec.Criteria);//如果规范的 Criteria 不为 null，则将其应用于查询，通过 Where 方法进行筛选。
                //If the Criteria in the specification are not null, apply them to the query and filter through the Where method.
            }
            //这行代码使用 Aggregate 方法将规范的 Include 列表中的每个 lambda 表达式应用于查询。
            //它从起始查询 query 开始，并依次将每个 lambda 表达式应用于查询的结果，通过 Include 方法添加相关实体或导航属性
            //This line of code uses the Aggregate method to apply each lambda expression in the canonical Include list to the query. 
            //It starts with the starting query and sequentially applies each lambda expression to the results of the query, 
            //adding relevant entities or navigation attributes through the Include method
            query=spec.Include.Aggregate(query,(current,include)=>current.Include(include));
            return query;
        }
    }
}