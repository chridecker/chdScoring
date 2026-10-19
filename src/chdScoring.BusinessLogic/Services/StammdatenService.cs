using chdScoring.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.Contracts.Dtos;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using chdScoring.DataAccess.Contracts.Repositories.Base;
using chdScoring.DataAccess.EFCore;
using Microsoft.EntityFrameworkCore;

namespace chdScoring.BusinessLogic.Services
{
    public class StammdatenService(IBaseEntityRepository<Stammdaten> stammDatenRepository, IContextFactory<chdScoringContext> factory) : IStammdatenService
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
                To = data.End_datum
            };
        }
    }
}
