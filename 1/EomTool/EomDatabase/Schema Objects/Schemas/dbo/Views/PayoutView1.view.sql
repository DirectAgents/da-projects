--<?xml version="1.0" encoding="utf-8"?>
--<payout xmlns="http://www.digitalriver.com/directtrack/api/payout/v1_0" 
--xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
--xsi:schemaLocation="http://www.digitalriver.com/directtrack/api/payout/v1_0 payout.xsd"
--location="https://da-tracking.com/apifleet/rest/1137/payout/c3954">
--<payoutType>campaign</payoutType>
--<campaignResourceURL location="../campaign/1654"/>
--<affiliate allAffiliates="1"/>
--<impression>0</impression>
--<click>0</click>
--<lead>0</lead>
--<percentSale>0</percentSale>
--<flatSale>0</flatSale>
--<percentSubSale>0</percentSubSale>
--<flatSubSale>0</flatSubSale>
--<currency>USD</currency>
--</payout>

CREATE view [dbo].[PayoutView1] as
select 
	*
from Payout P
