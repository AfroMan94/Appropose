﻿using FluentResults;

namespace Appropose.Functions.FluentErrors
{
    class RuntimeError: Error
    {
        public RuntimeError(string message) : base(message)
        {
        }
    }
}
