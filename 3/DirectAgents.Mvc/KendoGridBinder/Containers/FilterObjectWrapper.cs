﻿using System.Collections.Generic;

namespace DirectAgents.Mvc.KendoGridBinder.Containers
{
    public class FilterObjectWrapper
    {
        public FilterObjectWrapper(string logic, IEnumerable<FilterObject> filterObjects)
        {
            Logic = logic;
            FilterObjects = filterObjects;
        }

        public string LogicToken
        {
            get
            {
                switch (Logic)
                {
                    case "and":
                        return "&&";
                    case "or":
                        return "||";
                    default:
                        return null;
                }
            }
        }

        public string Logic { get; set; }

        public IEnumerable<FilterObject> FilterObjects { get; set; }
    }
}