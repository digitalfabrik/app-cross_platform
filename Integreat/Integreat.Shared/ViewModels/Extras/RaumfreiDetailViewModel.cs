using Integreat.Shared.Models.Extras.Raumfrei;

namespace Integreat.Shared.ViewModels
{
    public class RaumfreiDetailViewModel : BaseViewModel
    {
        private const string AccomodationInformationHeaderText = "Mietobjekt";
        private const string CostInformationHeaderText = "Mietkosten";
        private const string LandlordInformationHeaderText = "Kontaktdaten";
        public RaumfreiOffer Offer { get; }

        public RaumfreiDetailViewModel(RaumfreiOffer offer)
        {
            Offer = offer;
            Title = "Mietangebot";
            HeaderImage = "raumfrei_logo";
        }

        public string HeaderImage { get; set; }

        public string AccomodationInformationHeader => AccomodationInformationHeaderText;
        public string TotalArea => Offer.FormData.Accommodation.TotalArea.ToString("F1");
        public string TotalRooms => Offer.FormData.Accommodation.TotalRooms.ToString();
        public string MoveInDate => Offer.FormData.Accommodation.MoveInDate.ToShortDateString();

        public string CostInformationHeader => CostInformationHeaderText;
        public string BaseRent => Offer.FormData.Costs.BaseRent.ToString("C");
        public string AdditionalCosts => Offer.FormData.Costs.AdditionalCosts.ToString("C");

        public string LandlordInformationHeader => LandlordInformationHeaderText;
        public string Email => Offer.EmailAddress;
        public string Phone => Offer.FormData.Landlord.PhoneNumber;
        public string Name => Offer.FormData.Landlord.FullName;
        public string Address { get; set; }
    }
}
