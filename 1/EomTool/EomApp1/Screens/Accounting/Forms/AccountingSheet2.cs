using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EomAppControls;
using System.Data.SqlClient;
using EomApp1.Screens.Accounting.Data;

namespace EomApp1.Screens.Accounting.Forms
{
    public partial class AccountingSheet2 : AccountingSheet2Base
    {
        public AccountingSheet2()
        {
            InitializeComponent();

            if (!DesignMode)
                InitControls();
        }

        private void InitControls()
        {
            // Create data table
            this.DataTable = new DataTable()
            {
                TableName = this.TableName
            };

            // Create data set and add data table to it
            this.DataSet = new DataSet();
            this.DataSet.Tables.Add(this.DataTable);

            // Create binding source
            this.BindingSource = new BindingSource(this.DataSet, this.TableName);

            // Create grid view for items
            this.GridView = new ExtendedDataGridView()
            {
                AutoGenerateColumns = true,
                DataSource = this.BindingSource,
                Dock = DockStyle.Fill,
                Visible = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToOrderColumns = true,
                RowHeadersVisible = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                ColumnFiltersEnabled = true,
                ColumnSelectorEnabled = true,
            };
            this.GridView.ColumnAdded += new DataGridViewColumnEventHandler(GridView_ColumnAdded);
            this.GridView.CellValueChanged += new DataGridViewCellEventHandler(GridView_CellValueChanged);

            // Create tool strip for filter
            this.ToolStrip1 = new ToolStrip
            {
                Dock = DockStyle.Bottom,
                GripStyle = ToolStripGripStyle.Hidden
            };

            // Create check box for filter
            this.FilterZeroCostAndRevCheckBox = new CheckBox
            {
                Text = "Filter zero cost and revenue",
                Checked = true
            };
            this.FilterZeroCostAndRevCheckBox.CheckedChanged += new EventHandler(filterZeroCostAndRevCheckBox_CheckedChanged);

            // Create tool strip control host for filter check box
            this.FilterZeroCostAndRevToolStripItem = new ToolStripControlHost(FilterZeroCostAndRevCheckBox)
            {
                Padding = new Padding(10, 0, 0, 0)
            };

            // Populate tool strip for filter
            this.ToolStrip1.Items.AddRange(new ToolStripItem[] {
                this.FilterZeroCostAndRevToolStripItem
            });

            // Create tool strip for button
            this.ToolStrip2 = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden
            };

            // Create save button
            this.SaveButton = new ToolStripControlHost(new Button
            {
                Text = "Save Changes",
                BackColor = Color.Green,
                ForeColor = Color.WhiteSmoke,
                Enabled = false,
            })
            {
                Padding = new Padding(10, 0, 0, 0)
            };
            this.SaveButton.Click += new EventHandler(saveButton_Click);

            // Populate the tool strip with buttons
            this.ToolStrip2.Items.Add(SaveButton);
            this.splitContainer.Panel1.Controls.AddRange(new Control[] {
                this.GridView,
                this.ToolStrip1
            });

            // Add the split container to the form
            this.Controls.Add(this.splitContainer);

            // Create data adapter
            this.DataAdapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(string.Format("SELECT * FROM {0}", this.TableName), new SqlConnection(this.ConnectionString))
            };

            // Fill the data
            Fill();

            // Create grid view for pending changes
            this.GridView2 = new DataGridView()
            {
                AutoGenerateColumns = true,
                Dock = DockStyle.Fill,
                Visible = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToOrderColumns = true,
                EditMode = DataGridViewEditMode.EditProgrammatically,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
            };
            this.GridView2.ColumnAdded += new DataGridViewColumnEventHandler(GridView2_ColumnAdded);

            // Add the pending changes controls to the split container panel
            this.splitContainer.Panel2.Controls.AddRange(new Control[] {
                this.GridView2,
                this.ToolStrip2
            });
        }

        private void SaveChanges()
        {
            var sqlStatements = new List<string>();
            string sqlFormat = "update item set {0}={1} where id in ({2})";
            foreach (var amountChange in this.ItemChanges.AmountChanges)
            {
                if (amountChange.Field == "Cost/Unit")
                {
                    string sql = string.Format(sqlFormat, "cost_per_unit", amountChange.ToAmount, amountChange.Ids);
                    sqlStatements.Add(sql);
                }
                else if (amountChange.Field == "Rev/Unit")
                {
                    string sql = string.Format(sqlFormat, "revenue_per_unit", amountChange.ToAmount, amountChange.Ids);
                    sqlStatements.Add(sql);
                }
                else
                {
                    throw new Exception("cannot edit field " + amountChange.Field);
                }
            }
            foreach (var sql in sqlStatements)
            {
                ExecuteSql(sql);
            }
        }

        private void ExecuteSql(string sql)
        {
            using (var con = new SqlConnection(EomAppCommon.EomAppSettings.ConnStr))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void Fill()
        {
            this.DataTable.Clear();
            this.DataAdapter.FillSchema(this.DataTable, SchemaType.Source);
            this.DataAdapter.Fill(this.DataTable);

            if (this.FilterZeroCostAndRevCheckBox.Checked)
                ApplyFilterAllZerosFilter();
        }

        private void FillChanges()
        {
            this.BindingSource.EndEdit();

            if (this.splitContainer.Panel2Collapsed)
                this.splitContainer.Panel2Collapsed = false;

            var revPerUnitChanges = from row in this.DataTable.GetChanges(DataRowState.Modified).AsEnumerable()
                                    let field = "Rev/Unit"
                                    select new
                                    {
                                        Field = field,
                                        From = row.Field<decimal>(field, DataRowVersion.Original),
                                        To = row.Field<decimal>(field, DataRowVersion.Current),
                                        ItemIds = row.Field<string>("ItemIds"),
                                    };

            var costPerUnitChanges = from row in this.DataTable.GetChanges(DataRowState.Modified).AsEnumerable()
                                     let field = "Cost/Unit"
                                     select new
                                     {
                                         Field = field,
                                         From = row.Field<decimal>(field, DataRowVersion.Original),
                                         To = row.Field<decimal>(field, DataRowVersion.Current),
                                         ItemIds = row.Field<string>("ItemIds"),
                                     };


            var changes = revPerUnitChanges.Concat(costPerUnitChanges).Where(c => c.From != c.To);

            ItemChanges = new ItemsDataSet();

            foreach (var change in changes.ToList())
            {
                ItemChanges.AmountChanges.AddAmountChangesRow(change.Field, change.From, change.To, change.ItemIds);
            }

            this.GridView2.DataSource = ItemChanges.AmountChanges;

            this.SaveButton.Enabled = ItemChanges.AmountChanges.Count > 0;

            this.GridView2.ClearSelection();
        }

        private void ApplyFilterAllZerosFilter()
        {
            this.BindingSource.Filter = "[Cost/Unit] <> 0.0 OR [Rev/Unit] <> 0.0";
        }

        #region Event Handlers

        void GridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Name == "Cost/Unit")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                e.Column.CellTemplate = BoldNumberCellTemplateWithRedBackColor;
            }
            else if (e.Column.Name == "Rev/Unit")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                e.Column.CellTemplate = BoldNumberCellTemplateWithGreenBackColor;
            }
            else if (e.Column.Name == "Cost")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                e.Column.CellTemplate = NumberCellTemplate;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.Name == "Rev")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                e.Column.CellTemplate = NumberCellTemplate;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.Name == "ItemIds")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                e.Column.Visible = false;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.Name == "Campaign")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                e.Column.FillWeight = 50;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.Name == "Publisher")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                e.Column.FillWeight = 25;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.Name == "Advertiser")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                e.Column.FillWeight = 25;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.Name == "Units")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                e.Column.Visible = true;
                e.Column.CellTemplate = WholeNumberCellTemplate;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.Name == "Unit Type")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                e.Column.Visible = true;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.Name == "%Margin")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                e.Column.CellTemplate = NumberCellTemplate;
                e.Column.Visible = true;
                e.Column.ReadOnly = true;
            }
        }

        void GridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            string revPerUnitFieldName = "Rev/Unit";
            string costPerUnitFieldName = "Cost/Unit";

            if (e.ColumnIndex == this.GridView.Columns[revPerUnitFieldName].Index)
            {
                var cell = this.GridView[e.ColumnIndex, e.RowIndex];
                decimal current = (decimal)cell.Value;

                var row = cell.OwningRow;
                var dataRow = (row.DataBoundItem as DataRowView).Row;
                decimal original = dataRow.Field<decimal>(revPerUnitFieldName, DataRowVersion.Original);

                if (original != current)
                {
                    cell.Style.BackColor = Color.Red;
                }
                FillChanges();
            }
            else if (e.ColumnIndex == this.GridView.Columns[costPerUnitFieldName].Index)
            {
                var cell = this.GridView[e.ColumnIndex, e.RowIndex];
                decimal current = (decimal)cell.Value;

                var row = cell.OwningRow;
                var dataRow = (row.DataBoundItem as DataRowView).Row;
                decimal original = dataRow.Field<decimal>(costPerUnitFieldName, DataRowVersion.Original);

                if (original != current)
                {
                    cell.Style.BackColor = Color.Red;
                }
                FillChanges();
            }
        }

        void filterZeroCostAndRevCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (this.GridView.Rows.Count > 0)
            {
                this.BindingSource.RemoveFilter();
                if (checkBox.Checked)
                    ApplyFilterAllZerosFilter();
            }
        }

        void saveButton_Click(object sender, EventArgs e)
        {
            this.BindingSource.EndEdit();
            FillChanges();
            SaveChanges();
            Fill();
            this.splitContainer.Panel2Collapsed = true;
        }

        void GridView2_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Name == "Ids")
            {
                e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                e.Column.FillWeight = 100;
            }
        }

        #endregion

        #region Cell Styles
        
        private static DataGridViewTextBoxCell BoldNumberCellTemplate
        {
            get
            {
                var font = new Font(DataGridView.DefaultFont, FontStyle.Bold);

                var style = new DataGridViewCellStyle
                {
                    Font = font,
                    Format = "N2"
                };

                var template = new DataGridViewTextBoxCell
                {
                    Style = style
                };
                return template;
            }
        }

        private static DataGridViewTextBoxCell BoldNumberCellTemplateWithGreenBackColor
        {
            get
            {
                var font = new Font(DataGridView.DefaultFont, FontStyle.Bold);

                var style = new DataGridViewCellStyle
                {
                    Font = font,
                    Format = "N2",
                    BackColor = Color.LightGreen
                };

                var template = new DataGridViewTextBoxCell
                {
                    Style = style
                };
                return template;
            }
        }

        private static DataGridViewTextBoxCell BoldNumberCellTemplateWithRedBackColor
        {
            get
            {
                var font = new Font(DataGridView.DefaultFont, FontStyle.Bold);

                var style = new DataGridViewCellStyle
                {
                    Font = font,
                    Format = "N2",
                    BackColor = Color.LightPink
                };

                var template = new DataGridViewTextBoxCell
                {
                    Style = style
                };
                return template;
            }
        }

        private static DataGridViewTextBoxCell NumberCellTemplate
        {
            get
            {
                var font = new Font(DataGridView.DefaultFont, FontStyle.Regular);

                var style = new DataGridViewCellStyle
                {
                    Font = font,
                    Format = "N2"
                };

                var template = new DataGridViewTextBoxCell
                {
                    Style = style
                };
                return template;
            }
        }

        private static DataGridViewTextBoxCell WholeNumberCellTemplate
        {
            get
            {
                var font = new Font(DataGridView.DefaultFont, FontStyle.Regular);

                var style = new DataGridViewCellStyle
                {
                    Font = font,
                    Format = "N0"
                };

                var template = new DataGridViewTextBoxCell
                {
                    Style = style
                };
                return template;
            }
        } 

        #endregion
    }
}
