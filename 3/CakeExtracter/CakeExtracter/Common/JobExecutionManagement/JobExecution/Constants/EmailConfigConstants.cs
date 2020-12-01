namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Constants
{
    /// <summary>
    /// Email configuration  constants.
    /// </summary>
    public class EmailConfigConstants
    {
        /// <summary>
        /// The job failed "to" configuration key.
        /// </summary>
        public const string JobFailedToConfigKey = "JEM_JobFailed_ToEmails";

        /// <summary>
        /// The job failed "cc" configuration key.
        /// </summary>
        public const string JobFailedCcConfigKey = "JEM_JobFailed_CcEmails";

        /// <summary>
        /// The job error occurred "to" configuration key.
        /// </summary>
        public const string JobErrorOccurredToConfigKey = "JEM_ErrorOccurred_ToEmails";

        /// <summary>
        /// The job error occurred "cc" configuration key.
        /// </summary>
        public const string JobErrorOccurredCcConfigKey = "JEM_ErrorOccurred_CcEmails";

        /// <summary>
        /// The job processing "to" configuration key.
        /// </summary>
        public const string JobProcessingToConfigKey = "JEM_JobProcessing_ToEmails";

        /// <summary>
        /// The job processing "cc" configuration key.
        /// </summary>
        public const string JobProcessingCcConfigKey = "JEM_JobProcessing_CcEmails";


        /// <summary>
        /// The job failed body email template name.
        /// </summary>
        public const string JobFailedBodyTemplateName = "JobFailedBody";

        /// <summary>
        /// The job failed subject email template name.
        /// </summary>
        public const string JobFailedSubjectTemplateName = "JobFailedSubject";

        /// <summary>
        /// The job error occurred body email template name.
        /// </summary>
        public const string JobErrorOccurredBodyTemplateName = "JobErrorOccuredBody";

        /// <summary>
        /// The job error occurred subject email template name.
        /// </summary>
        public const string JobErrorOccurredSubjectTemplateName = "JobErrorOccuredSubject";

        /// <summary>
        /// The job processing body email template name.
        /// </summary>
        public const string JobProcessingBodyTemplateName = "JobProcessingBody";

        /// <summary>
        /// The job processing subject email template name.
        /// </summary>
        public const string JobProcessingSubjectTemplateName = "JobProcessingSubject";
    }
}
