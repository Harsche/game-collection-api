using System;

namespace GameCollectionAPI.Exceptions;

public class DuplicateUsernameException : Exception
{
    public DuplicateUsernameException() : base("Username is already being used.") { }
}
