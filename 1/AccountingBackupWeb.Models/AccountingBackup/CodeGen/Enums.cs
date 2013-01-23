namespace AccountingBackupWeb.Models.AccountingBackup
{
	public enum eEntryTypes
	{
		Default = 1,
		Error = 2,
		Error_StartingBalanceLoader_CustomerNotFound = 3,
		Error_StartingBalanceLoader_MultipleCustomers = 4,
		Error_StartingBalanceLoader_SameCustomerMapsToMultipleAdvertisers = 5,
		Error_StartingBalanceLoader_CurrenciesDoNotMatch = 6,
		CurrencyConversion = 7,
		BudgetReportCache = 8
	}
}