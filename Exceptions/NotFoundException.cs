﻿namespace Test1retake.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }
    public NotFoundException(string? message) : base(message)
    {
    }
    public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}