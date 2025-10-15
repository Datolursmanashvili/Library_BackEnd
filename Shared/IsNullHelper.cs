using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared;
public static class IsNullHelper
{
    public static bool IsNull<T>(this T root, Expression<Func<T, object>> getter)
    {
        var visitor = new IsNullVisitor
        {
            CurrentObject = root
        };
        visitor.Visit(getter);
        return visitor.IsNull;
    }

    public static bool IsNull<T>(this T obj)
    {
        return obj == null;
    }

    public static bool IsNotNull<T>(this T obj)
    {
        return obj != null;
    }

    public static string ToObjectName<T>(this T obj)
    {
        if (obj.IsNull())
        {
            return typeof(T).Name;
        }
        return string.Empty;
    }
}

public class IsNullVisitor : ExpressionVisitor
{
    public bool IsNull { get; private set; }

    public object CurrentObject { get; set; }

    protected override Expression VisitMember(MemberExpression node)
    {
        base.VisitMember(node);
        if (CheckNull())
        {
            return node;
        }


        var member = (PropertyInfo)node.Member;
        CurrentObject = member.GetValue(CurrentObject, null);
        CheckNull();
        return node;
    }

    private bool CheckNull()
    {
        if (CurrentObject == null)
        {
            IsNull = true;
        }
        return IsNull;
    }
}