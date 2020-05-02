using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using McfStockChangeTracker.Extensions;
using McfStockChangeTracker.Services;

namespace McfStockChangeTracker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var io = new FileIOService();

            if (io.WasInitialSetup)
            {
                Console.WriteLine($"Tarvittavat kansiot, alikansiot ja asetustiedostot luotu hakemistoon {io.AppRootFolder}.");
                Console.WriteLine($"Ennen seuraavaa ajoa, tallenna kaupan nimi, apikäyttäjätunnus ja apiavain tiedostoon {io.StoreConfigFile} muodossa:" +
                                  $"\n\nkaupannimi:sposti:apiavain123abc\n\n");
                Console.WriteLine($"Halutessasi syötä käyttäjä-id:t ja niihin yhdistettävät nimet tiedostoon {io.UserConfigFile} muodossa:" +
                                  $"\n\n1:Varasto\n2:Matti\n\n");
            }
            else
            {
                await GetStockChangesInteractively(io);
            }

            Console.WriteLine("Voit nyt sulkea ikkunan painamalla mitä tahansa näppäintä.");
            Console.ReadKey();
        }

        private static async Task GetStockChangesInteractively(FileIOService io)
        {
            var apiSettings = io.GetCredentials();
            var userSettings = io.GetUsers();
            var service = new StockChangeTrackingService(apiSettings, userSettings);
            Console.WriteLine("Tervetuloa StockBuster5000000:aan!\n\n");
            Console.WriteLine("Anna tuotteen tai tuotevariaation yksilöllinen tuotekoodi.");
            Console.WriteLine("Jos haluat hakea tietyn tuotteen kaikkien variaatioiden tapahtumat, anna päätuotteen tuotekoodi.");
            Console.WriteLine("Syötä niin monta koodia kuin haluat. Kaikki tallennetaan omiin CSV-tiedostoihinsa. Anna tyhjä rivi, kun haluat lopettaa.");
            string givenLine = "";
            do
            {
                Console.WriteLine("Anna tuotteen tai tuotevariaation yksilöllinen tuotekoodi.\n> ");
                givenLine = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(givenLine)) break;
                Console.WriteLine("Anna alkupäivämäärä hakua varten muodossa VVVV-KK-PP. Jos et halua määrittää aloitusaikaa, paina enter.\n> ");
                var startInput = Console.ReadLine();
                var startDate = ParseDateInput(startInput);
                Console.WriteLine("Anna loppupäivämäärä hakua varten muodossa VVVV-KK-PP. Jos et halua määrittää lopetusaikaa, paina enter.\n> ");
                var endInput = Console.ReadLine();
                var endDate = ParseDateInput(endInput);
                if (startDate is null)
                {
                    Console.WriteLine("Aloituspäivämäärä ei validi. Haetaan ilman aloituspäivämäärää");
                }
                if (endDate is null)
                {
                    Console.WriteLine("Lopetuspäivämäärä ei validi. Haetaan ilman lopetuspäivämäärää");
                }
                else
                {
                    endDate = endDate.Value.AddDays(1);
                }

                try
                {
                    Console.WriteLine("Haetaan...");
                    var data = await service.SmartQueryForStockChanges(givenLine.Trim(), startDate, endDate);
                    if (data.IsNullOrEmptyCollection())
                    {
                        Console.WriteLine($"Varastotapahtumia ei löytynyt tuotekoodille {givenLine} annetulla aikavälillä");
                        continue;
                    }
                    Console.WriteLine($"Löydetty {data.Count} varastotapahtumaa tuotteelle {data.First().Product} annetulla aikavälillä.");
                    var filename = $"{DateTime.Now:yyyyMMddhhmmss}-{givenLine}.csv";
                    Console.WriteLine($"Kirjoitetaan tapahtumat tiedostoon {Path.Combine(io.OutputFolder, filename)}");
                    await io.WriteToCsv(filename, data);
                    Console.WriteLine("Kirjoitettu.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Jatka antamalla uusi tuotekoodi tai paina enter lopetaaksesi haut.");
                }
            } while (true);
        }

        private static DateTimeOffset? ParseDateInput(string input)
        {
            if (DateTimeOffset.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture.DateTimeFormat,
                DateTimeStyles.AssumeLocal, out var result))
                return result;
            return null;
        }
    }
}
