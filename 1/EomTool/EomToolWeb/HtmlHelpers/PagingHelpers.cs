﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomToolWeb.Models;
using System.Text;

namespace EomToolWeb.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
        PagingInfo pagingInfo,
        Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            if (pagingInfo.TotalPages > 1)
                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a"); // Construct an <a> tag
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();
                    if (i == pagingInfo.CurrentPage)
                        tag.AddCssClass("selected");
                    result.Append(tag.ToString());
                }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}