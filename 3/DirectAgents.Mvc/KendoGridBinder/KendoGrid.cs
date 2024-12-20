﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic;

namespace DirectAgents.Mvc.KendoGridBinder
{
    public class KendoGrid<T>
    {
        public KendoGrid() { }

        public KendoGrid(KendoGridRequest request, IQueryable<T> query, bool toListBeforeOrderBy = false)
        {
            // call another method here to get filtering and sorting.
            var filtering = GetFiltering(request);
            var sorting = GetSorting(request);

            var tempQuery = query.Where(filtering);
            if (toListBeforeOrderBy)
                tempQuery = tempQuery.ToList().AsQueryable();

            tempQuery = tempQuery.OrderBy(sorting);
            total = tempQuery.Count();
            data = tempQuery.Skip(request.Skip);
            if (request.Take > 0)
                data = data.Take(request.Take);
        }

        public KendoGrid(KendoGridRequest request, IEnumerable<T> list)
        {
            var filtering = GetFiltering(request);
            var sorting = GetSorting(request);
            var query = list.AsQueryable();
            if (!string.IsNullOrEmpty(filtering))
                query = query.Where(filtering);
            if (!string.IsNullOrEmpty(sorting))
                query = query.OrderBy(sorting);
            data = query.ToList();
            total = data.Count();
            if (request.Skip > 0)
                data = data.Skip(request.Skip);
            if (request.Take > 0)
                data = data.Take(request.Take);
        }

        public KendoGrid(KendoGridRequest request, IEnumerable<T> list, int totalCount)
        {
            // Just use the request as a container
            data = list;
            total = totalCount;
        }

        public static string GetSorting(KendoGridRequest request)
        {
            if (request.SortObjects == null)
                return "";

            var expression = "";

            foreach (var sortObject in request.SortObjects)
            {
                expression += sortObject.Field + " " + sortObject.Direction + ", ";
            }

            if (expression.Length < 2)
                return "true";

            expression = expression.Substring(0, expression.Length - 2);

            return expression;
        }

        public string GetFiltering(KendoGridRequest request)
        {
            if (request.FilterObjectWrapper == null)
                return "";

            var finalExpression = "";

            foreach (var filterObject in request.FilterObjectWrapper.FilterObjects)
            {
                if (finalExpression.Length > 0)
                    finalExpression += " " + request.FilterObjectWrapper.LogicToken + " ";


                if (filterObject.IsConjugate)
                {
                    var expression1 = GetExpression(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    var expression2 = GetExpression(filterObject.Field2, filterObject.Operator2, filterObject.Value2);
                    var combined = string.Format("({0} {1} {2})", expression1, request.FilterObjectWrapper.LogicToken, expression2);
                    finalExpression += combined;
                }
                else
                {
                    var expression = GetExpression(filterObject.Field1, filterObject.Operator1, filterObject.Value1);
                    finalExpression += expression;
                }
            }

            if (finalExpression.Length == 0)
                return "true";

            return finalExpression;
        }

        public IEnumerable<T> data { get; set; }

        public int total { get; set; }
        public object aggregates { get; set; }

        private static string GetExpression(string field, string op, string param)
        {
            var p = typeof(T).GetProperty(field);

            var dataType = (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                ? p.PropertyType.GetGenericArguments()[0].Name.ToLower()
                : p.PropertyType.Name.ToLower();

            string caseMod = string.Empty;

            if (dataType == "string")
            {
                param = @"""" + param.ToLower() + @"""";
                caseMod = ".ToLower()";
            }

            if (dataType == "datetime")
            {
                var i = param.IndexOf("GMT");
                if (i > 0)
                {
                    param = param.Remove(i);
                }
                var date = DateTime.Parse(param, new CultureInfo("en-US"));
                var str = string.Format("DateTime({0}, {1}, {2})", date.Year, date.Month, date.Day);
                param = str;
            }

            string exStr;

            switch (op)
            {
                case "eq":
                    exStr = string.Format("{0}{2} == {1}", field, param, caseMod);
                    break;

                case "neq":
                    exStr = string.Format("{0}{2} != {1}", field, param, caseMod);
                    break;

                case "contains":
                    exStr = string.Format("{0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "doesnotcontain":
                    exStr = string.Format("!{0}{2}.Contains({1})", field, param, caseMod);
                    break;

                case "startswith":
                    exStr = string.Format("{0}{2}.StartsWith({1})", field, param, caseMod);
                    break;

                case "endswith":
                    exStr = string.Format("{0}{2}.EndsWith({1})", field, param, caseMod);
                    break;
                case "gte":
                    exStr = string.Format("{0}{2} >= {1}", field, param, caseMod);
                    break;
                case "gt":
                    exStr = string.Format("{0}{2} > {1}", field, param, caseMod);
                    break;
                case "lte":
                    exStr = string.Format("{0}{2} <= {1}", field, param, caseMod);
                    break;
                case "lt":
                    exStr = string.Format("{0}{2} < {1}", field, param, caseMod);
                    break;
                default:
                    exStr = "";
                    break;
            }

            return exStr;
        }
    }
}
