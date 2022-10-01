﻿namespace ZeroMock;

internal class SetupResultAccessor : ISetupResultAccessor
{
    private readonly SetupResult _setupResult;

    internal SetupResultAccessor(SetupResult setupResult)
    {
        _setupResult = setupResult;
    }

    public Func<object>? GetReturnValue => null;

    public int InvocationAmount => _setupResult.InvocationAmount;
    public Action? Callback => _setupResult.GetCallback;

    public bool Match(object[] args) => _setupResult.Match(args);
}