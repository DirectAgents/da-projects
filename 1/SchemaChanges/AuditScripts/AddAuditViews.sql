CREATE VIEW [dbo].[CampaignFinalizationAudit]
AS
SELECT     TOP (100) PERCENT dbo.Advertiser.name AS Advertiser, dbo.Campaign.campaign_name AS Campaign, dbo.CampaignStatus.name AS [From], 
                      CampaignStatus_1.name AS [To], dbo.Audit.SysUser AS [User], CONVERT(varchar, dbo.Audit.AuditDate, 101) AS Date, CONVERT(varchar, dbo.Audit.AuditDate, 108) 
                      AS Time
FROM         dbo.Item INNER JOIN
                      dbo.Audit ON dbo.Item.id = dbo.Audit.PrimaryKey AND dbo.Audit.TableName = 'dbo.Item' AND dbo.Audit.ColumnName = '[campaign_status_id]' INNER JOIN
                      dbo.CampaignStatus ON dbo.Audit.OldValue = dbo.CampaignStatus.id INNER JOIN
                      dbo.CampaignStatus AS CampaignStatus_1 ON dbo.Audit.NewValue = CampaignStatus_1.id INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id
WHERE     (dbo.Audit.Operation = 'u')
GROUP BY dbo.CampaignStatus.name, CampaignStatus_1.name, CONVERT(varchar, dbo.Audit.AuditDate, 101), dbo.Campaign.campaign_name, dbo.Audit.SysUser, 
                      CONVERT(varchar, dbo.Audit.AuditDate, 108), dbo.Advertiser.name
ORDER BY Date DESC, Time DESC
GO

CREATE VIEW [dbo].[EOMactivity]
AS
select distinct a.primarykey,a.sysuser,convert(datetime, convert(char(19), a.auditdate, 126)) as date,a.operation,a.columnname,a.oldvalue,a.newvalue,
adv.name as advertiser,c.pid,c.campaign_name,aff.name2 as publisher
, db_name() AS DatabaseName
from audit a
join item i on i.id=a.primarykey
join campaign c on i.pid=c.pid
join advertiser adv on c.advertiser_id=adv.id
join affiliate aff on i.affid=aff.affid
where operation='u'
and columnname<>'[item_accounting_status_id]'
and columnname<>'[media_buyer_approval_status_id]'
and columnname<>'[batch_id]'
and (columnname<>'[campaign_status_id]' or newvalue<>4) -- don't include 'verify campaign'
and (columnname<>'[campaign_status_id]' or i.total_revenue<>0 or i.total_cost<>0)
--don't want campaign_status updates for items with no cost or revenue (currently)

GO

