---------------------------------------------------------------
-- GET Item num units
-- TODO: make the name right (CostRef ==> RevCost)
---------------------------------------------------------------
CREATE PROCEDURE [dbo].[SelectNumUnitsByPidCdCostRev]
	@Pid int,
	@Cd varchar(500),
	@Rev money,
	@Cost money
AS
BEGIN
	SELECT IT.id, IT.pid, AF.name2, IT.revenue_per_unit, IT.cost_per_unit, IT.num_units
	FROM Item IT
	INNER JOIN Affiliate AF ON IT.affid = AF.affid
	WHERE AF.name2 = @Cd
	AND IT.pid = @Pid
	AND IT.revenue_per_unit = @Rev
	AND IT.cost_per_unit = @Cost
END
