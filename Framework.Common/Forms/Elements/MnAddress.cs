using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.Framework.Common.Forms.JsonFormatter;
using Mn.Framework.Common.Forms.Model;
using Newtonsoft.Json;

namespace Mn.Framework.Common.Forms
{
    [JsonConverter(typeof(MnElementConverter))]
    public class MnAddress : MnBaseElement
    {
        public MnAddress()
        {
            AutoCompleteAddress = new PostalAddressAutoCompleteViewModel();
        }

        [NotMapped]
        public bool IsRequired { get; set; }

        [DisplayName("Address type")]
        public AddressType AddressType { get; set; }

        [DisplayName("Address")]
        [JsonIgnore]
        public PostalAddressAutoCompleteViewModel AutoCompleteAddress { get; set; }

        [DisplayName("Address line 1")]
        public string AddressLine1 { get; set; }

        [DisplayName("Address line 2")]
        public string AddressLine2 { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Zip code")]
        //[RegularExpression(@"^[0-9]*$", ErrorMessage = "Zip code invalid")]
        public string Zip { get; set; }

        [Display(Name = "Country")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "You must select a country.")]
        public string Country { get; set; }

        public override object GetValue()
        {
            return string.Join(",", new List<string>() { AddressLine1, AddressLine2, City, Zip, State, Country });
        }
        public override void SetValue(object value)
        {
            var addressItems = value.ToString().Split(',');
            if (addressItems.Count() == 6)
            {
                AddressLine1 = addressItems[0];
                AddressLine2 = addressItems[1];
                City = addressItems[2];
                Zip = addressItems[3].Trim();
                State = addressItems[4];
                Country = addressItems[5];
            }
        }
    }

    public enum AddressType : byte
    {
        [Description("US address")]
        US = 0,
        [Description("International address")]
        INTERNATIONAL = 1
    }
}
