﻿using System.Linq.Expressions;

namespace ZeroMock;

public static class It
{
    public static T IsAny<T>() => default;

    public static T Is<T>(Expression<Func<T, bool>> match) => default;
}
