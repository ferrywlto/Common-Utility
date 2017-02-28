using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Net;
using System.IO;
using System.Collections;

namespace PPCaller
{
    public class PPCaller
    {
        public void call_paypal_post_form_submit() //set input to StringDictionary to max flexibility
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
            //sb.AppendFormat(@"<body>");
            sb.AppendFormat("<form name='form' action='{0}?' method='POST' target='_top'>", ConfigurationManager.AppSettings["paypal_url"]);

            sb.AppendFormat("<input type='hidden' ID='cmd' name='cmd' value='_xclick' />");
            sb.AppendFormat("<input type='hidden' ID='business' name='business' Value='{0}' />", ConfigurationManager.AppSettings["business"]);
            sb.AppendFormat("<input type='hidden' ID='item_name' name='item_name' value='Test Class' />");
            sb.AppendFormat("<input type='hidden' ID='amount' name='amount' value='1' />");
            sb.AppendFormat("<input type='hidden' ID='currency_code' name='currency_code' value='HKD'/>");
            sb.AppendFormat("<input type='hidden' ID='custom' name='custom' value='xxx'/>");
            sb.AppendFormat("<input type='submit' ID='btnSubmit' value='Buy'/>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            response.Write(sb.ToString());
            response.End();
        }
        public void call_paypal_get_redirect()
        {
            HttpResponse response = HttpContext.Current.Response;

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}?cmd=_xclick&business={1}&item_name={2}&amount=1&currency_code=HKD&custom=xxx",
                ConfigurationManager.AppSettings["paypal_url"],
                ConfigurationManager.AppSettings["business"],
                "Test Class");
            response.Redirect(sb.ToString());
        }
        public void v()
        {
            HttpRequest in_request = HttpContext.Current.Request;
            HttpWebRequest out_request = (HttpWebRequest)(WebRequest.Create(ConfigurationManager.AppSettings["paypal_url"]));

            //Set values for the request back
            out_request.Method = "POST";
            out_request.ContentType = "application/x-www-form-urlencoded";
            Byte[] param = in_request.BinaryRead(in_request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);
            string ipnPost = strRequest;
            strRequest += "&cmd=_notify-validate";
            out_request.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://url:port#"))
            //req.Proxy = proxy

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(out_request.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();

            StreamReader streamIn = new StreamReader(out_request.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            // logging ipn messages... be sure that you give write
            // permission to process executing this code
            //Dim logPathDir As String = ResolveUrl("Messages")
            //Dim logPath As String = String.Format("0\\1.txt",
            //Server.MapPath(logPathDir), DateTime.Now.Ticks)
            //File.WriteAllText(logPath, ipnPost)
            //
            //Tell Paypal IPN gateway that the IPN has been processed and no need to resend.
            HttpResponse response = HttpContext.Current.Response;
            response.ClearHeaders();
            response.StatusCode = 200;
            response.Flush();
            response.End();

            if(strResponse == "VERIFIED"){

                //check the payment_status is Completed
                //check that txn_id has not been previously processed
                //check that receiver_email is your Primary PayPal email
                //check that payment_amount/payment_currency are correct
                //process payment
                //string grandOrderID = in_request.Form["custom"].ToString();

                //ArrayList sqlBatch = Application[grandOrderID];
                //WriteAllOrdersToDB(sqlBatch);

                //sqlBatch.Clear();
                //sqlBatch = null;
                //Application(grandOrderID) = null;
                //grandOrderID = "";
            }
            else if(strResponse == "INVALID"){
                //log for manual investigation
                Console.WriteLine("Invalid response received");
            }
            else{
                //log response/ipn data for manual investigation
            }

            //Response.Write("done")
        }
    }

}
