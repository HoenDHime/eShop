using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;

namespace E_shop.Code
{
    public static class LocalizationExtensions
    {
        //Метод вызова локальных ресурсов в представлении
        //Method call in the view of local resources
        public static string Resource(this HtmlHelper html, string expr, params object[] args)
        {
            string path = ((RazorView)html.ViewContext.View).ViewPath;

            ResourceExpressionFields fields = (ResourceExpressionFields)(new ResourceExpressionBuilder()).ParseExpression(
                expr,
                typeof(string),
                new ExpressionBuilderContext(path)
                );

            return (!string.IsNullOrEmpty(fields.ClassKey))
                ? string.Format((string)html.ViewContext.HttpContext.GetGlobalResourceObject(
                    fields.ClassKey,
                    fields.ResourceKey,
                    CultureInfo.CurrentUICulture),
                    args)

                : string.Format((string)html.ViewContext.HttpContext.GetLocalResourceObject(
                    path,
                    fields.ResourceKey,
                    CultureInfo.CurrentUICulture),
                    args);
        }
    }
}