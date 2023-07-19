using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T,bool>> criteria)
        {
            Criteria = criteria;
        }

        //这段代码定义了一个只读属性 Criteria，该属性是一个 lambda 表达式，用于表示某种条件。在特定上下文中，它可以用于筛选、过滤或查询对象集合。
        //This code defines a read-only attribute Criteria, which is a lambda expression used to represent a certain condition. 
        //In a specific context, it can be used to filter, filter, or query a collection of objects.
        public Expression<Func<T, bool>> Criteria {get;}

        //这段代码定义了一个只读属性 Include，该属性是一个列表，用于存储包含的 lambda 表达式。
        //在初始化时，它被赋予一个空的列表对象。这种设计可以用于指定需要包含的相关实体或导航属性，并且默认情况下为空列表。
        //This code defines a read-only attribute, Include, which is a list used to store the contained lambda expressions. 
        //At initialization, it is assigned an empty list object. 
        //This design can be used to specify the relevant entities or navigation attributes that need to be included, 
        //and is an empty list by default.
        public List<Expression<Func<T, object>>> Include {get;} = 
        new List<Expression<Func<T, object>>>();


        //这段代码定义了一个受保护的方法 AddInclude，用于向 Include 属性添加包含的 lambda 表达式。
        //这个方法可以在当前类或派生类的内部使用，以便动态地添加需要包含的实体或导航属性
        //This code defines a protected method called AddInclude, which is used to add the included lambda expression to the Include property. 
        //This method can be used internally within the current or derived class to dynamically add entities or navigation attributes that need to be included
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Include.Add(includeExpression);
        }
    }
}