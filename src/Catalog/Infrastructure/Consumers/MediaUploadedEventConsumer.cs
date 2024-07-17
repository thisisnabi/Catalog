using Media.Infrastructure.IntegrationEvents;

namespace Catalog.Infrastructure.Consumers;

public class MediaUploadedEventConsumer(CatalogDbContext catalogDbContext) : IConsumer<MediaUploadedEvent>
{
    private readonly CatalogDbContext _catalogDbContext = catalogDbContext;
    public async Task Consume(ConsumeContext<MediaUploadedEvent> context)
    {
        var catalogItem = await _catalogDbContext.CatalogItems
                            .Include(x => x.Medias)
                            .FirstOrDefaultAsync(x => x.Slug == context.Message.CatalogId);

        // TODO: Log error
        if (catalogItem is null)
            return;

        catalogItem.AddMedia(context.Message.FileName, context.Message.Url);
        await _catalogDbContext.SaveChangesAsync();
    }
}
