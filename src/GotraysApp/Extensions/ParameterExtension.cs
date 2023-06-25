namespace GotraysApp;

public static class ParameterExtension
{
    /// <summary>
    /// 解析object为url参数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetPargmeter(this object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        var queryString = string.Empty;
        var properties = value.GetType().GetProperties();
        var parameters = properties.Select(p => $"{p.Name}={p.GetValue(value)}");
        queryString = string.Join("&", parameters);
        if (!string.IsNullOrEmpty(queryString))
        {
            queryString = "?" + queryString;
        }

        return queryString;
    }
}
