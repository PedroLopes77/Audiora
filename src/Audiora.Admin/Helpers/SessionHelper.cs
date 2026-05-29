namespace Audiora.Admin.Helpers;

public static class SessionHelper
{
    private static string? _token;
    private static string? _userName;
    private static string? _userEmail;

    public static void SaveToken(string token) => _token = token;
    public static string? GetToken() => _token;

    public static void SaveUser(string name, string email)
    {
        _userName  = name;
        _userEmail = email;
    }

    public static string GetUserName()  => _userName  ?? "";
    public static string GetUserEmail() => _userEmail ?? "";
    public static bool IsLoggedIn()     => !string.IsNullOrEmpty(_token);

    public static void Logout()
    {
        _token     = null;
        _userName  = null;
        _userEmail = null;
    }
}