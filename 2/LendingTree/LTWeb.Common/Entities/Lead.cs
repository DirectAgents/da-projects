using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace LTWeb
{
    public class Lead : Entity<long>
    {
        public DateTime Timestamp { get; set; }

        public string RequestContent { get; set; }

        public string ResponseContent { get; set; }

        public DateTime ResponseTimestamp { get; set; }

        [NotMapped]
        public XElement RequestContentXml
        {
            get { return XElement.Parse(RequestContent); }
            set { RequestContent = value.ToString(); }
        }

        [NotMapped]
        public XElement ResponseContentXml
        {
            get { return XElement.Parse(ResponseContent); }
            set { ResponseContent = value.ToString(); }
        }

        public static string Example = @"<LendingTreeAffiliateRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <Request type=""Refinance"" created=""2011-07-22 15:28:30"" updated=""2011-07-22 15:28:30"" VisitorSessionID=""0"" AppID=""26f2953d-af1d-4da3-a108-0892a2c0f37a"" ElectronicDisclosureConsent=""Y"">
    <SourceOfRequest>
      <LendingTreeAffiliatePartnerCode>19655</LendingTreeAffiliatePartnerCode>
      <LendingTreeAffiliateUserName>DirectAgentsUser</LendingTreeAffiliateUserName>
      <LendingTreeAffiliatePassword>capeWrAjujujajA5uspa</LendingTreeAffiliatePassword>
      <LendingTreeAffiliateEsourceID>5860430</LendingTreeAffiliateEsourceID>
      <VisitorIPAddress>::1</VisitorIPAddress>
      <VisitorURL>https://mortgage-rate-offers.com</VisitorURL>
      <LTLOptin>Y</LTLOptin>
    </SourceOfRequest>
    <HomeLoanProduct>
      <Refinance>
        <PropertyEstimatedValue>100000</PropertyEstimatedValue>
        <MonthlyPayment>450</MonthlyPayment>
        <EstimatedMortgageBalance>9000</EstimatedMortgageBalance>
        <HaveMultipleMortages>N</HaveMultipleMortages>
        <RequestedProducts>
          <Product>30YRF</Product>
          <Product>15YRF</Product>
          <Product>5YRARM</Product>
        </RequestedProducts>
        <CashOut>75000</CashOut>
        <SubjectProperty>
          <PropertyType>SINGLEFAMATT</PropertyType>
          <PropertyUse>OWNEROCCUPIED</PropertyUse>
          <PropertyZip>08742</PropertyZip>
        </SubjectProperty>
      </Refinance>
    </HomeLoanProduct>
    <Applicant Primary=""Y"" PrimaryContact=""Y"">
      <FirstName>aaron</FirstName>
      <LastName>aa</LastName>
      <Street>23</Street>
      <State>CA</State>
      <Zip>08742</Zip>
      <DateOfBirth>10/10/1956</DateOfBirth>
      <HomePhone>2232232221</HomePhone>
      <WorkPhone>3342232232</WorkPhone>
      <EmailAddress>aa@b.com</EmailAddress>
      <Password>1234</Password>
      <IsVeteran>N</IsVeteran>
      <CreditHistory>
        <CreditSelfRating>SOMECREDITPROBLEMS</CreditSelfRating>
        <BankruptcyDischarged>NEVER</BankruptcyDischarged>
        <ForeclosureDischarged>NEVER</ForeclosureDischarged>
      </CreditHistory>
    </Applicant>
  </Request>";
    }
}
