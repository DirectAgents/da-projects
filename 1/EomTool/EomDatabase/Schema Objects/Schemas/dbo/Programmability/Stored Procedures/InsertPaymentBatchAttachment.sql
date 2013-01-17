CREATE PROCEDURE dbo.InsertPaymentBatchAttachment
	@name nvarchar(255),
	@binary_content varbinary(max),
	@payment_batch_id int,
	@Identity int OUT
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [dbo].[PaymentBatchAttachment] ([name],[binary_content],[payment_batch_id])
    VALUES (@name,@binary_content,@payment_batch_id)
	SET @Identity = SCOPE_IDENTITY()
END
GO
