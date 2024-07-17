using System.Web;

namespace Demo_Delivery.IntegrationTests;

public static class Ex
{
    public static string GetQueryString(this object obj)
    {
        var properties = from p in obj.GetType().GetProperties()
            where p.GetValue(obj, null) != null
            select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

        return string.Join("&", properties.ToArray());
    }
}