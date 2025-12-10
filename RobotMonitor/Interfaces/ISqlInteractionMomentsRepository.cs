public interface ISqlInteractionMomentsRepository
{
    Task<List<InteractieMoment>> SelectInteractionMoments();
    void DeleteMoment(int id);
    void AddMoment(int id,TimeOnly time);
}