namespace Common;
public static class JsDateEncoder
{
    static readonly string jsTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

    public static string JsDate(DateTime date) =>
        date.ToString(jsTimeFormat);
    public static string JsDate(int year, int month, int day) =>
        JsDate(new DateTime(year, month, day));
}