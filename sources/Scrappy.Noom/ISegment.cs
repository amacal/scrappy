﻿namespace Scrappy.Noom
{
    public interface ISegment
    {
        bool IsActive { get; }

        string Name { get; }

        void Activate();
    }
}