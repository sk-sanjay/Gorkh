
using System;

namespace WebSite.Helpers
{
    public class ForgotPasswordMessage
    {
        public static string Message(string link, string name, bool reset = true)
        {
            //$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            string sb = "";
            if (reset)
            {
                sb = $@"
                <html>
                <head>
                <link rel=""stylesheet"" href=""https://pro.fontawesome.com/releases/v5.10.0/css/all.css"" integrity=""sha384-AYmEC3Yw5cVb3ZcuHtOA93w35dYTsvhLPVnYs9eStHfGJvOvKxVfELGroGkvsg+p"" crossorigin=""anonymous"" />
                </head>
                <body>
                <h4> Dear Ms./ Mr. {name}</h4> 
                <p>Kindly follow below link to reset password –</p>
                <p><a style=""display:inline-block; color:white; background-color:blue; padding:4px 8px; border-radius:50px; text-decoration:none;"" href=""{link}"" target=""_blank""> Password reset Link</a></p>
                <h4><b>Password Guidelines:</b></h4>
                <p>The following are the guidelines for creating a Strong Password:</p>
                <p style = 'padding-left:20px'>1.	The Password should contain a minimum of 6 characters and a maximum of 28 characters.
                </p><p style = 'padding-left:20px'>2.	The Passwords are case sensitive i.e. Upper Case e.g. PASSWORD123 is differentiated from Lower Case e.g. password123.
                </p><p style = 'padding-left:20px'>3.	The Password should contain at least one special character.
                </p><p style = 'padding-left:20px'>4.	Only the following special characters ~, !, @, #, $, %, ^, &, *,_ are accepted as part of the Password.
                </p><p style = 'padding-left:20px'>5.	Password should be alphanumeric i.e. should contain both digits and alphabets
                </p>
                    </body>
                </html>";
            }
            else
            {
                //$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."

                sb =
                string.Format(" <h4> Dear Ms./ Mr. {0}</h4> " + Environment.NewLine +
                                     "<h4><b>Greetings from IiPM!</b></h4>" + Environment.NewLine +
                                     "<p>You are already a registered user on IiPM Training Management System (TMS) portal https://iipmindianoil.in/. </p>" + Environment.NewLine +
                                     "<p>If you do not remember your log-in password, you may generate it using FORGOT PASSWORD option in TMS portal.</p>" + Environment.NewLine +
                                     "<h4><b>Please note:</b></h4>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>a)	The username is Employee code (not changeable) and the password (changeable)</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>b)	This is a one-time registration. The same log-in credentials can be used for accessing TMS portal for all programmes conducted by IiPM.</p>" + Environment.NewLine +
                                     "<h4><b>Through the TMS portal you would be able to:</b></h4>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>a)	Provide feedback (Overall programme & Sessions)</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>b)	Provide feedback (Hostel / Catering – For programmes conducted through physical mode)</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>c)	Download pre-reads/ reading material (Cases/articles/Questionnaires) shared by faculty during training</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>d)	Upload Projects/Assignments</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>f)	Provide extended Feedback (if any)</p>" + Environment.NewLine + Environment.NewLine +
                                     "<br/><p><b>Regards</b></p>" + Environment.NewLine +
                                     "<p><b>Team IiPM</b></p>" + Environment.NewLine +
                                     "</body>" + Environment.NewLine +
                                     "</html>", name);
            }
            return sb;
        }
        public static string ParticipantMessage(string link, string name, bool reset = true,string ISIocl = "")
        {
            //$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
            string sb = "";
            var nottxt = "The username is Employee code (not changeable) and the password (changeable)";
            if (ISIocl == "Non-Iocl" || ISIocl == "Ex-Iocl") nottxt = "The username is not changeable and the password is changeable.";
            if (reset)
            {
                sb = $@"
                <html>
                <head>
                <link rel=""stylesheet"" href=""https://pro.fontawesome.com/releases/v5.10.0/css/all.css"" integrity=""sha384-AYmEC3Yw5cVb3ZcuHtOA93w35dYTsvhLPVnYs9eStHfGJvOvKxVfELGroGkvsg+p"" crossorigin=""anonymous"" />
                </head>
                <body>
                <h4> Dear Ms./ Mr. {name}</h4> 
                <p>Kindly follow below link to reset password –</p>
                <p><a style=""display:inline-block; color:white; background-color:blue; padding:4px 8px; border-radius:50px; text-decoration:none;"" href=""{link}"" target=""_blank""> Password reset Link</a></p>
                <h4><b>Password Guidelines:</b></h4>
                <p>The following are the guidelines for creating a Strong Password:</p>
                <p style = 'padding-left:20px'>1.	The Password should contain a minimum of 6 characters and a maximum of 28 characters.
                </p><p style = 'padding-left:20px'>2.	The Passwords are case sensitive i.e. Upper Case e.g. PASSWORD123 is differentiated from Lower Case e.g. password123.
                </p><p style = 'padding-left:20px'>3.	The Password should contain at least one special character.
                </p><p style = 'padding-left:20px'>4.	Only the following special characters ~, !, @, #, $, %, ^, &, *,_ are accepted as part of the Password.
                </p><p style = 'padding-left:20px'>5.	Password should be alphanumeric i.e. should contain both digits and alphabets
                </p>
                    </body>
                </html>";
            }
            else
            {
                //$"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."

                sb =
                string.Format(" <h4> Dear Ms./ Mr. {0}</h4> " + Environment.NewLine +
                                     "<h4><b>Greetings from IiPM!</b></h4>" + Environment.NewLine +
                                     "<p>You are already a registered user on IiPM Training Management System (TMS) portal https://iipmindianoil.in/. </p>" + Environment.NewLine +
                                     "<p>If you do not remember your log-in password, you may generate it using FORGOT PASSWORD option in TMS portal.</p>" + Environment.NewLine +
                                     "<h4><b>Please note:</b></h4>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>a)	 " + nottxt + "	</p>"  + Environment.NewLine +
                                     "<p style='padding-left:20px'>b)	This is a one-time registration. The same log-in credentials can be used for accessing TMS portal for all programmes conducted by IiPM.</p>" + Environment.NewLine +
                                     "<h4><b>Through the TMS portal you would be able to:</b></h4>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>a)	Provide feedback (Overall programme & Sessions)</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>b)	Provide feedback (Hostel / Catering – For programmes conducted through physical mode)</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>c)	Download pre-reads/ reading material (Cases/articles/Questionnaires) shared by faculty during training</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>d)	Upload Projects/Assignments</p>" + Environment.NewLine +
                                     "<p style='padding-left:20px'>f)	Provide extended Feedback (if any)</p>" + Environment.NewLine + Environment.NewLine +
                                     "<br/><p><b>Regards</b></p>" + Environment.NewLine +
                                     "<p><b>Team IiPM</b></p>" + Environment.NewLine +
                                     "</body>" + Environment.NewLine +
                                     "</html>", name);
            }
            return sb;
        }
    }
}
