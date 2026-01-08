using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkillsInternational
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True");

        private void RegistrationForm_Load(object sender, EventArgs e)
        {
            LoadRegNos();
        }

        private void LoadRegNos()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT regNo FROM Registration", con);
                SqlDataReader dr = cmd.ExecuteReader();
                cmbRegNo.Items.Clear();
                while (dr.Read())
                {
                    cmbRegNo.Items.Add(dr["regNo"].ToString());
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading RegNos: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string gender = rbMale.Checked ? "Male" : "Female";

                string query = "INSERT INTO Registration VALUES (@regNo, @firstName, @lastName, @dob, @gender, @address, @email, @mobile, @home, @parent, @nic, @contact)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@regNo", int.Parse(cmbRegNo.Text)); // Assuming user types a new ID
                cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@lastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@dob", dtpDOB.Value);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@mobile", int.Parse(txtMobilePhone.Text));
                cmd.Parameters.AddWithValue("@home", int.Parse(txtHomePhone.Text));
                cmd.Parameters.AddWithValue("@parent", txtParentName.Text);
                cmd.Parameters.AddWithValue("@nic", txtNic.Text);
                cmd.Parameters.AddWithValue("@contact", int.Parse(txtContactNo.Text));

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record Added Successfully", "Register Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRegNos(); // Refresh the list
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string gender = rbMale.Checked ? "Male" : "Female";

                string query = "UPDATE Registration SET firstName=@firstName, lastName=@lastName, dateOfBirth=@dob, gender=@gender, address=@address, email=@email, mobilePhone=@mobile, homePhone=@home, parentName=@parent, nic=@nic, contactNo=@contact WHERE regNo=@regNo";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@regNo", int.Parse(cmbRegNo.Text));
                cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@lastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@dob", dtpDOB.Value);
                cmd.Parameters.AddWithValue("@gender", gender);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@mobile", int.Parse(txtMobilePhone.Text));
                cmd.Parameters.AddWithValue("@home", int.Parse(txtHomePhone.Text));
                cmd.Parameters.AddWithValue("@parent", txtParentName.Text);
                cmd.Parameters.AddWithValue("@nic", txtNic.Text);
                cmd.Parameters.AddWithValue("@contact", int.Parse(txtContactNo.Text));

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record Updated Successfully", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbRegNo.Text = "";
            txtFirstName.Clear();
            txtLastName.Clear();
            dtpDOB.Value = DateTime.Now;
            rbMale.Checked = false;
            rbFemale.Checked = false;
            txtAddress.Clear();
            txtEmail.Clear();
            txtMobilePhone.Clear();
            txtHomePhone.Clear();
            txtParentName.Clear();
            txtNic.Clear();
            txtContactNo.Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure, Do you really want to Delete this Record...?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string query = "DELETE FROM Registration WHERE regNo = @regNo";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@regNo", int.Parse(cmbRegNo.Text));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Record Deleted Successfully", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields and refresh dropdown
                    btnClear_Click(sender, e);
                    LoadRegNos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    if (con.State == ConnectionState.Open) con.Close();
                }
            }
        }

        private void cmbRegNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM Registration WHERE regNo = @regNo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@regNo", cmbRegNo.Text);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtFirstName.Text = dr["firstName"].ToString();
                    txtLastName.Text = dr["lastName"].ToString();
                    dtpDOB.Value = Convert.ToDateTime(dr["dateOfBirth"]);

                    if (dr["gender"].ToString() == "Male") rbMale.Checked = true;
                    else rbFemale.Checked = true;

                    txtAddress.Text = dr["address"].ToString();
                    txtEmail.Text = dr["email"].ToString();
                    txtMobilePhone.Text = dr["mobilePhone"].ToString();
                    txtHomePhone.Text = dr["homePhone"].ToString();
                    txtParentName.Text = dr["parentName"].ToString();
                    txtNic.Text = dr["nic"].ToString();
                    txtContactNo.Text = dr["contactNo"].ToString();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
