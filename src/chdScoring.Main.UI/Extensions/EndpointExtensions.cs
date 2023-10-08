using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chdScoring.DataAccess.Contracts.Domain;
using chdScoring.DataAccess.Contracts.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace chdScoring.Main.UI.Extensions
{
    public static class EndpointExtensions
    {
        public static IEndpointRouteBuilder MapChdScoring(this IEndpointRouteBuilder app, string endpoint)
        {
            var group = app.MapGroup(endpoint).WithName(endpoint);

            group.MapGet($"image/{{id}}", async (int id, IImageRepository imageRepository, CancellationToken cancellationToken) =>
            {
                try
                {
                    var imgObj = await imageRepository.FindById(id, cancellationToken);
                    return Results.File(imgObj.Img_Data, imgObj.Img_Type);
                }
                catch (Exception ex)
                {
                   return Results.BadRequest(ex);
                }
            })
                .WithName("image")
                .WithOpenApi();

            return app;
        }
    }
}
