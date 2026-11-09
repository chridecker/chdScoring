using chd.Base.Hosting.Contracts.Interfaces;
using chdScoring.Contracts.Interfaces;
using chdScoring.Main.WebServer.Modules;
using CsvHelper;

namespace chdScoring.Main.WebServer.Utils
{
    public static class PilotImport
    {
        public static async Task ImportPilots(this IModuleManager moduleManager,Form parent, Stream fileStream, CancellationToken cancellationToken)
        {
            using var reader = CsvReader.Create(fileStream);

            var service = moduleManager.GetModuleService<IPilotService>(nameof(ServerModule));
            var pilots = await service.GetAllPilots(cancellationToken);
            var countries = await service.GetCountries(cancellationToken);
            var pilotsCount = pilots.Any() ? pilots.Max(s => s.Id) : 0;

            try
            {
                while (await reader.ReadAsync())
                {
                    var name = reader[1].ToString();
                    var lastname = reader[2].ToString();
                    var club = reader[4].ToString();
                    var license = reader[5].ToString();
                    var country = reader[3].ToString();


                    if (!await service.Add(new Contracts.Dtos.PilotDto
                    {
                        Id = ++pilotsCount,
                        Firstname = name,
                        Lastname = lastname,
                        Club = club,
                        License = license,
                        CountryId = countries.FirstOrDefault(c => c.Name == country)?.Id ?? 0,
                    }, CancellationToken.None))
                    {
                        MessageBox.Show(parent, $"Fehler beim Importieren der CSV: Pilot {name} {lastname} konnte nicht hinzugefügt werden.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(parent, $"Fehler beim Importieren der CSV: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
