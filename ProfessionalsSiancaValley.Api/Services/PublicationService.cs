using Microsoft.EntityFrameworkCore;
using ProfessionalsSiancaValley.Api.Data;

namespace ProfessionalsSiancaValley.Api.Services
{
    public class PublicationService
    {
        private readonly AppDbContext _context;

        public PublicationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateIdPublicacion()
        {
            // Obtener último registro
            var last = await _context.Publications
                .OrderByDescending(p => p.Id_Publicacion)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (last != null)
            {
                // Ej: PUB0005 → 5
                var numberPart = last.Id_Publicacion.Replace("PUB", "");

                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"PUB{nextNumber:D4}";
        }
    }
}