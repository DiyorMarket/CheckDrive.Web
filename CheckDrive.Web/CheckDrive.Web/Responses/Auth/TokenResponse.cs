namespace CheckDrive.Web.Responses.Auth;

public sealed record TokenResponse(string AccessToken, string RefreshToken);
