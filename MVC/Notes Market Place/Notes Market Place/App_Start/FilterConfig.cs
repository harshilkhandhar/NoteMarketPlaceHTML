﻿using System.Web;
using System.Web.Mvc;

namespace Notes_Market_Place
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
