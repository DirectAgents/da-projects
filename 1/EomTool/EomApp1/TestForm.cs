using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DirectTrack.Rest;
using System.Xml.Linq;
using DAgents.Common;
using DAgents.Synch;
using System.IO;
using System.Threading.Tasks;

namespace EomApp1
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetAdvertisers();

            //ReadCreatives();
            //ConcatXmlFiles();
        }

        private void GetAdvertisers()
        {
            var ds = new DirectTrackDataSet();
            var da = new DirectTrackDataSetTableAdapters.AdvertiserItemsTableAdapter();
            da.Fill(ds.AdvertiserItems);
            DirectTrackDataSet.AdvertiserItemsDataTable dt = ds.AdvertiserItems;
            foreach (var item in AdvertiserList.PullFromDirectTrack().Items)
            {
                richTextBox1.AppendText(item.Name + "/" + item.Company + "/" + item.Email);
                var row = dt.NewAdvertiserItemsRow();
                row.Name = item.Name;
                row.Company = item.Company;
                row.Email = item.Email;
                row.DirectTrackAdvertiserId = item.DirectTrackAdvertiserId;
                row.Xml = item.GetDetail();
                dt.Rows.Add(row);
            }
            da.Update(ds);
        }

        string dirName = @"\\ad1\aaron\SynchCreatives";
        string fileFormat = @"creatives_for_pid_{0}_{1}.xml";

        private void ReadCreatives()
        {
            Parallel.ForEach(CampaignList.PullActiveFromDirectTrack().CampaignItems.OrderByDescending(c => c.CampaignId), (campaign) =>
            {
                var results = new List<XDocument>();
                XmlGetter.GetResrouces2("https://da-tracking.com/apifleet/rest/1137/1/creative/campaign/" + campaign.CampaignId + "/", results, new ConsoleLogger());
                foreach (var result in results)
                {
                    var fileName = string.Format(fileFormat, campaign.CampaignId, campaign.CampaignName);
                    var legalFileName = Utilities.MakeLegalFilename(fileName);
                    var fullPath = dirName + @"\" + legalFileName;
                    File.WriteAllText(fullPath, result.ToString());
                }
            });
        }

        private void ConcatXmlFiles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<creatives>");
            foreach (var file in new DirectoryInfo(dirName).GetFiles("*.xml"))
            {
                sb.Append(File.ReadAllText(file.FullName));
            }
            sb.Append("</creatives");
            File.WriteAllText(dirName + @"\" + "creatives.xml", sb.ToString());
        }
    }
}
