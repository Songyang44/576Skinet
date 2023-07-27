using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISpecification<T>
    {
        //Expression<Func<T, bool>>，A lambda expression that accepts a parameter of type T and returns a bool value
        Expression<Func<T,bool>> Criteria{get;}
        //This code defines a read-only attribute, Include, which is a list used to store the contained lambda expressions.
        List<Expression<Func<T, object>>> Include{get;}
        Expression<Func<T, object>> OrderBy{get;}

        Expression<Func<T, object>> OrderByDescending{get;}

        int Take{get;}
        int Skip{get;}
        Boolean IsPaginEnabled{get;}
    }
}