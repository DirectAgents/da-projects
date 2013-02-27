namespace Huggies.Web
{
    public class Constants
    {
        public static string ModelValidationError_child_must_be_lessthan_4_months_old_or_unborn
        {
            get { return "Child must be lessthan 4 months old or unborn."; }
        }

        public static string ModelValidationError_must_be_first_child
        {
            get { return "Must be first child."; }
        }

        public static string ModelValidationError_gender_cannot_be_unknown_if_child_is_already_born
        {
            get { return "Gender cannot be unknown if child is already born."; }
        }
    }
}