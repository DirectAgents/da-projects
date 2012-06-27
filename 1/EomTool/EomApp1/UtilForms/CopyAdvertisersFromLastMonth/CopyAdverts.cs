using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EomApp1.UtilForms.CopyAdvertisersFromLastMonth
{
    public partial class CopyAdverts : Form
    {
        public CopyAdverts()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            NovDataContext novdb = new NovDataContext();
            DecDataContext decdb = new DecDataContext();
            foreach (var item in 
                from c in novdb.Advertisers 
                          orderby  c.id
                          select c)
            {
                if (item.id == 1) continue;

                if ((from c in decdb.Advertisers
                     where c.name == item.advertiser_name
                     select c).FirstOrDefault() != null) continue;
                
                Dec.Advertiser adv = new Dec.Advertiser();
                adv.name = item.advertiser_name;
                decdb.Advertisers.InsertOnSubmit(adv);
                decdb.SubmitChanges();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NovDataContext novdb = new NovDataContext();
            DecDataContext decdb = new DecDataContext();
            //Dictionary<string,int> advertiserName2ID = decdb.Advertisers.ToDictionary(c=>c.name,c=>c.id);
            
            // look over november campaigns
            foreach (var item in from c in novdb.Campaigns select c)
            {
                // get the matching dec campaign
                var cdec = from c in decdb.Campaigns
                           where c.pid == item.pid
                           select c;

                // make sure it exists
                if (cdec.FirstOrDefault() != null)
                {
                    try
                    {


                        // set the ad manager id to the one in the
                        // dec database that has the same name

                        // first get the name from the nov database
                        var novAdvertName = from c in novdb.Advertisers
                                            where c.id == item.advertiser_id
                                            select c;

                        // now get that from the dec database
                        var decAdvertID = from c in decdb.Advertisers
                                          where c.name == novAdvertName.First().advertiser_name
                                          select c;

                        cdec.First().advertiser_id = decAdvertID.First().id;
                    }
                    catch
                    {

                    }
                }

                // yes, update the campaign.advertiser_id field
            }
            decdb.SubmitChanges();
        }
    }
}
