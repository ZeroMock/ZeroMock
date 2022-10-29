namespace ZeroMock;

internal class ArgumentMatcher
{
    private readonly List<Condition> _matchers;

    public ArgumentMatcher(List<Condition> matchers)
    {
        _matchers = matchers;
    }

    public bool Match(object[] args)
    {
        if (_matchers.Count == 0)
        {
            return true;
        }

        if (args.Count() != _matchers.Count)
        {
            return false;
        }

        if (args.Count() == 0)
        {
            return true;
        }

        for (int i = 0; i < _matchers.Count; i++)
        {
            if (!_matchers[i].Match(args[i]))
            {
                return false;
            }
        }

        return true;
    }
}
