public interface ISqlInteractionMomentsRepository
{
    Task<List<TimeSpan>> SelectInteractionMoments();
}