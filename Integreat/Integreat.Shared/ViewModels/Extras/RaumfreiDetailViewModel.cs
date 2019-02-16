using Integreat.Model.Extras.Raumfrei;
using Xamarin.Forms;

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
        public string AccommodationTitle => Offer.FormData.Accommodation.Title;
        public string AccommodationLocation => Offer.FormData.Accommodation.Location;
        public string TotalArea => Offer.FormData.Accommodation.TotalArea.ToString("0.0' qm'");
        public string TotalRooms => Offer.FormData.Accommodation.TotalRooms.ToString();
        public string Rooms => string.Join(", ", Offer.FormData.Accommodation.TranslatedRooms);
        public string MoveInDate => Offer.FormData.Accommodation.MoveInDate.ToShortDateString();

        public string CostInformationHeader => CostInformationHeaderText;

        public string BaseRent => Offer.FormData.Costs.BaseRent == 0 ? "Keine" : Offer.FormData.Costs.BaseRent.ToString("0' € monatlich'");
        public string RunningCosts => Offer.FormData.Costs.RunningCosts == 0 ? "Keine" : Offer.FormData.Costs.RunningCosts.ToString("0' € monatlich'");
        public string RunningServices => string.Join(", ", Offer.FormData.Costs.TranslatedRunningServices);
        public string NotRunningServices => string.Join(", ", Offer.FormData.Costs.TranslatedNotRunningServices);
        public string HotWaterInRunningCosts => Offer.FormData.Costs.HotWaterInRunningCosts ? "Ja" : "Nein";
        public string AdditionalCosts => Offer.FormData.Costs.AdditionalCosts == 0 ? "Keine" : Offer.FormData.Costs.AdditionalCosts.ToString("0' € monatlich'");
        public string AdditionalServices => string.Join(", ", Offer.FormData.Costs.TranslatedAdditionalServices);
        public string NotAdditionalServices => string.Join(", ", Offer.FormData.Costs.TranslatedNotAdditionalServices);

        public string LandlordInformationHeader => LandlordInformationHeaderText;
        public string Email => Offer.EmailAddress;
        public string Phone => Offer.FormData.Landlord.PhoneNumber;
        public string Name => Offer.FormData.Landlord.FullName;
        public string Address { get; set; }

        public Command OnEmailTapCommand { get; } = new Command(Email =>
        {
            Device.OpenUri(new System.Uri("mailto:" + Email));
        });

        public Command OnPhoneTapCommand { get; } = new Command(Phone =>
        {
            Device.OpenUri(new System.Uri("tel:" + Phone));
        });
    }
}