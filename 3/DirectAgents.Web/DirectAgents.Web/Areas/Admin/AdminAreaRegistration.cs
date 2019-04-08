﻿using System.Web.Mvc;

namespace DirectAgents.Web.Areas.Admin
{
    /// <summary>
    /// Admin Area Registration
    /// </summary>
    /// <seealso cref="System.Web.Mvc.AreaRegistration" />
    public class AdminAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the name of the area to register.
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "History", id = UrlParameter.Optional }
            );
        }
    }
}