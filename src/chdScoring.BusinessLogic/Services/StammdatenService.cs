using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Dtos;
using chdScoring.Contracts.Enums;
using chdScoring.Contracts.Settings;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using chdScoring.DataAccess.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace chdScoring.BusinessLogic.Services
{
    public class StammdatenService(IBaseEntityRepository<Stammdaten> stammDatenRepository, IContextFactory<chdScoringContext> factory, IOptionsMonitor<AppSettings> optionsMonitor) : IStammdatenService
    {
        public async Task<ImageDto> GetImage(int id, CancellationToken cancellationToken)
        {
            using var context = factory.Create();
            var img = await context.Set<Images>().FirstOrDefaultAsync(x => x.Img_Id == id);
            return new()
            {
                Data = img.Img_Data,
                Type = img.Img_Type
            };
        }

        public async Task<StammdatenDto> Get(CancellationToken cancellationToken)
        {
            var data = await stammDatenRepository.FirstOrDefaultAsync(_ => true, cancellationToken);
            return new()
            {
                TournamentName = data.Turnier,
                From = data.Datum,
                To = data.End_datum,
                Finale = data.Finale,
                Final_Durchgang = data.Final_Durchgang,
                Final_Teilnehmer = data.Final_Teilnehmer,
                Img_Id = data.Img_Id,
                Score_Mode = (EScoreMode)data.Score_Mode,
                Edit = data.Edit,
                Fed_Id = data.Fed_Id,
                Federation = data.Federation,
                Judge_Pin = data.Judge_Pin,
                Jury = data.Jury,
                Number = data.Number,
                Org_Leiter = data.Org_Leiter,
                Location = data.Ort,
                Urkunde_Id = data.Urkunde_Id,
                Host = data.Veranstalter,
                HostWeb = data.Veranstalter_web,
                Wettkampf_Leiter = data.Wettkampf_Leiter,
            };
        }

        public Task<string> GetRoundResultPdfUrl(CancellationToken cancellationToken) => Task.FromResult(optionsMonitor.CurrentValue.RoundScorePrintUrl);
    }
}
