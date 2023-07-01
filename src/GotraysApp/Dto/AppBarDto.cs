namespace GotraysApp.Dto;

public class AppBarDto
{
    public string Key { get; set; }

    public string Name { get; set; }

    public string Icon { get; set; }

    public string Href { get; set; }

    public AppBarDto(string name, string href, string icon)
    {
        Key = Guid.NewGuid().ToString();
        Name = name;
        Icon = icon;
        Href = href;
    }

    public AppBarDto()
    {
        Key = Guid.NewGuid().ToString();
    }
}
