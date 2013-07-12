﻿CREATE TABLE [dbo].[Conversion] (
    [conversion_id]                NVARCHAR (128)  NOT NULL,
    [visitor_id]                   INT             NOT NULL,
    [request_session_id]           INT             NOT NULL,
    [click_id]                     INT             NULL,
    [conversion_date]              DATETIME        NOT NULL,
    [last_updated]                 DATETIME        NULL,
    [click_date]                   DATETIME        NULL,
    [affiliate_affiliate_id]       INT             NOT NULL,
    [affiliate_affiliate_name]     NVARCHAR (MAX)  NULL,
    [advertiser_advertiser_id]     INT             NOT NULL,
    [advertiser_advertiser_name]   NVARCHAR (MAX)  NULL,
    [offer_offer_id]               INT             NOT NULL,
    [offer_offer_name]             NVARCHAR (MAX)  NULL,
    [campaign_id]                  INT             NOT NULL,
    [creative_creative_id]         INT             NOT NULL,
    [creative_creative_name]       NVARCHAR (MAX)  NULL,
    [sub_id_1]                     NVARCHAR (MAX)  NULL,
    [sub_id_2]                     NVARCHAR (MAX)  NULL,
    [sub_id_3]                     NVARCHAR (MAX)  NULL,
    [sub_id_4]                     NVARCHAR (MAX)  NULL,
    [sub_id_5]                     NVARCHAR (MAX)  NULL,
    [conversion_type]              NVARCHAR (MAX)  NULL,
    [paid_currency_id]             TINYINT         NOT NULL,
    [paid_amount]                  DECIMAL (18, 2) NOT NULL,
    [paid_formatted_amount]        NVARCHAR (MAX)  NULL,
    [received_currency_id]         TINYINT         NOT NULL,
    [received_amount]              DECIMAL (18, 2) NOT NULL,
    [received_formatted_amount]    NVARCHAR (MAX)  NULL,
    [step_reached]                 TINYINT         NOT NULL,
    [pixel_dropped]                BIT             NOT NULL,
    [suppressed]                   BIT             NOT NULL,
    [returned]                     BIT             NOT NULL,
    [test]                         BIT             NOT NULL,
    [transaction_id]               NVARCHAR (MAX)  NULL,
    [conversion_ip_address]        NVARCHAR (MAX)  NULL,
    [click_ip_address]             NVARCHAR (MAX)  NULL,
    [country]                      NVARCHAR (MAX)  NULL,
    [conversion_referrer_url]      NVARCHAR (MAX)  NULL,
    [click_referrer_url]           NVARCHAR (MAX)  NULL,
    [conversion_user_agent]        NVARCHAR (MAX)  NULL,
    [click_user_agent]             NVARCHAR (MAX)  NULL,
    [disposition_approved]         BIT             NOT NULL,
    [disposition_disposition_name] NVARCHAR (MAX)  NULL,
    [disposition_contact]          NVARCHAR (MAX)  NULL,
    [disposition_disposition_date] DATETIME        NULL,
    [note]                         NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_dbo.Conversion] PRIMARY KEY CLUSTERED ([conversion_id] ASC)
);

