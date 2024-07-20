namespace HappyGift.Data.Seeding
{
    using System;

    internal class UserSeederConstants
    {
        public const string DefaultPassword = "123123";

        public static readonly (string UserName, string Name, DateTime BirthDate)[] Users = new[]
        {
            ("diana.apple@gift.com", "Diana Apple", new DateTime(1990, 1, 1)),
            ("john.doe@gift.com", "John Doe", new DateTime(1985, 6, 15)),
            ("jane.smith@gift.com", "Jane Smith", new DateTime(1992, 3, 22)),
            ("alice.jones@gift.com", "Alice Jones", new DateTime(1988, 11, 5)),
            ("bob.brown@gift.com", "Bob Brown", new DateTime(1995, 9, 10)),
        };
    }
}
