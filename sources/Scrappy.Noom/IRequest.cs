﻿namespace Scrappy.Noom
{
    public interface IRequest
    {
        string[] Path { get; }

        IParameters Parameters { get; }

        dynamic Payload { get; }

        string ToString();
    }
}