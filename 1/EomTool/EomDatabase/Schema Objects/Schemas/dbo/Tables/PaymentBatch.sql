CREATE TABLE [dbo].[PaymentBatch](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[approver_identity] [varchar](255) NULL,
	[payment_batch_state_id] [int] NOT NULL,
	[is_current] [bit] NOT NULL,
	[payment_threshold] [money] NULL,
	[parent_batch_id] [int] NULL,
	[payment_method_id] [int] NULL,
	[name] [varchar](255) NULL,
	[date_sent] [datetime] NULL,
 CONSTRAINT [PK_PaymentBatch] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentBatch]  WITH CHECK ADD  CONSTRAINT [FK_PaymentBatch_AffiliatePaymentMethod] FOREIGN KEY([payment_method_id])
REFERENCES [dbo].[AffiliatePaymentMethod] ([id])
GO

ALTER TABLE [dbo].[PaymentBatch] CHECK CONSTRAINT [FK_PaymentBatch_AffiliatePaymentMethod]
GO

ALTER TABLE [dbo].[PaymentBatch]  WITH CHECK ADD  CONSTRAINT [FK_PaymentBatch_PaymentBatch] FOREIGN KEY([parent_batch_id])
REFERENCES [dbo].[PaymentBatch] ([id])
GO

ALTER TABLE [dbo].[PaymentBatch] CHECK CONSTRAINT [FK_PaymentBatch_PaymentBatch]
GO

ALTER TABLE [dbo].[PaymentBatch]  WITH CHECK ADD  CONSTRAINT [FK_PaymentBatch_PaymentBatchState] FOREIGN KEY([payment_batch_state_id])
REFERENCES [dbo].[PaymentBatchState] ([id])
GO

ALTER TABLE [dbo].[PaymentBatch] CHECK CONSTRAINT [FK_PaymentBatch_PaymentBatchState]
GO

ALTER TABLE [dbo].[PaymentBatch] ADD  CONSTRAINT [DF_PaymentBatch_payment_batch_state_id]  DEFAULT ((1)) FOR [payment_batch_state_id]
GO
