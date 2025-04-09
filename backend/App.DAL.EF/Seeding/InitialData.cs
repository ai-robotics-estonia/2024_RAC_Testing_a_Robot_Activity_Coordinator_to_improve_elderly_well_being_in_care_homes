namespace App.DAL.EF.Seeding;

public static class InitialData
{
    public static readonly (string roleName, Guid? id)[]
        Roles =
        [
            ("admin", null),
            ("user", null),
            ("sysadmin", null),
        ];

    public static readonly (string name, string password, Guid? id, string[] roles)[]
        Users =
        [
            ("akaver@akaver.com", "Kala.Maja.101", null, new string[] { "admin", "sysadmin", "user" }),
            ("user@taltech.ee", "Kala.Maja.123", null, new string[] { "admin" }),
            ("temi@pihlakodu.ee", "Temi.Admin.123", null, new string[] { "user" }),
        ];
}