CREATE VIEW [dbo].[FakePublishers]
AS
SELECT DISTINCT name
FROM         dbo.Affiliate
WHERE     (name IN ('DAS', 'DA- Marchex', 'Direct Agents Search Team', 'Extra', 'Direct Agents Creative Servcies'))
