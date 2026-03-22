namespace LabWork20.Data;

public class Geo_auth
{
    public string? Geo { get; set; }

    public string? Login { get; set; }
    public string? Password { get; set; }

    public void GetUserLocation() => Console.WriteLine(Geo);
    public void GetLogin() => Console.WriteLine(Login);
    public void GetInformation() => Console.WriteLine("Some information");
}
