using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Websocket.Samples
{
    public class DataServer:BaseServer
    {

        private IList<Customer> mCustomers;

        public class Customer
        {
           
            public string CustomerID { get; set; }
          
            public string CompanyName { get; set; }
          
            public string ContactName { get; set; }
           
            public string ContactTitle { get; set; }
          
            public string Address { get; set; }
       
            public string City { get; set; }
           
            public string PostalCode { get; set; }
            
            public string Country { get; set; }
           
            public string Phone { get; set; }
         
            public string Fax { get; set; }

            public static IList<Customer> List()
            {
                DataSet1.CustomersDataTable cdt = new DataSet1.CustomersDataTable();
                cdt.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "customers.xml");
                List<Customer> items = new List<Customer>();
                foreach (DataSet1.CustomersRow crow in cdt.Rows)
                {
                    Customer cust = new Customer();
                    cust.Address = crow.Address;
                    cust.City = crow.City;
                    cust.CompanyName = crow.CompanyName;
                    cust.ContactName = crow.ContactName;
                    cust.ContactTitle = crow.ContactTitle;
                    cust.Country = crow.Country;
                    cust.CustomerID = crow.CustomerID;
                    cust.Fax = crow.Fax;
                    cust.Phone = crow.Phone;
                    cust.PostalCode = crow.PostalCode;
                    items.Add(cust);

                }
                return items;
            }
        }

        public override void Opened(Beetle.Express.IServer server)
        {
            base.Opened(server);
            mCustomers = Customer.List();
        }

        class SearchResult
        {
            public SearchResult()
            {
                Items = new List<Customer>();
            }
            public int Pages
            {
                get;
                set;
            }
            public IList<Customer> Items
            {
                get;
                set;
            }
        }

        [Command("search")]
        public void Search(Beetle.Express.IChannel channel, string id, string command, JToken token)
        {
            int size = 10;
            int pageindex = token["pageindex"].ToObject<int>();
            SearchResult result = new SearchResult();
            result.Pages = mCustomers.Count / size;
            if (mCustomers.Count % size > 0)
                result.Pages++;
            for (int i = pageindex * size; i < mCustomers.Count; i++)
            {
                result.Items.Add(mCustomers[i]);
                if (result.Items.Count >= size)
                    break;
            }
            Send(channel, id, command, result);
        }
    }
}
