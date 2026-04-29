using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PlantManagement.Comm;

namespace PlantManagement.Api;

public static class LampColorApi
{
    public static void MapLampColorEndpoints(this WebApplication app)
    {
        app.MapPost("/api/lamps/{id:int}/color", (int id, SetLampColorRequest req, ILampColorService service) =>
        {
            if (string.IsNullOrWhiteSpace(req.Color))
            {
                return Results.BadRequest("color is required.");
            }

            service.SetColor(id, req.Color.Trim());
            return Results.Ok(new { lampId = id, color = req.Color.Trim() });
        });

        app.MapGet("/api/lamps/{id:int}/color", (int id, ILampColorService service) =>
        {
            var color = service.GetColor(id);
            return color is null
                ? Results.NotFound()
                : Results.Ok(new { lampId = id, color });
        });

        app.MapGet("/api/lamps", (ILampColorService service) =>
        {
            var ids = service.GetRegisteredIds();
            return Results.Ok(new { registeredLampIds = ids });
        });
    }
}

public record SetLampColorRequest(string Color);
