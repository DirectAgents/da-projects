﻿using EomTool.Domain.Abstract;
using EomToolWeb.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;

namespace EomToolWeb
{
    public static class Extensions
    {
        public static DateTime FirstDayOfMonth(this DateTime dateTime, int addMonths = 0)
        {
            var firstOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            return firstOfMonth.AddMonths(addMonths);
        }

        public static void Set(this HttpSessionState session, string key, object value)
        {
            object currentValue = session[key];
            if (currentValue != null && !currentValue.Equals(value))
                session[key] = value;
        }

        public static T SingleCustomAttribute<T>(this Type type) where T : Attribute
        {
            var attrType = typeof(T);
            var result = (T)type.GetCustomAttributes(attrType, false).FirstOrDefault();
            if (result == null)
                throw new Exception("Type " + type.FullName + " does not have the attribute " + attrType.FullName + ".");
            return result;
        }

        public static T GetSection<T>(this Configuration config) where T : ConfigurationSection
        {
           return (T)config.GetSection(typeof(T).SingleCustomAttribute<ConfigurationSectionAttribute>().Name);
        }

        public static IEnumerable<SelectListItem> ChooseMonthListItems(this IDAMain1Repository daMain1Repository, DateTime? minDate = null, DateTime? includeDate = null)
        {
            var daDatabases = daMain1Repository.DADatabases.Where(d => d.initialized);
            if (minDate.HasValue)
            {
                if (includeDate.HasValue)
                    daDatabases = daDatabases.Where(d => d.effective_date.HasValue && (d.effective_date >= minDate.Value || d.effective_date == includeDate.Value));
                else
                    daDatabases = daDatabases.Where(d => d.effective_date.HasValue && d.effective_date >= minDate.Value);
            }
            var listItems = from d in daDatabases
                                .OrderByDescending(d => d.effective_date)
                                .ToList()
                            select new SelectListItem
                            {
                                Text = d.name,
                                Value = d.effective_date.Value.ToShortDateString()
                            };
            return listItems;
        }

        public static SelectList ChooseMonthSelectList(this IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig, DateTime? minDate = null)
        {
            DateTime currentEomDate = eomEntitiesConfig.CurrentEomDate;
            DateTime? includeDate = null;
            if (minDate.HasValue && currentEomDate < minDate.Value)
                includeDate = currentEomDate;
            return new SelectList(daMain1Repository.ChooseMonthListItems(minDate, includeDate), "Value", "Text", currentEomDate.ToShortDateString());
        }
    }
}