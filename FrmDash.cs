using Dapper;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Bake_Shop
{

    public partial class FrmDash : MaterialForm
    {
        int isAuthenticated = 0;
        int lastSelectedIndex = 0;
        SqlConnection obj = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
        public FrmDash()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500,
                Accent.DeepOrange700, TextShade.WHITE);

            if (isAuthenticated == 0)
            {
                //disable tabs except login
                for (int i = 0; i < mTbController.TabCount; i++)
                {
                    if (i != 9) // skip tbLogOut tab (index 9)
                    {
                        mTbController.TabPages[i].Enabled = false;

                    }
                }
            }
        }


        MaterialSkinManager ThemeManager = MaterialSkinManager.Instance;

        public object NumberCompareValidator { get; private set; }
        public object TextBoxRequiredValidator { get; private set; }

        //tbSettings
        //Select Theme
        private void SwitchColor_CheckedChanged_1(object sender, EventArgs e)
        {
            if (SwitchColor.Checked)
            {
                ThemeManager.Theme = MaterialSkinManager.Themes.LIGHT;
            }
            else
            {
                ThemeManager.Theme = MaterialSkinManager.Themes.DARK;
            }
        }
        //Select theme color
        private void rbGreen_CheckedChanged(object sender, EventArgs e)
        {
            ThemeManager.ColorScheme = new ColorScheme(Primary.Green700, Primary.Green900, Primary.Green500, Accent.Green700, TextShade.WHITE);
        }

        private void rbBlue_CheckedChanged(object sender, EventArgs e)
        {
            ThemeManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue900, Primary.Blue500, Accent.Blue700, TextShade.WHITE);
        }

        private void rbDefault_CheckedChanged(object sender, EventArgs e)
        {
            ThemeManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500,
                Accent.DeepOrange700, TextShade.WHITE);
        }
        //tbProduct
        //Add product details
        private void mBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "insert into ProductInfo (P_Code,P_Name,P_Desc,Category,Unit_Price,Qty) values ('" + mtxtCode.Text + "','" + mtxtName.Text + "','" + mtxtDes.Text + "','" + mCmbBoxC.Text + "','" + mtxtPrice.Text + "','" + mtxtQty.Text + "')";
                obj.Open();

                SqlCommand cmd = new SqlCommand(sql, obj);
                cmd.ExecuteNonQuery();
                MaterialMessageBox.Show("Successfully input product details !!");


            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message);
            }
            finally
            {
                obj.Close();
            }
        }

        private void mBtnNew_Click(object sender, EventArgs e)
        {
            mtxtCode.Text = " ";
            mtxtName.Text = " ";
            mtxtDes.Text = " ";
            mCmbBoxC.Text = " ";
            mtxtPrice.Text = " ";
            mtxtQty.Text = " ";
        }

        private void mBtnExit_Click(object sender, EventArgs e)
        {
            tbHome.Hide();
            tbHome.Show();
        }

        private void mtxtCode_Click(object sender, EventArgs e)
        {
            //error provider
            //if (string.IsNullOrEmpty(mtxtCode.Text))
            //{
            //    e. = true;
            //    mtxtCode.Focus();
            //    errorProviderCode.SetError(mtxtCode, "Please enter only number!!");

            //}
            //else
            //{

            //}
        }

        private void mtxtName_Click(object sender, EventArgs e)
        {
            //error provider
        }

        private void mtxtQty_Click(object sender, EventArgs e)
        {
            //error provider
        }

        //tbListofProduct
        //List view
        private void mBtnSearch_Click(object sender, EventArgs e)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {

                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                   /* var data = db.Query<ReceiveList>(
                            "SELECT TransNo, Pcode, Pname, Pprice, R_qty, Descri, Tot_Price, Transaction_Date " +
                            "FROM StockIn " +
                            "WHERE Pname LIKE @SearchTerm " +
                            "ORDER BY Transaction_Date DESC",
                            new { SearchTerm = $"%{mtxtReceiveSearch.Text}%" },
                            commandType: CommandType.Text);*/
                    var data = db.Query<Product>("select P_Code,P_Name,Category,Unit_Price,Qty from ProductInfo",
                        "WHERE P_Name LIKE "+ $"%{mtxtSearch.Text}%" +
                            " ORDER BY Transaction_Date DESC", 
                            commandType: CommandType.Text);
                    mlistViewPro.Items.Clear();
                    // List<Product> list = DataAccess.GetAll();
                    foreach (Product pro in data)
                    {
                        ListViewItem item = new ListViewItem(pro.P_Code.ToString());
                        item.SubItems.Add(pro.P_Name);
                        item.SubItems.Add(pro.Category);
                        item.SubItems.Add(pro.Unit_Price.ToString());
                        item.SubItems.Add(pro.Qty.ToString());

                        mlistViewPro.Items.Add(item);
                    }
                }
            }
        }

        private void mBtnRefresh_Click(object sender, EventArgs e)
        {
            mlistViewPro.Items.Clear();
            mtxtSearch.Text = " ";

        }

        private void mBtnExitL_Click(object sender, EventArgs e)
        {
            tbHome.Hide();
            tbHome.Show();
        }

        private void mBtnEdit_Click(object sender, EventArgs e)
        {
            tbProduct.Hide();
            tbProduct.Show();
        }

        //tbStockIn
        //Add receive product
        private void mtxtpc_Click(object sender, EventArgs e)
        {
            double val = 0;
            obj.Open();
            SqlCommand cm = new SqlCommand("Select count(TransNo) From StockIn", obj);
            int i = Convert.ToInt32(cm.ExecuteScalar());
            i++;

            mLbltrans.Text = val + i.ToString();
            obj.Close();


            //validation
        }

        private void mtxtup_Click(object sender, EventArgs e)
        {
            //validation
        }

        private void mtxtqtyIn_Click(object sender, EventArgs e)
        {
            //validation
        }

        //calculate total price
        private void mtxtTot_Click(object sender, EventArgs e)
        {
            int price = Convert.ToInt32(mtxtup.Text);
            int qty = Convert.ToInt32(mtxtqtyIn.Text);
            int tot = price * qty;
            mtxtTot.Text = tot.ToString();
        }

        private void mBtnInSave_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "insert into StockIn (TransNo,Transaction_Date,Pcode,Pname,Descri,Pprice,R_qty,Tot_Price) values ('" + mLbltrans.Text + "','" + dTPickIn.Text + "','" + mtxtpc.Text + "','" + mtxtPn.Text + "','" + mtxtdescri.Text + "','" + mtxtup.Text + "', '" + mtxtqtyIn.Text + "','" + mtxtTot.Text + "')";
                obj.Open();

                SqlCommand cmd = new SqlCommand(sql, obj);
                cmd.ExecuteNonQuery();
                MaterialMessageBox.Show("Successfully input  receive product information !!");

                // Update ProductInfo table with new quantity
                string updateSql = "UPDATE ProductInfo SET qty = qty + @newQty WHERE P_Code = @productCode";
                SqlCommand updateCmd = new SqlCommand(updateSql, obj);
                updateCmd.Parameters.AddWithValue("@newQty", Convert.ToInt32(mtxtqtyIn.Text));
                updateCmd.Parameters.AddWithValue("@productCode", mtxtpc.Text);
                updateCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message);
            }
            finally
            {
                obj.Close();
            }

        }

        private void mBtnInNew_Click(object sender, EventArgs e)
        {
            mLbltrans.Text = string.Empty;
            mtxtpc.Text = string.Empty;
            mtxtPn.Text = string.Empty;
            mtxtdescri.Text = string.Empty;
            mtxtup.Text = string.Empty;
            mtxtqtyIn.Text = string.Empty;
            mtxtTot.Text = string.Empty;
        }

        private void mBtnInExit_Click(object sender, EventArgs e)
        {
            tbHome.Hide();
            tbHome.Show();

        }

        //tbreceiveProduct
        //List of receive product
        private void mBtnReceNew_Click(object sender, EventArgs e)
        {
            tbStockin.Hide();
            tbStockin.Show();
        }

        private void mBtnReceiveSearch_Click(object sender, EventArgs e)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                try
                {

                    if (db.State == ConnectionState.Closed)
                    {
                        db.Open();
                        var data = db.Query<ReceiveList>(
                            "SELECT TransNo, Pcode, Pname, Pprice, R_qty, Descri, Tot_Price, Transaction_Date " +
                            "FROM StockIn " +
                            "WHERE Pname LIKE @SearchTerm " +
                            "ORDER BY Transaction_Date DESC",
                            new { SearchTerm = $"%{mtxtReceiveSearch.Text}%" },
                            commandType: CommandType.Text);
                        mviewReceive.Items.Clear();
                        foreach (ReceiveList list in data)
                        {
                            ListViewItem item = new ListViewItem(list.Pcode.ToString());
                            //list.Pcode.ToString()
                            //item.SubItems.Add(list.Pcode.ToString());
                            item.SubItems.Add(list.Pname);
                            item.SubItems.Add(list.Pprice.ToString());
                            item.SubItems.Add(list.R_qty.ToString());
                            item.SubItems.Add(list.TransNo.ToString());
                            item.SubItems.Add(list.Descri);
                            item.SubItems.Add(list.Tot_Price.ToString());
                            item.SubItems.Add(list.Transaction_Date.ToString());

                            mviewReceive.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show(ex.Message);
                }
            }
        }

        private void mBtnClear_Click(object sender, EventArgs e)
        {
            mtxtReceiveSearch.Text = string.Empty;
            mviewReceive.Items.Clear();
        }

        private void mBtnReceExit_Click(object sender, EventArgs e)
        {
            tbHome.Hide();
            tbHome.Show();
        }

        //tbStockIn exit
        private void mBtnExitStock_Click(object sender, EventArgs e)
        {
            tbHome.Hide();
            tbHome.Show();
        }

        //tbStockOut
        //Sold
        private void mtxtPcodeOut_Click(object sender, EventArgs e)
        {
            double val = 0;
            obj.Open();
            SqlCommand cm = new SqlCommand("Select count(Trans_Num) From StockOut", obj);
            int i = Convert.ToInt32(cm.ExecuteScalar());
            i++;

            mlblTranOut.Text = val + i.ToString();
            obj.Close();

            //Validation
        }

        private void mtxtPnameOut_Click(object sender, EventArgs e)
        {
            //validation
        }

        private void mtxtUpriceOut_Click(object sender, EventArgs e)
        {
            //validation
        }

        private void mtxtTotOut_Click(object sender, EventArgs e)
        {

            try
            {
                int Uprice = Convert.ToInt32(mtxtUpriceOut.Text);
                int Qty = Convert.ToInt32(mtxtQtyOut.Text);

                int Profit = Uprice * Qty;
                mtxtTotOut.Text = Profit.ToString();
            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message);
            }
        }

        private void mbtnSaveOut_Click(object sender, EventArgs e)
        {
            try
            {

                //string sql = "insert into StockOut (Trans_Num,Pro_Code,Pro_Name,Pro_Category,Pro_Price,Trans_date,ORemaining_qty,OTot_Price,Pro_Qty) values " + "('" + mlblTranOut.Text + "','" + mtxtPcodeOut .Text + "','" + mtxtPnameOut .Text + "','" + mcmbOut .Text + "','" + mtxtUpriceOut .Text + "'," + "'" + dtPickOut .Text + "', '" + mtxtRemainingOut .Text + "','" + mtxtTotOut.Text + "')";
                obj.Open();
                var command = new SqlCommand("INSERT INTO StockOut (Trans_Num, Pro_Code, Pro_Name, Pro_Category, Pro_Price, Trans_date, OTot_Price, Pro_Qty) " +
                                  "VALUES (@Trans_Num, @Pro_Code, @Pro_Name, @Pro_Category, @Pro_Price, @Trans_date,  @OTot_Price, @Pro_Qty)", obj);
                command.Parameters.AddWithValue("@Trans_Num", mlblTranOut.Text);
                command.Parameters.AddWithValue("@Pro_Code", mtxtPcodeOut.Text);
                command.Parameters.AddWithValue("@Pro_Name", mtxtPnameOut.Text);
                command.Parameters.AddWithValue("@Pro_Category", mcmbOut.Text);
                command.Parameters.AddWithValue("@Pro_Price", mtxtUpriceOut.Text);
                command.Parameters.AddWithValue("@Trans_date", dtPickOut.Text);
                // command.Parameters.AddWithValue("@ORemaining_qty", mtxtRemainingOut.Text);
                command.Parameters.AddWithValue("@OTot_Price", mtxtTotOut.Text);
                command.Parameters.AddWithValue("@Pro_Qty", mtxtQtyOut.Text);

                command.ExecuteNonQuery();
                //SqlCommand cmd = new SqlCommand(sql, obj);
                //cmd.ExecuteNonQuery();
                MaterialMessageBox.Show("Successfully input  sold product information !!");

                // Update ProductInfo table with new quantity
                string updateSql = "UPDATE ProductInfo SET qty = qty - @newQty WHERE P_Code = @productCode";
                SqlCommand updateCmd = new SqlCommand(updateSql, obj);
                updateCmd.Parameters.AddWithValue("@newQty", Convert.ToInt32(mtxtQtyOut.Text));
                updateCmd.Parameters.AddWithValue("@productCode", mtxtPcodeOut.Text);
                updateCmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                MaterialMessageBox.Show(ex.Message);
            }
            finally
            {
                obj.Close();
            }

        }

        private void mbtnResetOut_Click(object sender, EventArgs e)
        {
            mlblTranOut.Text = string.Empty;
            mtxtPcodeOut.Text = string.Empty;
            mtxtPnameOut.Text = string.Empty;
            mcmbOut.Text = string.Empty;
            mtxtQtyOut.Text = string.Empty;

            mtxtUpriceOut.Text = string.Empty;
            mtxtTotOut.Text = string.Empty;
        }

        private void mbtnExitOut_Click(object sender, EventArgs e)
        {
            tbHome.Hide();
            tbHome.Show();
        }
        //tbSoldProduct
        //List of sold product
        private void mbtnSearchSold_Click(object sender, EventArgs e)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
            {
                try
                {

                    if (db.State == ConnectionState.Closed)
                    {
                        db.Open();
                        var data = db.Query<SoldList>("select Trans_Num, Pro_Code, Pro_Name,Pro_Price, Pro_Category,Trans_Date,Pro_Qty from StockOut WHERE Pro_Name LIKE @SearchTerm " +
                            "ORDER BY Trans_Date DESC",
                            new
                            {
                                SearchTerm = $"%{mtxtSearchSold.Text}%"
                            }, commandType: CommandType.Text);

                        mViewSold.Items.Clear();
                        // List<Product> list = DataAccess.GetAll();
                        foreach (SoldList sold in data)
                        {
                            ListViewItem item = new ListViewItem(sold.Trans_Num.ToString());
                            item.SubItems.Add(sold.Pro_Code.ToString());
                            item.SubItems.Add(sold.Pro_Name);
                            item.SubItems.Add(sold.Pro_Category);
                            item.SubItems.Add(sold.Trans_Date.ToString());
                            item.SubItems.Add(sold.Pro_Qty.ToString());
                            item.SubItems.Add(sold.Pro_Price.ToString());

                            mViewSold.Items.Add(item);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MaterialMessageBox.Show(ex.Message);
                }

            }
        }

        private void mbtnResetSold_Click(object sender, EventArgs e)
        {
            mtxtSearchSold.Text = String.Empty;
            mViewSold.Items.Clear();
        }

        private void mbtnEditSold_Click(object sender, EventArgs e)
        {
            tbStockOut.Hide();
            tbStockOut.Show();
        }

        private void mbtnExitSold_Click(object sender, EventArgs e)
        {
            tbHome.Hide();
            tbHome.Show();
        }

        private void mbtnEdilRecei_Click(object sender, EventArgs e)
        {
            tbStockin.Hide();
            tbStockin.Show();
        }

        private void tbLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mbtnLogIn_Click(object sender, EventArgs e)
        {
            string username = mtxtUserName.Text;
            string password = mtxtPassword.Text;
            if (username == "admin" && password == "admin")
            {
                //login
                if (isAuthenticated == 0)
                {
                    isAuthenticated = 1;
                    //disable tabs except login
                    for (int i = 0; i < mTbController.TabCount; i++)
                    {
                        if (i != 9) // skip tbLogOut tab (index 9)
                        {
                            mTbController.TabPages[i].Enabled = true;
                        }
                        else
                        {
                            mTbController.TabPages[i].Text = "Log Out";
                        }
                    }
                }
                mTbController.SelectedIndex = 0;

            }
            else
            {
                //show messagebox
                MaterialMessageBox.Show("invalid Credintionls");
            }

        }

        private void mTbController_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(isAuthenticated+ mTbController.SelectedIndex.ToString());
            // Check if the selected tab is disabled
            //if (!mTbController.TabPages[mTbController.SelectedIndex].Enabled)
            //{
            //    // If it is disabled, set the SelectedIndex property back to the last selected tab
            //    mTbController.SelectedIndex = lastSelectedIndex;
            //}
            //else
            //{
            //    // If it is not disabled, update the last selected tab index
            //    lastSelectedIndex = mTbController.SelectedIndex;
            //}
            if (isAuthenticated == 1 && mTbController.SelectedIndex == 9)
            {
                //Application.Exit();
                ////MessageBox.Show(isAuthenticated + mTbController.SelectedIndex.ToString());
                for (int i = 0; i < mTbController.TabCount; i++)
                {
                    if (i != 9) // skip tbLogOut tab (index 9)
                    {
                        mTbController.TabPages[i].Enabled = false;


                    }
                    else
                    {
                        mtxtUserName.Text = "";
                        mtxtPassword.Text = "";
                        mTbController.TabPages[i].Text = "Log In";
                        isAuthenticated = 0;


                    }
                    mTbController.SelectedIndex = 0;
                }


            }
        }

        private void mBtnSearchIn_Click(object sender, EventArgs e)
        {
            string table = "StockIn";

            // Determine which table to search based on radio button selection
            if (mRadioSin.Checked)
            {
                table = "StockIn";
            }
            else if (mRadioSold.Checked)
            {
                table = "StockOut";
            }

            // Determine the date range based on the selected radio button
            DateTime fromDate;
            DateTime toDate = DateTime.Now; // default to today's date
            if (mRadioToday.Checked)
            {
                fromDate = DateTime.Today;
            }
            else if (mRadioWeek.Checked)
            {
                fromDate = DateTime.Today.AddDays(-7);
            }
            else // no radio button is checked, so use the date pickers
            {
                fromDate = dateTimeFrom.Value;
                toDate = dateTimeTo.Value;
            }

            // Connect to the database
            string connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Query the database for the relevant data
                string query;
                if (table == "StockIn")
                {
                    query = $"SELECT * FROM {table} WHERE Transaction_Date BETWEEN @FromDate AND @ToDate";
                }
                else
                {
                    query = $"SELECT * FROM {table} WHERE Trans_Date >= @FromDate AND Trans_Date <= @ToDate";

                }
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FromDate", fromDate);
                command.Parameters.AddWithValue("@ToDate", toDate);
                
                //SqlDataReader reader = command.ExecuteReader();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                
                //Load the data into the DataGridView
               // DataTable dataTable = new DataTable();
               //dataTable.Load(reader);

               
                sqlDataAdapter.Fill(dataTable);

                connection.Close();
                dataGridInventory.DataSource = dataTable;
            }
        }


        private void mRadioWeek_CheckedChanged(object sender, EventArgs e)
        {
            if (mRadioWeek.Checked)
            {
                mRadioToday.Checked = false;
                dateTimeFrom.Enabled = false;
                dateTimeTo.Enabled = false;
            }
            else
            {
                dateTimeFrom.Enabled = true;
                dateTimeTo.Enabled = true;
            }
        }

        private void mRadioToday_CheckedChanged(object sender, EventArgs e)
        {
            if (mRadioToday.Checked)
            {
                mRadioWeek.Checked = false;
                dateTimeFrom.Enabled = false;
                dateTimeTo.Enabled = false;
            }
            else
            {
                dateTimeFrom.Enabled = true;
                dateTimeTo.Enabled = true;
            }
        }

        private void mRadioSin_CheckedChanged(object sender, EventArgs e)
        {
            if (mRadioSin.Checked)
            {
                mRadioSold.Checked = false;
            }
        }

        private void mRadioSold_CheckedChanged(object sender, EventArgs e)
        {
            if (mRadioSold.Checked) 
            { 
                mRadioSin.Checked = false; 
            }
        }

        private void mRadioCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (mRadioCustom.Checked)
            {
                dateTimeFrom.Enabled = true;
                dateTimeTo.Enabled = true;
            }
        }

       
    }

}

