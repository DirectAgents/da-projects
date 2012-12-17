CREATE TABLE [dbo].[PaymentBatchUpdate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[payment_batch_id] [int] NOT NULL,
	[windows_identity] [varchar](255) NULL,
	[from_payment_batch_approval_state_id] [int] NULL,
	[to_payment_batch_approval_state_id] [int] NULL,
	[note] [varchar](max) NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_PaymentBatchUpdate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PaymentBatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_PaymentBatchUpdate_PaymentBatch] FOREIGN KEY([payment_batch_id])
REFERENCES [dbo].[PaymentBatch] ([id])
GO

ALTER TABLE [dbo].[PaymentBatchUpdate] CHECK CONSTRAINT [FK_PaymentBatchUpdate_PaymentBatch]
GO

ALTER TABLE [dbo].[PaymentBatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_Table_1_PaymentBatchApprovalStateFrom] FOREIGN KEY([from_payment_batch_approval_state_id])
REFERENCES [dbo].[PaymentBatchApprovalState] ([id])
GO

ALTER TABLE [dbo].[PaymentBatchUpdate] CHECK CONSTRAINT [FK_Table_1_PaymentBatchApprovalStateFrom]
GO

ALTER TABLE [dbo].[PaymentBatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_Table_1_PaymentBatchApprovalStateTo] FOREIGN KEY([to_payment_batch_approval_state_id])
REFERENCES [dbo].[PaymentBatchApprovalState] ([id])
GO

ALTER TABLE [dbo].[PaymentBatchUpdate] CHECK CONSTRAINT [FK_Table_1_PaymentBatchApprovalStateTo]
GO
