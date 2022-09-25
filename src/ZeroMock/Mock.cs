﻿using System.Linq.Expressions;

namespace ZeroMock;

public class Mock<T> where T : class
{
    public SetupResult<TReturn> Setup<TReturn>(Expression<Func<T, TReturn>> expression)
    {
        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            Patcher.Patch(mce.Method);
            return new SetupResult<TReturn>(mce.Method, _object);
        }

        throw new InvalidOperationException("Setup only works for methods");
    }

    public SetupResult Setup(Expression<Action<T>> expression)
    {
        var body = expression.Body;

        if (body is MethodCallExpression mce)
        {
            Patcher.Patch(mce.Method);
            return new SetupResult(_object);
        }

        throw new InvalidOperationException("Setup only works for methods");
    }

    private static T Create()
    {
        Patcher.SetupHooks<T>();
        var instance = InstanceFactory.CreateNew<T>();
        return instance;
    }

    private readonly T _object = Create();
    public T Object => _object;
}