using Microsoft.EntityFrameworkCore;
using System;

namespace QLBH_Dion.Util
{
    public static class CustomQuery
    {
        //Cách sử dụng CustomQuery
        //1. Thêm vào DB context
        //ModelBuilder.HasDbFunction(typeof(CustomQuery).GetMethod(nameof(CustomQuery.ToCustomString))).HasTranslation(
        //     e =>
        //     {
        //    return new SqlFunctionExpression(functionName: "format", arguments: new[]{
        //                     e.First(),
        //                     new SqlFragmentExpression("'dd/MM/yyyy HH:mm:ss'")
        //                 }, nullable: true, new List<bool>(), type: typeof(string), typeMapping: new StringTypeMapping("", DbType.String));
        //});

        // modelBuilder.HasDbFunction(typeof(CustomQuery).GetMethod(nameof(CustomQuery.ToDateString))).HasTranslation(
        //     e =>
        //     {
        //    return new SqlFunctionExpression(functionName: "format", arguments: new[]{
        //                     e.First(),
        //                     new SqlFragmentExpression("'dd/MM/yyyy'")
        //                 }, nullable: true, new List<bool>(), type: typeof(string), typeMapping: new StringTypeMapping("", DbType.String));
        //});
        //2.Sau đó trong linq sử dụng để filter theo serverside

        public static string ToCustomString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }
        public static string ToDateString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
        public static string ToDateMonthString(this DateTime date)
        {
            return date.ToString("dd/MM");
        }
        public static string ToMonthYearString(this DateTime date)
        {
            return date.ToString("MM/yyyy");
        }
    }
}
