using System;
using FilesChecker;

namespace Paulov.Tarkov.Minimal.Models;

public class FakeFileCheckerResult : ICheckResult
{
    public TimeSpan ElapsedTime => TimeSpan.Zero;

    public Exception Exception => null;
}