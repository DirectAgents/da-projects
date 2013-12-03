namespace Huggies.Web
{
    public class Constants
    {
        public const string ModelValidationError_child_must_be_lessthan_4_months_old_or_unborn =
            "Child must be less than 4 months old or unborn.";

        public const string ModelValidationError_must_be_first_child =
            "Must be first child.";

        public const string ModelValidationError_gender_cannot_be_unknown_if_child_is_already_born =
            "Gender cannot be unknown if child is already born.";

        public const string ModelValidationError_duedate_must_be_specified =
            "A valid due date must be specified.";

        public const string ModelValidationError_duedate_too_far_out =
            "Due date must be within 11 months.";
    }
}