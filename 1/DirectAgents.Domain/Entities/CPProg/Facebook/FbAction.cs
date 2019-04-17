using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook
{
    /// <summary>
    /// Base facebook entity action.
    /// </summary>
    public class FbAction
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the action type identifier.
        /// </summary>
        /// <value>
        /// The action type identifier.
        /// </value>
        public int ActionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        [ForeignKey("ActionTypeId")]
        public virtual FbActionType ActionType { get; set; }

        /// <summary>
        /// Gets or sets the post click.
        /// </summary>
        /// <value>
        /// The post click.
        /// </value>
        public int PostClick { get; set; }

        /// <summary>
        /// Gets or sets the post view.
        /// </summary>
        /// <value>
        /// The post view.
        /// </value>
        public int PostView { get; set; }

        /// <summary>
        /// Gets or sets the post click value.
        /// </summary>
        /// <value>
        /// The post click value.
        /// </value>
        public decimal PostClickVal { get; set; }

        /// <summary>
        /// Gets or sets the post view value.
        /// </summary>
        /// <value>
        /// The post view value.
        /// </value>
        public decimal PostViewVal { get; set; }

        /// <summary>
        /// Gets or sets the click attribute window.
        /// </summary>
        /// <value>
        /// The click attribute window.
        /// </value>
        public string ClickAttrWindow { get; set; }

        /// <summary>
        /// Gets or sets the view attribute window.
        /// </summary>
        /// <value>
        /// The view attribute window.
        /// </value>
        public string ViewAttrWindow { get; set; }
    }
}
