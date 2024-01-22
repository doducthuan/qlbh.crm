using QLBH_Dion.Models;
using System.Net.Mail;
using System.Net;

namespace QLBH_Dion.Util.Email
{
	public class EmailUtil
	{
		//public static string EMAIL_CREDENTIAL_NAME = "lacvietauctionvn@gmail.com";
		//public static string EMAIL_CREDENTIAL_PASSWORD = "Novatic25";
		public static string EMAIL_CREDENTIAL_NAME = AppSettingConfig.Instance.Get<string>("EmailConfig:Username");
		public static string EMAIL_CREDENTIAL_PASSWORD = AppSettingConfig.Instance.Get<string>("EmailConfig:Password");
		public static string EMAIL_CREDENTIAL_NAME_OFFICE365 = AppSettingConfig.Instance.Get<string>("EmailConfig:Office365Username");
		public static string EMAIL_CREDENTIAL_PASSWORD_OFFICE365 = AppSettingConfig.Instance.Get<string>("EmailConfig:Office365Password");
		public static string EMAIL_SENDER_NAME = AppSettingConfig.Instance.Get<string>("EmailConfig:NameSender");
		public static string currentUrl = SystemConstant.DEFAULT_URL;
        private static string currentUrlLogo = "";
        public static QLBH_Response SendEmail(string recipients, string subject, string body)
		{
			//var novaticResponse = SendEmailOffice365(recipients, subject, body);
			var novaticResponse = SendEmailOffice365(recipients, subject, body);
			return novaticResponse;
		}
		public static QLBH_Response SendEmailOffice365(string recipients, string subject, string body)
		{
			var novaticResponse = QLBH_Response.SUCCESS();

			string emailUsername = EMAIL_CREDENTIAL_NAME_OFFICE365;
			string emailPassword = EMAIL_CREDENTIAL_PASSWORD_OFFICE365;
			string senderName = EMAIL_SENDER_NAME;
			//string recipients = "hung.nguyen@novatic.vn,nguyenhungbk92@gmail.com";
			//string subject = "Hello!";
			//string body = "<h1>Hello World</h1>";

			try
			{
				var toAddresses = recipients.Split(',');
				foreach (var to in toAddresses)
				{
					int tryAgain = 20;
					bool failed = false;
					do
					{
						Thread.Sleep(3000);
						new Thread(() =>
						{
							SmtpClient client = new SmtpClient();
							client.Host = "smtp.office365.com";
							client.Port = 587;
							client.UseDefaultCredentials = false; // This require to be before setting Credentials property
							client.DeliveryMethod = SmtpDeliveryMethod.Network;
							client.Credentials = new NetworkCredential(emailUsername, emailPassword); // you must give a full email address for authentication 
							client.TargetName = "STARTTLS/smtp.office365.com"; // Set to avoid MustIssueStartTlsFirst exception
							client.EnableSsl = true;// Set to avoid secure connection exception
							client.Timeout = 1000000000;                //Timeout = 1000000000
							MailMessage message = new MailMessage();
							message.From = new MailAddress(emailUsername, senderName); // sender must be a full email address
							message.Subject = subject;
							message.IsBodyHtml = true;
							message.Body = body;
							message.BodyEncoding = System.Text.Encoding.UTF8;
							message.SubjectEncoding = System.Text.Encoding.UTF8;
							message.To.Add(to.Trim());
							try
							{
								client.Send(message);
							}
							catch (Exception ex)
							{
								//throw;
								failed = true;
								tryAgain--;
								var exception = ex.Message.ToString();
							}
						}).Start();
					} while (failed && tryAgain != 0);
				}
			}
			catch (Exception e)
			{
				novaticResponse = QLBH_Response.BAD_REQUEST(e);
			}


			return novaticResponse;
		}
		public static QLBH_Response SendEmailGmail(string recipients, string subject, string body)
		{
			var novaticResponse = QLBH_Response.SUCCESS();
            string emailUsername = EMAIL_CREDENTIAL_NAME;
            string emailPassword = EMAIL_CREDENTIAL_PASSWORD;
            string senderName = EMAIL_SENDER_NAME;
            //string emailUsername = "novaticvn@outlook.com";
            //string emailPassword = "Novatic@25";
            //string recipients = "hung.nguyen@novatic.vn,nguyenhungbk92@gmail.com";
            try
			{
				var toAddresses = recipients.Split(',');
                foreach (var to in toAddresses)
                {
                    int tryAgain = 20;
                    bool failed = false;
                    do
                    {
                        Thread.Sleep(3000);
                        new Thread(() =>
                        {
                            SmtpClient client = new SmtpClient();
                            client.Host = "smtp.gmail.com";
                            client.Port = 587;
                            client.UseDefaultCredentials = false; // This require to be before setting Credentials property
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.Credentials = new NetworkCredential(emailUsername, emailPassword); // you must give a full email address for authentication 
                            //client.TargetName = "STARTTLS/smtp.office365.com"; // Set to avoid MustIssueStartTlsFirst exception
                            client.EnableSsl = true;// Set to avoid secure connection exception
                            client.Timeout = 1000000000;                //Timeout = 1000000000
                            MailMessage message = new MailMessage();
                            message.From = new MailAddress(emailUsername, senderName); // sender must be a full email address
                            message.Subject = subject;
                            message.IsBodyHtml = true;
                            message.Body = body;
                            message.BodyEncoding = System.Text.Encoding.UTF8;
                            message.SubjectEncoding = System.Text.Encoding.UTF8;
                            message.To.Add(to.Trim());
                            try
                            {
                                client.Send(message);
                            }
                            catch (Exception ex)
                            {
                                //throw;
                                failed = true;
                                tryAgain--;
                                var exception = ex.Message.ToString();
                            }
                        }).Start();
                    } while (failed && tryAgain != 0);
                
    //            foreach (var to in toAddresses)
				//{
				//	new Thread(() =>
				//	{

				//		// send  email 
				//		var client = new SmtpClient("smtp.gmail.com", 587)
				//		{
				//			Credentials = new NetworkCredential(EmailUtil.EMAIL_CREDENTIAL_NAME, EmailUtil.EMAIL_CREDENTIAL_PASSWORD),
				//			EnableSsl = true
				//		};

				//		MailMessage msg = new MailMessage(EmailUtil.EMAIL_CREDENTIAL_NAME, to, subject, body);
				//		msg.IsBodyHtml = true;
				//		client.Send(msg);
				//	}).Start();
				}
			}
			catch (Exception e)
			{
				novaticResponse = QLBH_Response.BAD_REQUEST(e);
			}
			return novaticResponse;
		}
		// Email quên mật khẩu
		public static string EmailForgotPassword(string username, string stringRandom)
		{
			string content = @"
                          <!DOCTYPE html>
                        <html>
                        <head>

                          <meta charset='utf-8'>
                          <meta http-equiv='x-ua-compatible' content='ie=edge'>
                          <title>Password Reset</title>
                          <meta name='viewport' content='width=device-width, initial-scale=1'>
                          <link href='https://fonts.googleapis.com/css?family=Montserrat&display=swap' rel='stylesheet'>


                          <style type='text/css'>
  
                          body,
                          table,
                          td,
                          a {
                            -ms-text-size-adjust: 100%; /* 1 */
                            -webkit-text-size-adjust: 100%; /* 2 */
                          }

  
                          table,
                          td {
                            mso-table-rspace: 0pt;
                            mso-table-lspace: 0pt;
                          }

  
                          img {
                            -ms-interpolation-mode: bicubic;
                          }

  
                          a[x-apple-data-detectors] {
                            font-family: inherit !important;
                            font-size: inherit !important;
                            font-weight: inherit !important;
                            line-height: inherit !important;
                            color: inherit !important;
                            text-decoration: none !important;
                          }

  
                          div[style*='margin: 16px 0;'] {
                            margin: 0 !important;
                          }

                          body {
                            width: 100% !important;
                            height: 100% !important;
                            padding: 0 !important;
                            margin: 0 !important;
                          }

  
                          table {
                            border-collapse: collapse !important;
                          }

                          a {
                            color: #1a82e2;
                          }

                          img {
                            height: auto;
                            line-height: 100%;
                            text-decoration: none;
                            border: 0;
                            outline: none;
                          }
                          .passwordValue{
                            display: none;
                          }

                          .showEmailButton:active{
                            background: green !important;
                          }
                          .showEmailButton:active .passwordValue{
                            display: block !important;
                          }
                          .showEmailButton:active .passwordDummy{
                            display: none !important;
                          }
                          </style> 
                        </head>
                        <body style='background-color: #e9ecef;'>

                          <!-- start preheader -->
                          <div class='preheader' style='display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;'>
    
                          </div>
                          <!-- end preheader -->

                          <!-- start body -->
                          <table border='0' cellpadding='0' cellspacing='0' width='100%'>

                            <!-- start logo -->
                            <tr>
                              <td align='center' bgcolor='#e9ecef'>
        
                                <table border='0' cellpadding='0' cellspacing='0' width='90%' style='margin: 0 5%'>
                                  <tr>
                                    <td align='center' valign='top' style='padding: 36px 24px;'>
                                      <a href='" + currentUrl + @"' target='_blank' style='display: inline-block;'>
                                        <img src='" + currentUrl + @"" + currentUrlLogo + @"' alt='' border='0'   style='display: block;min-width: 48px;width: 200px;'>
                                      </a>
                                    </td>
                                  </tr>
                                </table>
        
                              </td>
                            </tr>
                            <!-- end logo -->

                            <!-- start hero -->
                            <tr>
                              <td align='center' bgcolor='#e9ecef'>
                                <!--[if (gte mso 9)|(IE)]>
        
                                <![endif]-->
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'  >
                                  <tr>
                                    <td align='left' bgcolor='#ffffff' style='padding: 36px 24px 0; font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif; border-top: 3px solid #d4dadf;'>
                                      <h1 style='margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;'>Yêu cầu thay đổi mật khẩu</h1>
                                    </td>
                                  </tr>
                                </table>
        
                              </td>
                            </tr>
                            <!-- end hero -->

                            <!-- start copy block -->
                            <tr>
                              <td align='center' bgcolor='#e9ecef'>
                                <!--[if (gte mso 9)|(IE)]>
        
                                <![endif]-->
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;' class='showEmailButton'>

                                  <!-- start copy -->
                                  <tr>
                                    <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 16px; line-height: 24px;'>
                                      <p style='margin: 0; padding-bottom: 10px;font-size: 18px!important;'>Xin chào " + username + @"!</p>
                                      <p style='margin: 0; padding-bottom: 10px;font-size: 18px!important;'>Hệ thống đấu giá Lạc Việt đã nhận được yêu cầu thay đổi mật khẩu của quý khách.</p>
                                      <p style='margin: 0; padding-bottom: 10px;font-size: 18px!important;'>Xin hãy click vào đường dẫn sau để đổi mật khẩu. Lưu ý link chỉ có hiệu lực trong vòng 24 giờ.</p>
                                    </td>
                                  </tr>
                                  <!-- end copy -->

                                  <!-- start button -->
                                  <tr>
                                    <td align='left' bgcolor='#ffffff'>
                                      <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                        <tr>
                                          <td align='center' bgcolor='#ffffff' style='padding: 12px;'>
                                            <table border='0' cellpadding='0' cellspacing='0'>
                                              <tr>
                                                <td align='center' bgcolor='#1a82e2' style='border-radius: 6px;'>
                                                  <a href='" + stringRandom + @"' target='_blank' style='display: inline-block; padding: 16px 36px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;'>Đổi mật khẩu</a>
                                                </td>
                                              </tr>
                                            </table>
                                          </td>
                                        </tr>
                                      </table>
                                    </td>
                                  </tr> 
                                  <!-- end button -->

                                  <!-- start copy -->
                                  <tr>
                                    <td align='left' bgcolor='#ffffff' style='padding: 20px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 16px; line-height: 24px;'>
             
                                    </td>
                                  </tr>
          
                                  <tr>
                                    <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf'>
                                      <p style='margin: 0;'>Trân trọng!<br> CÔNG TY ĐẤU GIÁ HỢP DANH LẠC VIỆT <br> Địa chỉ: số 49 Văn Cao, phường Liễu Giai, quận Ba Đình, TP. Hà Nội <br> Số điện thoại: 0243.211.5234/0867.523.488 </p>
                                    </td>
                                  </tr>

                                </table>
       
                              </td>
                            </tr>

                            <tr>
                              <td align='center' bgcolor='#e9ecef' style='padding: 24px;'>
        
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>

                                  <!-- start permission -->
                                  <tr>
                                    <td align='center' bgcolor='#e9ecef' style='padding: 12px 24px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 14px; line-height: 20px; color: #666;'>
                                      <p style='margin: 0;'>Bạn nhận được email này vì chúng tôi đã nhận được yêu cầu từ tài khoản của bạn.</p>
                                    </td>
                                  </tr>
                                  <!-- end permission -->

                                  <!-- start unsubscribe -->
                                  <tr>
                                    <td align='center' bgcolor='#e9ecef' style='padding: 12px 24px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 14px; line-height: 20px; color: #666;'>
                                      <p style='margin: 0;'>Copyright " + DateTime.UtcNow.Year + @" <a href='" + currentUrl + @"' target='_blank'>Lạc Việt Auction</a></p>
                                      <p style='margin: 0;'>All rights reserved.</p>
                                    </td>
                                  </tr>
                                  <!-- end unsubscribe -->

                                </table>
        
                              </td>
                            </tr>
                            <!-- end footer -->

                          </table>
                          <!-- end body -->

                        </body>
                        </html>

                                                                               ";
			return content;
		}
        // Email đăng ký tài khoản
        public static string EmailRegister(string username, string stringRandom)
        {
            string content = @"
                          <!DOCTYPE html>
                        <html>
                        <head>

                          <meta charset='utf-8'>
                          <meta http-equiv='x-ua-compatible' content='ie=edge'>
                          <title>Password Reset</title>
                          <meta name='viewport' content='width=device-width, initial-scale=1'>
                          <link href='https://fonts.googleapis.com/css?family=Montserrat&display=swap' rel='stylesheet'>


                          <style type='text/css'>
  
                          body,
                          table,
                          td,
                          a {
                            -ms-text-size-adjust: 100%; /* 1 */
                            -webkit-text-size-adjust: 100%; /* 2 */
                          }

  
                          table,
                          td {
                            mso-table-rspace: 0pt;
                            mso-table-lspace: 0pt;
                          }

  
                          img {
                            -ms-interpolation-mode: bicubic;
                          }

  
                          a[x-apple-data-detectors] {
                            font-family: inherit !important;
                            font-size: inherit !important;
                            font-weight: inherit !important;
                            line-height: inherit !important;
                            color: inherit !important;
                            text-decoration: none !important;
                          }

  
                          div[style*='margin: 16px 0;'] {
                            margin: 0 !important;
                          }

                          body {
                            width: 100% !important;
                            height: 100% !important;
                            padding: 0 !important;
                            margin: 0 !important;
                          }

  
                          table {
                            border-collapse: collapse !important;
                          }

                          a {
                            color: #1a82e2;
                          }

                          img {
                            height: auto;
                            line-height: 100%;
                            text-decoration: none;
                            border: 0;
                            outline: none;
                          }
                          .passwordValue{
                            display: none;
                          }

                          .showEmailButton:active{
                            background: green !important;
                          }
                          .showEmailButton:active .passwordValue{
                            display: block !important;
                          }
                          .showEmailButton:active .passwordDummy{
                            display: none !important;
                          }
                          </style> 
                        </head>
                        <body style='background-color: #e9ecef;'>

                          <!-- start preheader -->
                          <div class='preheader' style='display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;'>
    
                          </div>
                          <!-- end preheader -->

                          <!-- start body -->
                          <table border='0' cellpadding='0' cellspacing='0' width='100%'>

                            <!-- start logo -->
                            <tr>
                              <td align='center' bgcolor='#e9ecef'>
        
                                <table border='0' cellpadding='0' cellspacing='0' width='90%' style='margin: 0 5%'>
                                  <tr>
                                    <td align='center' valign='top' style='padding: 36px 24px;'>
                                      <a href='" + currentUrl + @"' target='_blank' style='display: inline-block;'>
                                        <img src='" + currentUrl + @"" + currentUrlLogo + @"' alt='' border='0'   style='display: block;min-width: 48px;width: 200px;'>
                                      </a>
                                    </td>
                                  </tr>
                                </table>
        
                              </td>
                            </tr>
                            <!-- end logo -->

                            <!-- start hero -->
                            <tr>
                              <td align='center' bgcolor='#e9ecef'>
                                <!--[if (gte mso 9)|(IE)]>
        
                                <![endif]-->
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'  >
                                  <tr>
                                    <td align='left' bgcolor='#ffffff' style='padding: 36px 24px 0; font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif; border-top: 3px solid #d4dadf;'>
                                      <h1 style='margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;'>Yêu cầu xác thực tài khoản</h1>
                                    </td>
                                  </tr>
                                </table>
        
                              </td>
                            </tr>
                            <!-- end hero -->

                            <!-- start copy block -->
                            <tr>
                              <td align='center' bgcolor='#e9ecef'>
                                <!--[if (gte mso 9)|(IE)]>
        
                                <![endif]-->
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;' class='showEmailButton'>

                                  <!-- start copy -->
                                  <tr>
                                    <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 16px; line-height: 24px;'>
                                      <p style='margin: 0; padding-bottom: 10px;font-size: 18px!important;'>Xin chào " + username + @"!</p>
                                      <p style='margin: 0; padding-bottom: 10px;font-size: 18px!important;'>Happy Smile yêu cầu xác thực tài khoản của bạn.</p>
                                      <p style='margin: 0; padding-bottom: 10px;font-size: 18px!important;'>Xin hãy click vào đường dẫn sau để xác thực tài khoản. Lưu ý link chỉ có hiệu lực trong vòng 24 giờ.</p>
                                    </td>
                                  </tr>
                                  <!-- end copy -->

                                  <!-- start button -->
                                  <tr>
                                    <td align='left' bgcolor='#ffffff'>
                                      <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                                        <tr>
                                          <td align='center' bgcolor='#ffffff' style='padding: 12px;'>
                                            <table border='0' cellpadding='0' cellspacing='0'>
                                              <tr>
                                                <td align='center' bgcolor='#1a82e2' style='border-radius: 6px;'>
                                                  <a href='" + stringRandom + @"' target='_blank' style='display: inline-block; padding: 16px 36px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;'>Xác thực tài khoản</a>
                                                </td>
                                              </tr>
                                            </table>
                                          </td>
                                        </tr>
                                      </table>
                                    </td>
                                  </tr> 
                                  <!-- end button -->

                                  <!-- start copy -->
                                  <tr>
                                    <td align='left' bgcolor='#ffffff' style='padding: 20px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 16px; line-height: 24px;'>
             
                                    </td>
                                  </tr>
          
                                  <tr>
                                    <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf'>
                                      <p style='margin: 0;'>Trân trọng!<br> CÔNG TY ĐẤU GIÁ HỢP DANH LẠC VIỆT <br> Địa chỉ: số 49 Văn Cao, phường Liễu Giai, quận Ba Đình, TP. Hà Nội <br> Số điện thoại: 0243.211.5234/0867.523.488 </p>
                                    </td>
                                  </tr>

                                </table>
       
                              </td>
                            </tr>

                            <tr>
                              <td align='center' bgcolor='#e9ecef' style='padding: 24px;'>
        
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>

                                  <!-- start permission -->
                                  <tr>
                                    <td align='center' bgcolor='#e9ecef' style='padding: 12px 24px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 14px; line-height: 20px; color: #666;'>
                                      <p style='margin: 0;'>Bạn nhận được email này vì chúng tôi đã nhận được yêu cầu từ tài khoản của bạn.</p>
                                    </td>
                                  </tr>
                                  <!-- end permission -->

                                  <!-- start unsubscribe -->
                                  <tr>
                                    <td align='center' bgcolor='#e9ecef' style='padding: 12px 24px; font-family: Roboto,RobotoDraft,Helvetica,Arial,sans-serif; font-size: 14px; line-height: 20px; color: #666;'>
                                      <p style='margin: 0;'>Copyright " + DateTime.UtcNow.Year + @" <a href='" + currentUrl + @"' target='_blank'>Lạc Việt Auction</a></p>
                                      <p style='margin: 0;'>All rights reserved.</p>
                                    </td>
                                  </tr>
                                  <!-- end unsubscribe -->

                                </table>
        
                              </td>
                            </tr>
                            <!-- end footer -->

                          </table>
                          <!-- end body -->

                        </body>
                        </html>

                                                                               ";
            return content;
        }
    }
}
