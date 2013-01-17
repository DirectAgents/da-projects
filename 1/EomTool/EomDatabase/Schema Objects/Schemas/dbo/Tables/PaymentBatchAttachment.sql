CREATE TABLE [dbo].[PaymentBatchAttachment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[description] [nvarchar](255) NULL,
	[binary_content] [varbinary](max) NOT NULL,
	[payment_batch_id] [int] NULL,
 CONSTRAINT [PK_PaymentBatchAttachment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[PaymentBatchAttachment]  WITH CHECK ADD  CONSTRAINT [FK_PaymentBatchAttachment_PaymentBatch] FOREIGN KEY([payment_batch_id])
REFERENCES [dbo].[PaymentBatch] ([id])
GO

ALTER TABLE [dbo].[PaymentBatchAttachment] CHECK CONSTRAINT [FK_PaymentBatchAttachment_PaymentBatch]
GO
