namespace QLBH_Dion.Util.Entities
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
        int Active { get; set; }
        DateTime CreatedTime { get; set; }
    }
}
