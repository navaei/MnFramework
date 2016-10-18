using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.Framework.Common.Forms.Model
{    

    /// <summary>
    /// Used with Google Places Autocomplete
    /// We do not require the fields (except for AutoCompletedAddress) in order to allow the model
    /// be used in various scenarios.
    /// </summary>
    /// <remarks>
    /// FinalAddres field allows user to make slight changes to the computed address
    /// Locality is city
    /// Administrative_area_level_1 is state
    /// Administrative_area_level_2 is county
    /// </remarks>
    public class PostalAddressAutoCompleteViewModel
    {
        private bool _showMap;

        [Display(Name = "Location address")]
        [MinLength(10, ErrorMessage = "Please specify the complete {0}")]
        [StringLength(256, ErrorMessage = "Address is too long")]
        public string AutoCompletedAddress { get; set; }

        [StringLength(64, ErrorMessage = "{0} is too long")]
        public string Street_number { get; set; }

        [StringLength(128, ErrorMessage = "{0} is too long")]
        public string Route { get; set; }

        [StringLength(64, ErrorMessage = "{0} is too long")]
        public string Locality { get; set; }

        [StringLength(64, ErrorMessage = "{0} is too long")]
        public string Administrative_area_level_1 { get; set; }

        [StringLength(64, ErrorMessage = "{0} is too long")]
        public string Administrative_area_level_2 { get; set; }

        [StringLength(64, ErrorMessage = "{0} is too long")]
        public string Postal_code { get; set; }

        [StringLength(64, ErrorMessage = "{0} is too long")]
        public String Country { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

        /// <summary>
        /// empty constructor to prevent "No parameterless constructor defined for this object" error.
        /// This exception occurs when a model has no parameterless constructor.
        /// Danial
        /// </summary>
        public PostalAddressAutoCompleteViewModel()
        {

        }

        public PostalAddressAutoCompleteViewModel(bool showMap)
        {
            _showMap = showMap;
            Lat = 37.708333;
            Lng = -122.280278;
        }

        //public PostalAddressAutoCompleteViewModel(PostalAddress model, bool showMap)
        //{
        //    _showMap = showMap;
        //    Lat = model.Lat;
        //    Lng = model.Lng;
        //    Street_number = model.StreetNumber;
        //    Route = model.Route;
        //    Locality = model.City;
        //    Administrative_area_level_1 = model.State;
        //    Country = model.Country;
        //    Postal_code = model.Zip;
        //    AutoCompletedAddress = model.AutoCompletedAddress;
        //}

        public bool IsMapVisible()
        {
            return _showMap;
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// We do not enfore street number b/c clubs might be in a public library for example.
        /// </remarks>
        /// <returns>
        /// True if all fields except Administrative_area_level_2 and Street_number are present. 
        /// Note AutoCompletedAddress field will be enforced by framework.
        /// </returns>
        public bool IsAddressComplete()
        {
            return !(string.IsNullOrEmpty(Administrative_area_level_1) ||
                         string.IsNullOrEmpty(Locality));

            // Allow empty street numbers or routes, for example ny public library, ny, ny

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Ensure address is complete first.
        /// </remarks>
        /// <returns></returns>
        public string GetCompleteAddress()
        {
            string countryMod = Country;

            switch (Country)
            {
                case "US":
                    countryMod += "A";
                    break;
                default:
                    break;
            }

            // when address contain country name 
            if (AutoCompletedAddress.Contains(", United States"))
            {
                AutoCompletedAddress = AutoCompletedAddress.Replace(", United States", "");
                return AutoCompletedAddress + " " + Postal_code + "," + " " + countryMod;
            }
            else
            {
                return AutoCompletedAddress + " " + Postal_code + "," + " " + countryMod;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// The street field does not a corresponding field in this view model and is not set.
        /// </remarks>
        /// <returns></returns>
        //public PostalAddress ToPostalAddress()
        //{
        //    var add = new PostalAddress
        //    {
        //        Lat = Lat,
        //        Lng = Lng,
        //        StreetNumber = Street_number,
        //        Route = Route,
        //        City = Locality,
        //        State = Administrative_area_level_1,
        //        Country = Country,
        //        Zip = Postal_code,
        //        AutoCompletedAddress = AutoCompletedAddress,
        //        Address = GetCompleteAddress()
        //    };

        //    return add;
        //}
    }
}
