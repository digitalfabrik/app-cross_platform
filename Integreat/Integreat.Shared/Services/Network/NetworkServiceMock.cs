using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Integreat.Shared.Models;

namespace Integreat.Shared.Services.Network
{
    public class NetworkServiceMock : INetworkService
    {
        Task<string> INetworkService.IsServerAlive()
        => Task.Factory.StartNew(() => "some string");

        Task<Collection<Disclaimer>> INetworkService.GetDisclaimers(Language language, Location location, UpdateTime time)
        => Task.Factory.StartNew(() => new Collection<Disclaimer> {
            new Disclaimer {
                Id=100, Title="Feedback, Kontakt und mögliches Engagement", Type="disclaimer", Status="publish", Modified=new DateTime(2016, 01, 23),
                Description="Auszug", Content="Noch kein HTML! Muss noch auf HTML View angepasst werden",
                ParentId=null, Order=0, Thumbnail=null, Author=new Author("login", "Mux", "Mastermann"), AutoTranslated=false,
                AvailableLanguages=new List<AvailableLanguage> ()
            }
        });

        Task<Collection<Page>> INetworkService.GetPages(Language language, Location location, UpdateTime time)
        => Task.Factory.StartNew(() => new Collection<Page>{
            new Page {
                Id=135, Title="Arztbesuch", Type="page", Status="publish", Modified=new DateTime(2016, 01, 23),
                Description="Wenn Sie in Augsburg angekommen sind, bekommen Sie einen  Berechtigungsschein für den Allgemeinarzt und für den Zahnarzt. Sie brauchen für ein Quartal einen Schein. 1. Quartal: Januar, Februar, März 2. Quartal: April, Mai, Juni 3. Quartal: Juli, August, September 4. Quartal: Oktober, November, Dezember Ein Behandlungsschein ist bis Ende des Quartals gültüg. Für das nächste",
                Content="<p>Wenn Sie in Augsburg angekommen sind, bekommen Sie einen  <a href=\"http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/ankunftsinformationen/die-wichtigsten-dokumente-und-schritte-nach-ankunft/\">Berechtigungsschein</a> für den Allgemeinarzt und für den Zahnarzt. Sie brauchen für ein Quartal einen Schein.</p><p><table style=\"border-color: #0a0909;height: 47px\" border=\"1\" width=\"590\"></p><p><tbody></p><p><tr></p><p><td style=\"border-color: #000000\"></p><p><div>1. Quartal: Januar, Februar, März</p><p>2. Quartal: April, Mai, Juni</p><p>3. Quartal: Juli, August, September</div></p><p><div>4. Quartal: Oktober, November, Dezember</div></td></p><p></tr></p><p></tbody></p><p></table></p><p>Ein Behandlungsschein ist bis Ende des Quartals gültüg. Für das nächste Quartal erhalten Sie wieder einen Schein. Dieser kommt automatisch per Post. Wenn der Schein nicht kommen sollte, gehen Sie zum <a href=\"http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/ankunftsinformationen/termin-beim-amt-fuer-soziale-leistungen-asl/\">Amt für Soziale Leistungen</a>. Mit dem Behandlungsschein ist der Arztbesuch für Sie kostenlos. Allerdings gibt es bestimmte Einschränkungen.</p><p></p><p><strong>Zu welchem Arzt gehen Sie?</strong></p><p>Sie können sich den Arzt frei auswählen. Aber innerhalb eines Quartals dürfen Sie für weitere Arzttermine den Arzt, bei dem Sie Ihren Behandlungsschein abgegeben haben, nicht wechseln. Falls Sie Hilfe bei der Suche nach einem geeigneten Arzt brauchen, fragen Sie Ihren <a href=\"http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/asylberatung-und-helpdesks/helferkreise/\">Helferkreis</a>, die <a href=\"http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/asylberatung-und-helpdesks/sprechstunde-fuer-dezentral-untergebrachte/\">Sprechstunde </a>für dezentral untergebrachte Asylsuchende oder die <a href=\"http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/kinder-und-familie/beratungsstellen-und-hilfsangebote/\">Asylsozialberatungsstellen</a>.</p><p></p><p>Arztpraxen haben normalerweise von Montag bis Freitag geöffnet. Die Öffnungszeiten sind je nach Arzt unterschiedlich. Es ist gut, einen Termin beim Arzt zu vereinbaren. Wenn Sie nachts einen Arzt brauchen, aber es kein Notfall ist, können Sie hier anrufen:</p><p><table></p><p><tbody></p><p><tr></p><p><td width=\"236\">Ärztlicher Bereitschaftsdienst</td></p><p><td width=\"190\">Telefon 0821 – 116 117</td></p><p><td width=\"190\">nur abends und an Wochenenden</td></p><p></tr></p><p><tr></p><p><td width=\"236\">Zahnärztlicher Notdienst</td></p><p><td width=\"190\"><a href=\"http://www.zahnarzt-notdienst-augsburg.de\" target=\"_blank\">www.zahnarzt-notdienst-augsburg.de</a></td></p><p><td width=\"190\">nur abends und an Wochenenden</td></p><p></tr></p><p></tbody></p><p></table></p><p>&nbsp;</p><p></p><p><strong><u>Allgemeinarzt und Kinderarzt</u></strong></p><p></p><p>Wenn Sie sich seelisch oder körperlich nicht gesund fühlen, gehen Sie zuerst zum Allgemeinarzt. Kinder müssen zu einem Kinderarzt gehen. Dieser Arzt übernimmt die Grundversorgung und ist der erste Ansprechpartner bei allen gesundheitlichen Beschwerden. Er führt eine erste Untersuchung durch und entscheidet über die weitere Behandlung.</p><p></p><p>Wenn eine Behandlung durch einen Facharzt notwendig ist, wird der Allgemeinarzt oder der Kinderarzt Sie dorthin überweisen. Mit der Überweisung des Allgemeinarztes oder des Kinderarztes bekommen Sie im <a href=\"http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/de/ankunftsinformationen/termin-beim-amt-fuer-soziale-leistungen-asl/\">Amt für Soziale Leistungen</a> einen Krankenbehandlungsschein für den Facharzt.</p><p></p><p>&nbsp;</p><p></p><p><strong><u>Fachärzte / Spezialisten</u></strong></p><p></p><p>Fachärzte sind auf ein medizinisches Gebiet spezialisiert und übernehmen nach Überweisung durch den Allgemeinarzt die weitere Behandlung.</p><p></p><p>Sollte eine Operation notwendig sein, lassen Sie sich von Ihrem Arzt einen Kostenvoranschlag für die Operationskosten erstellen. Dieser Kostenvoranschlag muss durch das Sozialamt <strong>vor</strong> der geplanten Operation genehmigt werden, ansonsten werden die Kosten nicht übernommen. Gleiches gilt für psychotherapeutische Behandlungen.</p><p></p><p>&nbsp;</p><p></p><p>&nbsp;</p>",
                ParentId=null, Order=0, Thumbnail="http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/09/patient1.png",
                Author=new Author("login", "Mux", "Mastermann"), AutoTranslated=false,
                AvailableLanguages=new List<AvailableLanguage> ()
            }
        });

        Task<HttpResponseMessage> INetworkService.GetPagesDebug(Language language, Location location, UpdateTime time)
        => Task.Factory.StartNew(() => new HttpResponseMessage());

        Task<Collection<EventPage>> INetworkService.GetEventPages(Language language, Location location, UpdateTime time)
        => Task.Factory.StartNew(() => new Collection<EventPage>());

        Task<Collection<Location>> INetworkService.GetLocations()
        => Task.Factory.StartNew(() => new Collection<Location>{
            new Location (0, "Augsburg",
                "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//wp-content//uploads//sites//2//2015//10//cropped-Logo-Stadt_Augsburg-rotgruen-RGB.jpg",
                "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/",
                "Es schwäbelt", "yellow", "http://vmkrcmar21.informatik.tu-muenchen.de/wordpress/augsburg/wp-content/uploads/sites/2/2015/11/cropped-Augsburg.jpg",
                0, 0, false)
        });

        Task<Collection<Language>> INetworkService.GetLanguages(Location location)
        => Task.Factory.StartNew(() => new Collection<Language>{
            new Language (0, "de", "Deutsch", "http://vmkrcmar21.informatik.tu-muenchen.de//wordpress//augsburg//wp-content//plugins//sitepress-multilingual-cms//res//flags//de.png")
        });

        Task<string> INetworkService.SubscribePush(Location location, string regId)
        => Task.Factory.StartNew(() => "" /* TODO: need to lookup what goes here */);

        Task<string> INetworkService.UnsubscribePush(Location location, string regId)
        => Task.Factory.StartNew(() => "" /* TODO: need to lookup what goes here */);
    }
}
