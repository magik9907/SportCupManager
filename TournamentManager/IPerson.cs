﻿namespace SportCupManagerConsole
{
    public interface IPerson
    {
        byte Age { get; set; }
        string Firstname { get; set; }
        string Fullname { get; }
        string Lastname { get; set; }
    }
}