public interface ISqlInteractionMomentsRepository
{
    Task<List<InteractieMoment>> SelectInteractionMoments(int robotID);
    void DeleteMoment(int id);
    void AddMoment(int id,TimeOnly time);
}