using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;

public partial class k12_bomis_nashik_admission_enquiry : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["birlaopenmindsConnectionString"].ToString());
    bomisDataContext obomisDataContext = new bomisDataContext();
    bomisDBContext obomisDBContext = new bomisDBContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PopulateCategory();
            load_School(128);
            load_Grade();
            //btnSubmit.Attributes.Add("onclick", "return validation('Enter Your Name','Enter Email ID','Enter Mobile','Select School','Select Grade','Enter Address');");
        }

    }
    private void load_School(int schoolId)
    {

        try
        {

            var Schools = obomisDataContext.Usp_Populate_SchoolMaster_ById(Convert.ToInt32(schoolId)).ToList();



            if (Schools.Count > 0)
            {

                ddlSchool.Items.Clear();
                ddlSchool.DataSource = Schools;
                ddlSchool.DataTextField = "SchoolName";
                ddlSchool.DataValueField = "SchoolId";
                ddlSchool.DataBind();
                //ddlSchool.Items.Insert(0, new ListItem("--Select School--", ""));

            }
            else
            {
                ddlSchool.Items.Insert(0, new ListItem("--Select School--", ""));

            }
        }
        catch (Exception ex)
        {
            // ErrorLogging.LogErrorToLogFile(ex, "Error Occured : " + ex.Message);
        }

    }
    private void load_Grade()
    {

        try
        {

            // var Grade = obomisDataContext.Usp_Populate_Grade_Master(Convert.ToInt32(schoolcategory)).ToList();
            var Grade = (from gm in obomisDataContext.Grade_Masters
                         where gm.GradeId >= 2 && gm.GradeId <= 9
                         select gm).ToList();


            if (Grade.Count > 0)
            {

                ddlGrade.Items.Clear();
                ddlGrade.DataSource = Grade;
                ddlGrade.DataTextField = "Grade";
                ddlGrade.DataValueField = "GradeId";
                ddlGrade.DataBind();
                ddlGrade.Items.Insert(0, new ListItem("--Select Grade--", ""));

            }
            else
            {
                ddlGrade.Items.Insert(0, new ListItem("--Select Grade--", ""));

            }
        }
        catch (Exception ex)
        {
            // ErrorLogging.LogErrorToLogFile(ex, "Error Occured : " + ex.Message);
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string msg = string.Empty;
            int? msgno = 0;
            string Strmsg = "";
            string strClientMsg = "";
            var EmailId1 = "";
            var EmailId2 = "";
            var SchoolName = "";
            AdmissionDetail oAdmissionDetail = new AdmissionDetail();
            var EmailDetails = (from sm in obomisDataContext.School_Masters
                                where sm.SchoolId == Convert.ToInt32(ddlSchool.SelectedValue)
                                select sm).Single();
            if (EmailDetails.SchoolEmailId1.ToString() != "")
                EmailId1 = EmailDetails.SchoolEmailId1.ToString();
            if (EmailDetails.SchoolEmailId2.ToString() != "")
                EmailId2 = EmailDetails.SchoolEmailId2.ToString();
            SchoolName = EmailDetails.SchoolName.ToString();
            int result;
            using (con)
            {
                con.Open();
                SqlCommand sql_cmnd = new SqlCommand("USP_GETDUPLICATEENTRIES", con);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@NAME", SqlDbType.NVarChar).Value = txtName.Text.ToString();
                sql_cmnd.Parameters.AddWithValue("@EMAILID", SqlDbType.NVarChar).Value = txtEmailId.Text;
                sql_cmnd.Parameters.AddWithValue("@MOBILENO", SqlDbType.NVarChar).Value = txtMobile.Text.ToString();
                sql_cmnd.Parameters.AddWithValue("@School_Name", SqlDbType.NVarChar).Value = SchoolName.ToString();

                result = Convert.ToInt32(sql_cmnd.ExecuteScalar());
                con.Close();
            }
            if (result < 1)
            {

                try
                {
                    SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["birlaopenmindsConnectionString"].ToString());
                    con1.Open();

                    if (txtName.Text != "")
                        oAdmissionDetail.Name = txtName.Text;
                    if (txtEmailId.Text != "")
                        oAdmissionDetail.EmailId = txtEmailId.Text;

                    oAdmissionDetail.Category = "";
                    // if (ddlSchool.SelectedIndex != 0)
                    oAdmissionDetail.School = SchoolName;
                    if (ddlGrade.SelectedIndex != 0)
                        oAdmissionDetail.Grade = ddlGrade.SelectedItem.ToString();
                    if (txtMobile.Text != "")
                        oAdmissionDetail.MobileNo = txtMobile.Text;
                    if (txtAddress.Text != "")
                        oAdmissionDetail.Address = txtAddress.Text;
                    if (txtQuery.Text != "")
                        oAdmissionDetail.Query = txtQuery.Text;
                    // if (ddlSource.SelectedIndex != 0)
                    // oAdmissionDetail.EnquirySource = ddlSource.SelectedItem.ToString();
                    obomisDBContext.InsertAdmissionDetails(oAdmissionDetail, ref msgno, ref msg);
                    Strmsg = "<table width='540' border='1' align='center' cellpadding='0.5' cellspacing='0.5'>" +
                            "<tr><td height='35' align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'> <b>Name :- </b> </td><td align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'>&nbsp; " + txtName.Text + "&nbsp;</td></tr>" +
                            "<tr><td height='35' align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'> <b>Email Id :- </b> </td><td align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'>&nbsp; " + txtEmailId.Text + "&nbsp;</td></tr>" +
                            "<tr><td height='35' align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'> <b>Mobile No :- </b> </td><td align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'>&nbsp; " + txtMobile.Text + "&nbsp;</td></tr>" +

                             "<tr><td height='35' align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'> <b>School :- </b> </td><td align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'>&nbsp; " + SchoolName + "&nbsp;</td></tr>" +
                             "<tr><td height='35' align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'> <b>Grade :- </b> </td><td align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'>&nbsp; " + ddlGrade.SelectedItem.ToString() + "&nbsp;</td></tr>" +
                            "<tr><td height='35' align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'> <b>Address :- </b> </td><td align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'>&nbsp; " + txtAddress.Text + "&nbsp;</td></tr>" +
                            "<tr><td height='35' align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'> <b>Query :- </b> </td><td align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'>&nbsp; " + txtQuery.Text + "&nbsp;</td></tr>" +
                           // "<tr><td height='35' align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'> <b>Query :- </b> </td><td align='left' style='font-family: Arial, Helvetica,sans-serif; font-size:12px;  text-align:left; line-height:20px;' valign='middle'>&nbsp; " + ddlSource.SelectedItem.ToString() + "&nbsp;</td></tr>" +
                           "<tr><td height='35' align='left' valign='middle' colspan='2'>Thanks & Regards,</td></tr>" +
                            "<tr><td height='35' align='left' valign='middle' colspan='2'>'" + SchoolName + "'</td></tr>" +
                            "</table>";


                    strClientMsg = "<table width='600' border='0' cellpadding='0' cellspacing='0'>" +
                           "<tr><td valign='top' style='font-family: Arial, Helvetica,sans-serif; font-size: 12px;text-align: left; line-height: 20px;'>Dear<b> " + txtName.Text + ",</b></td></tr>" +
                           "<tr><td>&nbsp;</td></tr>" +
                            "<tr><td valign='top' style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; text-align: left; line-height: 20px;'>Thank you for your interest. Our Executive team will get back to you soon.</td></tr>" +
                            "<br>" +
                            "<tr><td valign='top' style='font-family: Arial, Helvetica, sans-serif; font-size: 12px;text-align: left; line-height: 20px;'><p></p></td></tr>" +
                            "<tr><td valign='top' style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; text-align: left; line-height: 20px;'><p><b>Thanks</b></p></td></tr>" +
                            "<tr><td valign='top' style='font-family: Arial, Helvetica, sans-serif; font-size: 12px;text-align: left; line-height: 20px;'><p><b>'" + SchoolName + "'</b></p></td></tr>" +
                            "</table>";
                    System.Net.Mail.SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["smtpServer"].ToString(), 587);
                    smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtpUser"].ToString(), ConfigurationManager.AppSettings["smtpPass"].ToString());
                    using (System.Net.Mail.MailMessage msg1 = new System.Net.Mail.MailMessage())
                    {
                        smtpClient.EnableSsl = true;
                        msg1.From = new MailAddress("contact@birlaopenminds.com", SchoolName);
                        msg1.Subject = "Admission Enquiry from : " + txtEmailId.Text.Trim();
                        msg1.IsBodyHtml = true;
                        msg1.Body = Strmsg;

                        if (EmailId1 != "")
                            msg1.To.Add(EmailId1);
                        if (EmailId2 != "")
                            msg1.CC.Add(EmailId2);



                        smtpClient.Send(msg1);

                    }
                    using (System.Net.Mail.MailMessage msg2 = new System.Net.Mail.MailMessage())
                    {
                        smtpClient.EnableSsl = true;
                        msg2.From = new MailAddress("contact@birlaopenminds.com", SchoolName);
                        msg2.Subject = "Admission Enquiry from " + SchoolName;
                        msg2.IsBodyHtml = true;
                        msg2.Body = strClientMsg;
                        msg2.To.Add(txtEmailId.Text);
                        smtpClient.Send(msg2);
                    }
                    Clear();
                    con1.Close();

                }
                catch (SqlException Ex)
                { }

                Response.Redirect("thank-you.html");
                // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "alert('Thank you for enquiry we will get back to you soon.');", true);
                Clear();
            }

            else
            {

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('You have already made an enquiry for this school today.You can make only one Enquiry in a day.')", true);
                Clear();
            }

        }
        catch (Exception ex)
        { }

    }
    private void Clear()
    {
        txtName.Text = "";
        txtEmailId.Text = "";
        ddlSchool.SelectedIndex = 0;
        ddlGrade.SelectedIndex = 0;
        txtMobile.Text = "";
        txtAddress.Text = "";
        txtQuery.Text = "";
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Clear();
    }
}